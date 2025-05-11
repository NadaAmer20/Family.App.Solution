using Family.Api.Helpers;
using Family.Core.DTOs;
using Family.Core.Entities;
using Family.Core.Identity;
using Family.Core.Repository.Interfaces;
using Family.Core.Specifications.BranchSpecifications;
using Family.Core.Specifications.ClanSpecifications;
using Family.Repository.Data;
using Family.Repository.Response;
using Family.Service.DTOS.Family.Service.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Family.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly IGenericRepository<Clan> _clanRepo;
        private readonly IGenericRepository<Branch> _branchRepo;
        private readonly UserManager<AppUser> _userManager;

        public FamilyController(
            IGenericRepository<Clan> clanRepo,
            IGenericRepository<Branch> branchRepo,
            UserManager<AppUser> userManager)
        {
            _clanRepo = clanRepo;
            _branchRepo = branchRepo;
            _userManager = userManager;
        }

        #region Clan Operations

        [HttpGet("clans")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClanReturnDto>>>> GetAllClans()
        {
            try
            {
                var spec = new ClanWithBranchesSpecification();
                var clans = await _clanRepo.ListAsync(spec);
                return Ok(ApiResponse<IEnumerable<ClanReturnDto>>.Success(clans.ToReturnDtos()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ClanReturnDto>>.Fail("An error occurred while retrieving clans", new List<string> { ex.Message }));
            }
        }

        [HttpGet("clans/{clanId}")]
        public async Task<ActionResult<ApiResponse<ClanReturnDto>>> GetClanById(int clanId)
        {
            try
            {
                var spec = new ClanWithBranchesSpecification(clanId);
                var clan = await _clanRepo.GetBySpecification(spec);

                if (clan == null)
                    return NotFound(ApiResponse<ClanReturnDto>.Fail($"Clan with ID {clanId} not found"));

                return Ok(ApiResponse<ClanReturnDto>.Success(clan.ToReturnDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ClanReturnDto>.Fail("An error occurred while retrieving the clan", new List<string> { ex.Message }));
            }
        }

        [HttpPost("clans")]
        public async Task<ActionResult<ApiResponse<ClanReturnDto>>> CreateClan([FromBody] ClanCreateDto clanDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<ClanReturnDto>.Fail("Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

                var clan = new Clan
                {
                    Name = clanDto.Name,
                    PhotoUrl = clanDto.PhotoUrl,
                    Region = clanDto.Region
                };

                await _clanRepo.AddAsync(clan);

                return CreatedAtAction(nameof(GetClanById), new { clanId = clan.Id },
                    ApiResponse<ClanReturnDto>.Success(clan.ToReturnDto(), "Clan created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ClanReturnDto>.Fail("An error occurred while creating the clan", new List<string> { ex.Message }));
            }
        }

        [HttpPut("clans/{clanId}")]
        public async Task<ActionResult<ApiResponse<ClanReturnDto>>> UpdateClan(int clanId, [FromBody] ClanUpdateDto clanDto)
        {
            try
            {
                if (clanId != clanDto.Id)
                    return BadRequest(ApiResponse<ClanReturnDto>.Fail("ID mismatch"));

                var existingClan = await _clanRepo.GetByIdAsync(clanId);
                if (existingClan == null)
                    return NotFound(ApiResponse<ClanReturnDto>.Fail($"Clan with ID {clanId} not found"));

                existingClan.Name = clanDto.Name;
                existingClan.PhotoUrl = clanDto.PhotoUrl;
                existingClan.Region = clanDto.Region;

                await _clanRepo.UpdateAsync(existingClan);

                return Ok(ApiResponse<ClanReturnDto>.Success(existingClan.ToReturnDto(), "Clan updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ClanReturnDto>.Fail("An error occurred while updating the clan", new List<string> { ex.Message }));
            }
        }

        [HttpDelete("clans/{clanId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteClan(int clanId)
        {
            try
            {
                var clan = await _clanRepo.GetByIdAsync(clanId);
                if (clan == null)
                    return NotFound(ApiResponse<bool>.Fail($"Clan with ID {clanId} not found"));

                // Check if clan has branches
                var branchSpec = new BranchWithPersonsSpecification(clanId);
                var branches = await _branchRepo.ListAsync(branchSpec);
                if (branches.Any())
                    return BadRequest(ApiResponse<bool>.Fail("Cannot delete clan with existing branches"));

                await _clanRepo.DeleteAsync(clan);

                return Ok(ApiResponse<bool>.Success(true, "Clan deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Fail("An error occurred while deleting the clan", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region Branch Operations

        [HttpGet("branches")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BranchReturnDto>>>> GetAllBranches()
        {
            try
            {
                var spec = new BranchWithPersonsSpecification();
                var branches = await _branchRepo.ListAsync(spec);
                return Ok(ApiResponse<IEnumerable<BranchReturnDto>>.Success(branches.ToReturnDtos()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<BranchReturnDto>>.Fail("An error occurred while retrieving branches", new List<string> { ex.Message }));
            }
        }

        [HttpGet("branches/{branchId}")]
        public async Task<ActionResult<ApiResponse<BranchReturnDto>>> GetBranchById(int branchId)
        {
            try
            {
                var spec = new BranchWithPersonsSpecification(branchId);
                var branch = await _branchRepo.GetBySpecification(spec);

                if (branch == null)
                    return NotFound(ApiResponse<BranchReturnDto>.Fail($"Branch with ID {branchId} not found"));

                return Ok(ApiResponse<BranchReturnDto>.Success(branch.ToReturnDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BranchReturnDto>.Fail("An error occurred while retrieving the branch", new List<string> { ex.Message }));
            }
        }

        [HttpGet("clans/{clanId}/branches")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BranchReturnDto>>>> GetBranchesByClanId(int clanId)
        {
            try
            {
                var spec = new BranchWithPersonsSpecification(clanId, true);
                var branches = await _branchRepo.ListAsync(spec);

                if (!branches.Any())
                    return NotFound(ApiResponse<IEnumerable<BranchReturnDto>>.Fail($"No branches found for clan ID {clanId}"));

                return Ok(ApiResponse<IEnumerable<BranchReturnDto>>.Success(branches.ToReturnDtos()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<BranchReturnDto>>.Fail("An error occurred while retrieving branches", new List<string> { ex.Message }));
            }
        }

        [HttpPost("branches")]
        public async Task<ActionResult<ApiResponse<BranchReturnDto>>> CreateBranch([FromBody] BranchCreateDto branchDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<BranchReturnDto>.Fail("Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

                // Verify clan exists
                var clan = await _clanRepo.GetByIdAsync(branchDto.ClanId);
                if (clan == null)
                    return NotFound(ApiResponse<BranchReturnDto>.Fail($"Clan with ID {branchDto.ClanId} not found"));

                var branch = new Branch
                {
                    Name = branchDto.Name,
                    PhotoUrl = branchDto.PhotoUrl,
                    Region = branchDto.Region,
                    ClanId = branchDto.ClanId
                };

                await _branchRepo.AddAsync(branch);

                return CreatedAtAction(nameof(GetBranchById), new { branchId = branch.Id },
                    ApiResponse<BranchReturnDto>.Success(branch.ToReturnDto(), "Branch created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BranchReturnDto>.Fail("An error occurred while creating the branch", new List<string> { ex.Message }));
            }
        }

        [HttpPut("branches/{branchId}")]
        public async Task<ActionResult<ApiResponse<BranchReturnDto>>> UpdateBranch(int branchId, [FromBody] BranchUpdateDto branchDto)
        {
            try
            {
                if (branchId != branchDto.Id)
                    return BadRequest(ApiResponse<BranchReturnDto>.Fail("ID mismatch"));

                var existingBranch = await _branchRepo.GetByIdAsync(branchId);
                if (existingBranch == null)
                    return NotFound(ApiResponse<BranchReturnDto>.Fail($"Branch with ID {branchId} not found"));

                // Verify clan exists if changing clan
                if (existingBranch.ClanId != branchDto.ClanId)
                {
                    var clan = await _clanRepo.GetByIdAsync(branchDto.ClanId);
                    if (clan == null)
                        return NotFound(ApiResponse<BranchReturnDto>.Fail($"Clan with ID {branchDto.ClanId} not found"));
                }

                existingBranch.Name = branchDto.Name;
                existingBranch.PhotoUrl = branchDto.PhotoUrl;
                existingBranch.Region = branchDto.Region;
                existingBranch.ClanId = branchDto.ClanId;

                await _branchRepo.UpdateAsync(existingBranch);

                return Ok(ApiResponse<BranchReturnDto>.Success(existingBranch.ToReturnDto(), "Branch updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<BranchReturnDto>.Fail("An error occurred while updating the branch", new List<string> { ex.Message }));
            }
        }

        [HttpDelete("branches/{branchId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteBranch(int branchId)
        {
            try
            {
                var branch = await _branchRepo.GetByIdAsync(branchId);
                if (branch == null)
                    return NotFound(ApiResponse<bool>.Fail($"Branch with ID {branchId} not found"));

                // Check if branch has users
                var usersInBranch = await _userManager.Users.Where(u => u.BranchId == branchId).ToListAsync();
                if (usersInBranch.Any())
                    return BadRequest(ApiResponse<bool>.Fail("Cannot delete branch with existing members"));

                await _branchRepo.DeleteAsync(branch);

                return Ok(ApiResponse<bool>.Success(true, "Branch deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Fail("An error occurred while deleting the branch", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region User Operations
        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserReturnDto>>>> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                return Ok(ApiResponse<IEnumerable<UserReturnDto>>.Success(users.ToReturnDtos()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<UserReturnDto>>.Fail("An error occurred while retrieving users", new List<string> { ex.Message }));
            }
        }
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<ApiResponse<UserReturnDto>>> GetUserById(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound(ApiResponse<UserReturnDto>.Fail($"User with ID {userId} not found"));

                return Ok(ApiResponse<UserReturnDto>.Success(user.ToReturnDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserReturnDto>.Fail("An error occurred while retrieving the user", new List<string> { ex.Message }));
            }
        }

        [HttpGet("branches/{branchId}/users")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserReturnDto>>>> GetUsersByBranchId(int branchId)
        {
            try
            {
                var users = await _userManager.Users.Where(u => u.BranchId == branchId).ToListAsync();
                if (!users.Any())
                    return NotFound(ApiResponse<IEnumerable<UserReturnDto>>.Fail($"No users found for branch ID {branchId}"));

                return Ok(ApiResponse<IEnumerable<UserReturnDto>>.Success(users.ToReturnDtos()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<UserReturnDto>>.Fail("An error occurred while retrieving users", new List<string> { ex.Message }));
            }
        }

        [HttpGet("clans/{clanId}/users")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserReturnDto>>>> GetUsersByClanId(int clanId)
        {
            try
            {
                // First get all branches for the clan
                var branchSpec = new BranchWithPersonsSpecification(clanId, true);
                var branches = await _branchRepo.ListAsync(branchSpec);

                if (!branches.Any())
                    return NotFound(ApiResponse<IEnumerable<UserReturnDto>>.Fail($"No branches found for clan ID {clanId}"));

                // Then get all users from those branches
                var branchIds = branches.Select(b => b.Id).ToList();
                var users = await _userManager.Users.Where(u => branchIds.Contains(u.BranchId ?? 0)).ToListAsync();

                if (!users.Any())
                    return NotFound(ApiResponse<IEnumerable<UserReturnDto>>.Fail($"No users found for clan ID {clanId}"));

                return Ok(ApiResponse<IEnumerable<UserReturnDto>>.Success(users.ToReturnDtos()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<UserReturnDto>>.Fail("An error occurred while retrieving users", new List<string> { ex.Message }));
            }
        }

        [HttpPost("users")]
        public async Task<ActionResult<ApiResponse<UserReturnDto>>> CreateUser([FromBody] UserCreateDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<UserReturnDto>.Fail("Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

                // Verify branch exists if provided
                if (userDto.BranchId.HasValue)
                {
                    var branch = await _branchRepo.GetByIdAsync(userDto.BranchId.Value);
                    if (branch == null)
                        return NotFound(ApiResponse<UserReturnDto>.Fail($"Branch with ID {userDto.BranchId} not found"));
                }

                var user = new AppUser
                {
                    UserName = userDto.Email,
                    Email = userDto.Email,
                    DisplayName = userDto.DisplayName,
                    PhotoUrl = userDto.PhotoUrl,
                    FatherName = userDto.FatherName,
                    MotherName = userDto.MotherName,
                    BirthDate = userDto.BirthDate,
                    PhoneNumber = userDto.PhoneNumber,
                    FacebookAccount = userDto.FacebookAccount,
                    InstagramAccount = userDto.InstagramAccount,
                    FGrandFatherName = userDto.FGrandFatherName,
                    FGrandMotherName = userDto.FGrandMotherName,
                    MGrandFatherName = userDto.MGrandFatherName,
                    MGrandMotherName = userDto.MGrandMotherName,
                    EmailAddress = userDto.EmailAddress,
                    AddressTitle = userDto.AddressTitle,
                    Latitude = userDto.Latitude,
                    Longitude = userDto.Longitude,
                    FCMToken = userDto.FCMToken,
                    BranchId = userDto.BranchId
                };

                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                    return BadRequest(ApiResponse<UserReturnDto>.Fail("Failed to create user", result.Errors.Select(e => e.Description).ToList()));

                return CreatedAtAction(nameof(GetUserById), new { userId = user.Id },
                    ApiResponse<UserReturnDto>.Success(user.ToReturnDto(), "User created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserReturnDto>.Fail("An error occurred while creating the user", new List<string> { ex.Message }));
            }
        }

        [HttpPut("users/{userId}")]
        public async Task<ActionResult<ApiResponse<UserReturnDto>>> UpdateUser(string userId, [FromBody] UserUpdateDto userDto)
        {
            try
            {
                if (userId != userDto.Id)
                    return BadRequest(ApiResponse<UserReturnDto>.Fail("ID mismatch"));

                var existingUser = await _userManager.FindByIdAsync(userId);
                if (existingUser == null)
                    return NotFound(ApiResponse<UserReturnDto>.Fail($"User with ID {userId} not found"));

                // Verify branch exists if changing branch
                if (existingUser.BranchId != userDto.BranchId && userDto.BranchId.HasValue)
                {
                    var branch = await _branchRepo.GetByIdAsync(userDto.BranchId.Value);
                    if (branch == null)
                        return NotFound(ApiResponse<UserReturnDto>.Fail($"Branch with ID {userDto.BranchId} not found"));
                }

                existingUser.DisplayName = userDto.DisplayName;
                existingUser.PhotoUrl = userDto.PhotoUrl;
                existingUser.FatherName = userDto.FatherName;
                existingUser.MotherName = userDto.MotherName;
                existingUser.BirthDate = userDto.BirthDate;
                existingUser.PhoneNumber = userDto.PhoneNumber;
                existingUser.FacebookAccount = userDto.FacebookAccount;
                existingUser.InstagramAccount = userDto.InstagramAccount;
                existingUser.FGrandFatherName = userDto.FGrandFatherName;
                existingUser.FGrandMotherName = userDto.FGrandMotherName;
                existingUser.MGrandFatherName = userDto.MGrandFatherName;
                existingUser.MGrandMotherName = userDto.MGrandMotherName;
                existingUser.EmailAddress = userDto.EmailAddress;
                existingUser.AddressTitle = userDto.AddressTitle;
                existingUser.Latitude = userDto.Latitude;
                existingUser.Longitude = userDto.Longitude;
                existingUser.FCMToken = userDto.FCMToken;
                existingUser.BranchId = userDto.BranchId;

                var result = await _userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                    return BadRequest(ApiResponse<UserReturnDto>.Fail("Failed to update user", result.Errors.Select(e => e.Description).ToList()));

                return Ok(ApiResponse<UserReturnDto>.Success(existingUser.ToReturnDto(), "User updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserReturnDto>.Fail("An error occurred while updating the user", new List<string> { ex.Message }));
            }
        }

        [HttpDelete("users/{userId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound(ApiResponse<bool>.Fail($"User with ID {userId} not found"));

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                    return BadRequest(ApiResponse<bool>.Fail("Failed to delete user", result.Errors.Select(e => e.Description).ToList()));

                return Ok(ApiResponse<bool>.Success(true, "User deleted successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Fail("An error occurred while deleting the user", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region Assignment Operations

        [HttpPost("users/assign-to-branch")]
        public async Task<ActionResult<ApiResponse<UserReturnDto>>> AssignUserToBranch(
           [FromBody] Family.Core.DTOs.BranchAssignmentDto assignmentDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(assignmentDto.UserId);
                if (user == null)
                    return NotFound(ApiResponse<UserReturnDto>.Fail($"User with ID {assignmentDto.UserId} not found"));

                var branch = await _branchRepo.GetByIdAsync(assignmentDto.BranchId);
                if (branch == null)
                    return NotFound(ApiResponse<UserReturnDto>.Fail($"Branch with ID {assignmentDto.BranchId} not found"));

                user.BranchId = assignmentDto.BranchId;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    return BadRequest(ApiResponse<UserReturnDto>.Fail("Failed to assign user to branch",
                        result.Errors.Select(e => e.Description).ToList()));

                return Ok(ApiResponse<UserReturnDto>.Success(user.ToReturnDto(),
                    "User assigned to branch successfully"));
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx &&
                   sqlEx.Message.Contains("FOREIGN KEY constraint"))
            {
                return BadRequest(ApiResponse<UserReturnDto>.Fail(
                    "Invalid branch assignment - branch doesn't exist",
                    new List<string> { sqlEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserReturnDto>.Fail(
                    "An error occurred while assigning user to branch",
                    new List<string> { ex.Message }));
            }
        }

        #endregion
    }
}