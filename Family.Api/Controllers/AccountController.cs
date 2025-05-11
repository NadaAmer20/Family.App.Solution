using Azure.Core;
using Family.Core.DTOs;
using Family.Core.DTOs.Identity;
using Family.Core.Entities;
using Family.Core.Identity;
using Family.Core.Repository.Interfaces;
using Family.Core.Services.Interfaces;
using Family.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Family.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        private readonly IGenericRepository<RegistrationRequest> _registrationRequestRepo;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IEmailService emailService,
            IConfiguration config,
            IGenericRepository<RegistrationRequest> registrationRequestRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _config = config;
            _registrationRequestRepo = registrationRequestRepo;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                // Check if there's a pending request
                var request = await _registrationRequestRepo.GetBySpecification(
                    new RegistrationRequestSpecification(r => r.Email == loginDto.Email && !r.IsApproved));

                if (request != null)
                {
                    return Unauthorized("Your registration is pending admin approval");
                }

                return Unauthorized("Invalid email");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid password");

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user),
                IsAdmin = isAdmin
            };
        }
        // Update the Register method to include Person data
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return BadRequest("Email already in use");
            }

            var existingRequest = await _registrationRequestRepo.GetBySpecification(
                new RegistrationRequestSpecification(r => r.Email == registerDto.Email && !r.IsApproved));

            if (existingRequest != null)
            {
                return BadRequest("There's already a pending registration request for this email");
            }

            var isAdminRegistering = await IsCurrentUserAdmin();

            if (isAdminRegistering)
            {
                var user = new AppUser
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email,

                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded) return BadRequest(result.Errors);

                await _userManager.AddToRoleAsync(user, "User");

                return Ok(new { message = "Registration successful" });
            }
            else
            {
                var request = new RegistrationRequest
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                };

                await _registrationRequestRepo.AddAsync(request);
                await NotifyAdminAboutNewRequest(request);

                return Ok(new { message = "Registration request submitted. Please wait for admin approval." });
            }
        }

        // Update the ApproveRequest method
        [HttpPost("approve-request/{requestId}")]
        public async Task<ActionResult> ApproveRequest(int requestId)
        {
            var request = await _registrationRequestRepo.GetByIdAsync(requestId);
            if (request == null) return NotFound("Request not found");
            if (request.IsApproved) return BadRequest("Request already approved");

            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return BadRequest("Email is already in use by another account");
            }

            var tempPassword = GenerateTemporaryPassword();

            var user = new AppUser
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                UserName = request.Email,
            };

            var result = await _userManager.CreateAsync(user, tempPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "User");

            request.IsApproved = true;
            request.ApprovalDate = DateTime.UtcNow;
            request.ApprovedBy = User.Identity?.Name;
            await _registrationRequestRepo.UpdateAsync(request);
            await SendApprovalEmail(user, tempPassword);

            return Ok(new { message = "Request approved successfully" });
        }

        private async Task<bool> IsCurrentUserAdmin()
        {
            // Implement logic to check if current user is admin
            // This depends on your authentication setup
            var user = await _userManager.GetUserAsync(User);
            return user != null && await _userManager.IsInRoleAsync(user, "Admin");
        }

        private async Task NotifyAdminAboutNewRequest(RegistrationRequest request)
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            // Plain text version (for email clients that don't support HTML)
            string messageWeb = @"
    New Registration Request
    -----------------------
    Dear Admin,
    
    A new registration request requires your approval:
    
    Name: {DisplayName}
    Email: {Email}
    Request Date: {RequestDate}
    
    Please review this request in the admin panel.
    
    Best regards,
    Family App Team";

            // HTML version
            string htmlMessage = @"
    <h2>New Registration Request</h2>
    <p>Dear Admin,</p>
    <p>A new registration request requires your approval:</p>
    <ul>
        <li><strong>Name:</strong> {DisplayName}</li>
        <li><strong>Email:</strong> {Email}</li>
        <li><strong>Request Date:</strong> {RequestDate}</li>
    </ul>
    <p>Please review this request in the admin panel.</p>
    <p>Best regards,<br>Family App Team</p>";

            var placeholders = new Dictionary<string, string>
    {
        { "DisplayName", request.DisplayName },
        { "Email", request.Email },
        { "RequestDate", request.RequestDate.ToString("f") }
    };

            foreach (var admin in admins)
            {
                await _emailService.SendEmailAsync(
                    toEmail: admin.Email,
                    subject: "New Registration Request - Action Required",
                    body: htmlMessage,          
                    messageWeb: messageWeb,       
                    placeholders: placeholders);  
            }
        }

        [HttpGet("pending-requests")]
       // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<RegistrationRequestDto>>> GetPendingRequests()
        {
            var requests = await _registrationRequestRepo.ListAsync(
                new BaseSpecification<RegistrationRequest>(r => !r.IsApproved));

            return Ok(requests.Select(r => new RegistrationRequestDto
            {
                Id = r.Id,
                DisplayName = r.DisplayName,
                Email = r.Email,
                RequestDate = r.RequestDate,
                IsApproved = r.IsApproved
            }));
        }

 
        [HttpPost("reject-request/{requestId}")]
        public async Task<ActionResult> RejectRequest(int requestId, [FromBody] string? reason = null)
        {
            var request = await _registrationRequestRepo.GetByIdAsync(requestId);
            if (request == null) return NotFound("Request not found");
            if (request.IsApproved) return BadRequest("Request already approved");

            await _registrationRequestRepo.DeleteAsync(request);

            await SendRejectionEmail(request, reason);

            return Ok(new { message = "Request rejected successfully" });
        }
        private string GenerateTemporaryPassword()
        {
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "1234567890";
            const string special = "!@#$%^&*";

            var random = new Random();
            var chars = new List<char>
    {
        lower[random.Next(lower.Length)],
        upper[random.Next(upper.Length)],
        digits[random.Next(digits.Length)],
        special[random.Next(special.Length)]
    };

            string allChars = lower + upper + digits + special;
            for (int i = chars.Count; i < 8; i++)
            {
                chars.Add(allChars[random.Next(allChars.Length)]);
            }

            return new string(chars.OrderBy(x => random.Next()).ToArray());
        }
        private async Task SendApprovalEmail(AppUser user, string tempPassword)
        {
            // Plain text version
            var messageWeb = @"
Your Registration Has Been Approved
----------------------------------
Hello {DisplayName},

Your registration request has been approved by the administrator.

Your temporary password is: {TempPassword}

Please login and change your password immediately.

Best regards,
Family App Team";

            // HTML version
            var htmlMessage = @"
<h2>Your Registration Has Been Approved</h2>
<p>Hello {DisplayName},</p>
<p>Your registration request has been approved by the administrator.</p>
<p>Your temporary password is: <strong>{TempPassword}</strong></p>
<p>Please login and change your password immediately.</p>
<p>Best regards,<br>Family App Team</p>";

            var placeholders = new Dictionary<string, string>
    {
        { "DisplayName", user.DisplayName },
        { "TempPassword", tempPassword }
    };

            await _emailService.SendEmailAsync(
                toEmail: user.Email,
                subject: "Registration Approved - Family App",
                body: htmlMessage,
                messageWeb: messageWeb,
                placeholders: placeholders
            );
        }

        private async Task SendRejectionEmail(RegistrationRequest request, string? reason)
        {
            var messageWeb = @"
Your Registration Request
-------------------------
Hello {DisplayName},

We regret to inform you that your registration request has been rejected.
{Reason}

Best regards,
Family App Team";

            var htmlMessage = @"
<h2>Your Registration Request</h2>
<p>Hello {DisplayName},</p>
<p>We regret to inform you that your registration request has been rejected.</p>
{Reason}
<p>Best regards,<br>Family App Team</p>";

            var reasonHtml = string.IsNullOrEmpty(reason) ? "" : $"<p>Reason: {reason}</p>";
            var reasonText = string.IsNullOrEmpty(reason) ? "" : $"Reason: {reason}";

            var placeholders = new Dictionary<string, string>
    {
        { "DisplayName", request.DisplayName },
        { "Reason", reasonHtml } 
    };

            var placeholdersText = new Dictionary<string, string>
    {
        { "DisplayName", request.DisplayName },
        { "Reason", reasonText }  
    };

            var placeholdersCombined = new Dictionary<string, string>
    {
        { "DisplayName", request.DisplayName },
        { "Reason", string.IsNullOrEmpty(reason) ? "" : $"Reason: {reason}" }
    };

            await _emailService.SendEmailAsync(
                toEmail: request.Email,
                subject: "Registration Request Status - Family App",
                body: htmlMessage,
                messageWeb: messageWeb,
                placeholders: placeholdersCombined
            );
        }

        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
                if (user == null)
                    return Ok(new { message = "If your email is registered, you will receive password reset instructions." });

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = $"{Request.Scheme}://{Request.Host}/reset-password?email={WebUtility.UrlEncode(user.Email)}&token={WebUtility.UrlEncode(token)}";

                try
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Reset Your Password - Family App",
                        GetPasswordResetEmailTemplate(user.DisplayName, resetLink));

                    return Ok(new { message = "Password reset instructions have been sent to your email." });
                }
                catch (Exception ex)
                {
                    // Log the error details
                    return StatusCode(500, new { message = "Failed to send reset email. Please try again later." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }

        private string GetPasswordResetEmailTemplate(string userName, string resetLink)
        {
            return $@"
        <html>
            <body style='font-family: Arial, sans-serif;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #333;'>Reset Your Password</h2>
                    <p>Hello {userName},</p>
                    <p>You have requested to reset your password. Please click the button below to set a new password:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='background-color: #4CAF50; color: white; padding: 12px 25px; 
                                  text-decoration: none; border-radius: 3px; display: inline-block;'>
                            Reset Password
                        </a>
                    </div>
                    <p>If you didn't request this, please ignore this email.</p>
                    <p>The link will expire in 24 hours.</p>
                    <p>Best regards,<br>Family App Team</p>
                </div>
            </body>
        </html>";
        }

        [HttpPost("resetpassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                if (user == null)
                    return BadRequest("Invalid request");

                var result = await _userManager.ResetPasswordAsync(
                    user,
                    resetPasswordDto.Token,
                    resetPasswordDto.NewPassword);

                if (!result.Succeeded)
                    return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });

                // Send confirmation email
                var emailBody = $@"
                <h2>Password Reset Successful</h2>
                <p>Hello {user.DisplayName},</p>
                <p>Your password has been successfully reset.</p>
                <p>If you didn't make this change, please contact us immediately.</p>
                <p>Best regards,<br>Family App Team</p>";

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Password Reset Successful - Family App",
                    emailBody);

                return Ok(new { message = "Password has been reset successfully" });
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }
    }
}
