using Family.Core.DTOs;
using Family.Core.Entities;
using Family.Core.Identity;
using Family.Service.DTOS.Family.Service.DTOS;
using System.Linq;

namespace Family.Api.Helpers
{
    public static class MappingExtensions
    {
        public static UserReturnDto ToReturnDto(this AppUser user)
        {
            if (user == null)
                return null;

            return new UserReturnDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Created = user.Created,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                BranchId = user.BranchId,
                PhotoUrl = user.PhotoUrl,
                FatherName = user.FatherName,
                MotherName = user.MotherName,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                FacebookAccount = user.FacebookAccount,
                InstagramAccount = user.InstagramAccount,
                FGrandFatherName = user.FGrandFatherName,
                FGrandMotherName = user.FGrandMotherName,
                MGrandFatherName = user.MGrandFatherName,
                MGrandMotherName = user.MGrandMotherName,
                EmailAddress = user.EmailAddress,
                AddressTitle = user.AddressTitle,
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                FCMToken = user.FCMToken
            };
        }

        public static IEnumerable<UserReturnDto> ToReturnDtos(this IEnumerable<AppUser> users)
        {
            return users.Select(user => user.ToReturnDto());
        }
        #region AppUser Mappings
        public static AppUserSimpleDto ToSimpleDto(this AppUser user)
        {
            if (user == null) return null;

            return new AppUserSimpleDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
                // Add other properties you need
            };
        }
        #endregion

        #region Clan Mappings
        public static ClanReturnDto ToReturnDto(this Clan clan)
        {
            if (clan == null) return null;

            return new ClanReturnDto
            {
                Id = clan.Id,
                Name = clan.Name,
                PhotoUrl = clan.PhotoUrl,
                Region = clan.Region,
                Branches = clan.Branches?.Select(b => b.ToSimpleDto()) ?? Enumerable.Empty<BranchSimpleDto>(),
                Members = clan.Branches?
                    .SelectMany(b => b.Persons?.Select(u => new PersonSimpleDto
                    {
                        Id = u.Id,
                        Name = u.UserName, // or $"{u.FirstName} {u.LastName}"
                        PhotoUrl = u.PhotoUrl
                    }) ?? Enumerable.Empty<PersonSimpleDto>())
                    ?? Enumerable.Empty<PersonSimpleDto>()
            };
        }
        public static IEnumerable<ClanReturnDto> ToReturnDtos(this IEnumerable<Clan> clans)
        {
            return clans?.Select(c => c.ToReturnDto()) ?? Enumerable.Empty<ClanReturnDto>();
        }

        public static ClanSimpleDto ToSimpleDto(this Clan clan)
        {
            if (clan == null) return null;

            return new ClanSimpleDto
            {
                Id = clan.Id,
                Name = clan.Name
            };
        }
        #endregion

        #region Branch Mappings
        public static BranchReturnDto ToReturnDto(this Branch branch)
        {
            if (branch == null) return null;

            return new BranchReturnDto
            {
                Id = branch.Id,
                Name = branch.Name,
                PhotoUrl = branch.PhotoUrl,
                Region = branch.Region,
                Clan = branch.Clan?.ToSimpleDto(),
                Members = branch.Persons?.Select(u => u.ToPersonSimpleDto()) ?? Enumerable.Empty<PersonSimpleDto>()
            };
        }
        public static IEnumerable<BranchReturnDto> ToReturnDtos(this IEnumerable<Branch> branches)
        {
            return branches?.Select(b => b.ToReturnDto()) ?? Enumerable.Empty<BranchReturnDto>();
        }

        public static BranchSimpleDto ToSimpleDto(this Branch branch)
        {
            if (branch == null) return null;

            return new BranchSimpleDto
            {
                Id = branch.Id,
                Name = branch.Name
            };
        }
        #endregion

        #region Person Mappings
        public static PersonReturnDto ToReturnDto(this Person person)
        {
            if (person == null) return null;

            return new PersonReturnDto
            {
                Id = person.Id,
                Name = person.Name,
                PhotoUrl = person.PhotoUrl,
                FatherName = person.FatherName,
                MotherName = person.MotherName,
                BirthDate = person.BirthDate,
                PhoneNumber = person.PhoneNumber,
                FacebookAccount = person.FacebookAccount,
                InstagramAccount = person.InstagramAccount,
                FGrandFatherName = person.FGrandFatherName,
                FGrandMotherName = person.FGrandMotherName,
                MGrandFatherName = person.MGrandFatherName,
                MGrandMotherName = person.MGrandMotherName,
                EmailAddress = person.EmailAddress,
                AddressTitle = person.AddressTitle,
                Latitude = person.Latitude,
                Longitude = person.Longitude,
                Clan = person.Clan?.ToSimpleDto(),
                Branch = person.Branch?.ToSimpleDto()
            };
        }

        public static IEnumerable<PersonReturnDto> ToReturnDtos(this IEnumerable<Person> persons)
        {
            return persons?.Select(p => p.ToReturnDto()) ?? Enumerable.Empty<PersonReturnDto>();
        }
        public static PersonSimpleDto ToPersonSimpleDto(this AppUser user)
        {
            if (user == null) return null;

            return new PersonSimpleDto
            {
                Id = user.Id,
                Name = user.UserName, // or $"{user.FirstName} {user.LastName}"
                PhotoUrl = user.PhotoUrl
            };
        }
        #endregion

        #region DTO to Entity Mappings (for create/update operations)
        public static Clan ToEntity(this ClanCreateDto clanDto)
        {
            if (clanDto == null) return null;

            return new Clan
            {
                Name = clanDto.Name,
                PhotoUrl = clanDto.PhotoUrl,
                Region = clanDto.Region
            };
        }

        public static Clan ToEntity(this ClanUpdateDto clanDto, Clan existingClan)
        {
            if (clanDto == null) return null;

            if (existingClan == null)
            {
                return new Clan
                {
                    Name = clanDto.Name,
                    PhotoUrl = clanDto.PhotoUrl,
                    Region = clanDto.Region
                };
            }

            existingClan.Name = clanDto.Name;
            existingClan.PhotoUrl = clanDto.PhotoUrl;
            existingClan.Region = clanDto.Region;

            return existingClan;
        }

        public static Branch ToEntity(this BranchCreateDto branchDto)
        {
            if (branchDto == null) return null;

            return new Branch
            {
                Name = branchDto.Name,
                PhotoUrl = branchDto.PhotoUrl,
                Region = branchDto.Region,
                ClanId = branchDto.ClanId
            };
        }

        public static Branch ToEntity(this BranchUpdateDto branchDto, Branch existingBranch)
        {
            if (branchDto == null) return null;

            if (existingBranch == null)
            {
                return new Branch
                {
                    Name = branchDto.Name,
                    PhotoUrl = branchDto.PhotoUrl,
                    Region = branchDto.Region,
                    ClanId = branchDto.ClanId
                };
            }

            existingBranch.Name = branchDto.Name;
            existingBranch.PhotoUrl = branchDto.PhotoUrl;
            existingBranch.Region = branchDto.Region;
            existingBranch.ClanId = branchDto.ClanId;

            return existingBranch;
        }

        public static Person ToEntity(this PersonCreateDto personDto)
        {
            if (personDto == null) return null;

            return new Person
            {
                Name = personDto.Name,
                PhotoUrl = personDto.PhotoUrl,
                FatherName = personDto.FatherName,
                MotherName = personDto.MotherName,
                BirthDate = personDto.BirthDate,
                PhoneNumber = personDto.PhoneNumber,
                FacebookAccount = personDto.FacebookAccount,
                InstagramAccount = personDto.InstagramAccount,
                FGrandFatherName = personDto.FGrandFatherName,
                FGrandMotherName = personDto.FGrandMotherName,
                MGrandFatherName = personDto.MGrandFatherName,
                MGrandMotherName = personDto.MGrandMotherName,
                EmailAddress = personDto.EmailAddress,
                AddressTitle = personDto.AddressTitle,
                Latitude = personDto.Latitude,
                Longitude = personDto.Longitude,
                FCMToken = personDto.FCMToken,
                ClanId = personDto.ClanId,
                BranchId = personDto.BranchId
            };
        }
        public static Person ToEntity(this PersonUpdateDto personDto, Person existingPerson)
        {
            if (personDto == null) return null;

            if (existingPerson == null)
            {
                return new Person
                {
                    Name = personDto.Name,
                    PhotoUrl = personDto.PhotoUrl,
                    FatherName = personDto.FatherName,
                    MotherName = personDto.MotherName,
                    BirthDate = personDto.BirthDate,
                    PhoneNumber = personDto.PhoneNumber,
                    FacebookAccount = personDto.FacebookAccount,
                    InstagramAccount = personDto.InstagramAccount,
                    FGrandFatherName = personDto.FGrandFatherName,
                    FGrandMotherName = personDto.FGrandMotherName,
                    MGrandFatherName = personDto.MGrandFatherName,
                    MGrandMotherName = personDto.MGrandMotherName,
                    EmailAddress = personDto.EmailAddress,
                    AddressTitle = personDto.AddressTitle,
                    Latitude = personDto.Latitude,
                    Longitude = personDto.Longitude,
                    FCMToken = personDto.FCMToken,
                    ClanId = personDto.ClanId,
                    BranchId = personDto.BranchId
                };
            }

            // Update existing entity
            existingPerson.Name = personDto.Name;
            existingPerson.PhotoUrl = personDto.PhotoUrl;
            existingPerson.FatherName = personDto.FatherName;
            existingPerson.MotherName = personDto.MotherName;
            existingPerson.BirthDate = personDto.BirthDate;
            existingPerson.PhoneNumber = personDto.PhoneNumber;
            existingPerson.FacebookAccount = personDto.FacebookAccount;
            existingPerson.InstagramAccount = personDto.InstagramAccount;
            existingPerson.FGrandFatherName = personDto.FGrandFatherName;
            existingPerson.FGrandMotherName = personDto.FGrandMotherName;
            existingPerson.MGrandFatherName = personDto.MGrandFatherName;
            existingPerson.MGrandMotherName = personDto.MGrandMotherName;
            existingPerson.EmailAddress = personDto.EmailAddress;
            existingPerson.AddressTitle = personDto.AddressTitle;
            existingPerson.Latitude = personDto.Latitude;
            existingPerson.Longitude = personDto.Longitude;
            existingPerson.FCMToken = personDto.FCMToken;
            existingPerson.ClanId = personDto.ClanId;
            existingPerson.BranchId = personDto.BranchId;

            return existingPerson;
        }

        #endregion

    }
}