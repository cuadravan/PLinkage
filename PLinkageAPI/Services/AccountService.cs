using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using PLinkageAPI.Entities;
using PLinkageAPI.Interfaces;
using PLinkageAPI.Repository;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using static System.Net.Mime.MediaTypeNames;

namespace PLinkageAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<SkillProvider> _skillProviderRepository;
        private readonly IRepository<ProjectOwner> _projectOwnerRepository;
        private readonly IRepository<Admin> _adminRepository;

        public AccountService(IRepository<SkillProvider> skillProviderRepository, IRepository<ProjectOwner> projectOwnerRepository, IRepository<Admin> adminRepository)
        {
            _skillProviderRepository = skillProviderRepository;
            _projectOwnerRepository = projectOwnerRepository;
            _adminRepository = adminRepository;
        }

        //public async Task<ApiResponse<string>> HashAllPasswordsAsync()
        //{
        //    try
        //    {
        //        var passwordHasherSP = new PasswordHasher<SkillProvider>();
        //        var passwordHasherPO = new PasswordHasher<ProjectOwner>();
        //        var passwordHasherAdmin = new PasswordHasher<Admin>();

        //        int hashedCount = 0;

        //        // --- SkillProviders ---
        //        var allSkillProviders = await _skillProviderRepository.GetAllAsync();
        //        foreach (var sp in allSkillProviders)
        //        {
        //            // skip if already hashed
        //            if (!string.IsNullOrWhiteSpace(sp.UserPassword) && !sp.UserPassword.Contains("$"))
        //            {
        //                sp.UserPassword = passwordHasherSP.HashPassword(sp, sp.UserPassword);
        //                await _skillProviderRepository.UpdateAsync(sp);
        //                hashedCount++;
        //            }
        //        }

        //        // --- ProjectOwners ---
        //        var allProjectOwners = await _projectOwnerRepository.GetAllAsync();
        //        foreach (var po in allProjectOwners)
        //        {
        //            if (!string.IsNullOrWhiteSpace(po.UserPassword) && !po.UserPassword.Contains("$"))
        //            {
        //                po.UserPassword = passwordHasherPO.HashPassword(po, po.UserPassword);
        //                await _projectOwnerRepository.UpdateAsync(po);
        //                hashedCount++;
        //            }
        //        }

        //        // --- Admins ---
        //        var allAdmins = await _adminRepository.GetAllAsync();
        //        foreach (var admin in allAdmins)
        //        {
        //            if (!string.IsNullOrWhiteSpace(admin.UserPassword) && !admin.UserPassword.Contains("$"))
        //            {
        //                admin.UserPassword = passwordHasherAdmin.HashPassword(admin, admin.UserPassword);
        //                await _adminRepository.UpdateAsync(admin);
        //                hashedCount++;
        //            }
        //        }

        //        return ApiResponse<string>.Ok($"Successfully hashed {hashedCount} passwords.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<string>.Fail($"An error occurred while hashing passwords: {ex.Message}");
        //    }
        //}

        public async Task<ApiResponse<bool>> CheckEmailUniquenessAsync(string email)
        {
            var skillProviderFilter = Builders<SkillProvider>.Filter.Eq(sp => sp.UserEmail, email);
            var skillProvider = (await _skillProviderRepository.FindAsync(skillProviderFilter)).FirstOrDefault();

            if (skillProvider != null)
            {
                return ApiResponse<bool>.Fail($"This email is already in use. Try another email.");
            }

            var projectOwnerFilter = Builders<ProjectOwner>.Filter.Eq(po => po.UserEmail, email);
            var projectOwner = (await _projectOwnerRepository.FindAsync(projectOwnerFilter)).FirstOrDefault();

            if (projectOwner != null)
            {
                return ApiResponse<bool>.Fail($"This email is already in use. Try another email.");
            }

            var adminFilter = Builders<Admin>.Filter.Eq(a => a.UserEmail, email);
            var admin = (await _adminRepository.FindAsync(adminFilter)).FirstOrDefault();

            if (admin != null)
            {
                return ApiResponse<bool>.Fail($"This email is already in use. Try another email.");
            }

            return ApiResponse<bool>.Ok(true, "This email has not yet been used.");
        }
        public async Task<ApiResponse<LoginResultDto>> AuthenticateUserAsync(string email, string password)
        {
            try
            {
                var passwordHasherSP = new PasswordHasher<SkillProvider>();
                var passwordHasherPO = new PasswordHasher<ProjectOwner>();
                var passwordHasherAdmin = new PasswordHasher<Admin>();

                // SkillProvider
                var skillProviderFilter = Builders<SkillProvider>.Filter.Eq(sp => sp.UserEmail, email);
                var skillProvider = (await _skillProviderRepository.FindAsync(skillProviderFilter)).FirstOrDefault();

                if (skillProvider != null)
                {
                    var result = passwordHasherSP.VerifyHashedPassword(skillProvider, skillProvider.UserPassword, password);
                    if (result == PasswordVerificationResult.Failed)
                        return ApiResponse<LoginResultDto>.Fail("Incorrect password for Skill Provider account.");

                    return ApiResponse<LoginResultDto>.Ok(new LoginResultDto
                    {
                        UserId = skillProvider.UserId,
                        UserRole = PLinkageShared.Enums.UserRole.SkillProvider,
                        UserName = skillProvider.UserFirstName + " " + skillProvider.UserLastName,
                        UserLocation = skillProvider.UserLocation,
                        Message = $"Welcome back, {skillProvider.UserFirstName}!"
                    });
                }

                // ProjectOwner
                var projectOwnerFilter = Builders<ProjectOwner>.Filter.Eq(po => po.UserEmail, email);
                var projectOwner = (await _projectOwnerRepository.FindAsync(projectOwnerFilter)).FirstOrDefault();

                if (projectOwner != null)
                {
                    var result = passwordHasherPO.VerifyHashedPassword(projectOwner, projectOwner.UserPassword, password);
                    if (result == PasswordVerificationResult.Failed)
                        return ApiResponse<LoginResultDto>.Fail("Incorrect password for Project Owner account.");

                    return ApiResponse<LoginResultDto>.Ok(new LoginResultDto
                    {
                        UserId = projectOwner.UserId,
                        UserRole = PLinkageShared.Enums.UserRole.ProjectOwner,
                        UserLocation = projectOwner.UserLocation,
                        UserName = projectOwner.UserFirstName + " " + projectOwner.UserLastName,
                        Message = $"Welcome back, {projectOwner.UserFirstName}!"
                    });
                }

                // Admin
                var adminFilter = Builders<Admin>.Filter.Eq(a => a.UserEmail, email);
                var admin = (await _adminRepository.FindAsync(adminFilter)).FirstOrDefault();

                if (admin != null)
                {
                    var result = passwordHasherAdmin.VerifyHashedPassword(admin, admin.UserPassword, password);
                    if (result == PasswordVerificationResult.Failed)
                        return ApiResponse<LoginResultDto>.Fail("Incorrect password for Admin account.");

                    return ApiResponse<LoginResultDto>.Ok(new LoginResultDto
                    {
                        UserId = admin.UserId,
                        UserRole = PLinkageShared.Enums.UserRole.Admin,
                        UserName = admin.UserFirstName + " " + admin.UserLastName,
                        UserLocation = admin.UserLocation,
                        Message = $"Welcome back, {admin.UserFirstName}!"
                    });
                }

                // No user found
                return ApiResponse<LoginResultDto>.Fail("Account with this email does not exist.");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResultDto>.Fail($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var skillProviderFilter = Builders<SkillProvider>.Filter.Eq(sp => sp.UserEmail, registerUserDto.UserEmail);
            var skillProvider = (await _skillProviderRepository.FindAsync(skillProviderFilter)).FirstOrDefault();

            if (skillProvider != null)
            {
                return ApiResponse<string>.Fail($"This email is already in use. Try another email.");
            }

            var projectOwnerFilter = Builders<ProjectOwner>.Filter.Eq(po => po.UserEmail, registerUserDto.UserEmail);
            var projectOwner = (await _projectOwnerRepository.FindAsync(projectOwnerFilter)).FirstOrDefault();

            if (projectOwner != null)
            {
                return ApiResponse<string>.Fail($"This email is already in use. Try another email.");
            }

            var adminFilter = Builders<Admin>.Filter.Eq(a => a.UserEmail, registerUserDto.UserEmail);
            var admin = (await _adminRepository.FindAsync(adminFilter)).FirstOrDefault();

            if (admin != null)
            {
                return ApiResponse<string>.Fail($"This email is already in use. Try another email.");
            }

            if(registerUserDto.UserRole == PLinkageShared.Enums.UserRole.SkillProvider)
            {
                var passwordHasher = new PasswordHasher<SkillProvider>();

                SkillProvider skillProviderNew = new SkillProvider
                {
                    UserFirstName = registerUserDto.UserFirstName,
                    UserLastName = registerUserDto.UserLastName,
                    UserEmail = registerUserDto.UserEmail,
                    UserBirthDate = registerUserDto.UserBirthDate,
                    UserGender = registerUserDto.UserGender,
                    UserPhone = registerUserDto.UserPhone,
                    UserLocation = registerUserDto.UserLocation,
                    JoinedOn = registerUserDto.JoinedOn,
                    UserStatus = "Active"
                };

                skillProviderNew.UserPassword = passwordHasher.HashPassword(skillProviderNew, registerUserDto.UserPassword);
                await _skillProviderRepository.AddAsync(skillProviderNew);
                return ApiResponse<string>.Ok("User successfully registered.");
            }
            else if (registerUserDto.UserRole == PLinkageShared.Enums.UserRole.ProjectOwner)
            {
                var passwordHasher = new PasswordHasher<ProjectOwner>();

                ProjectOwner projectOwnerNew = new ProjectOwner
                {
                    UserFirstName = registerUserDto.UserFirstName,
                    UserLastName = registerUserDto.UserLastName,
                    UserEmail = registerUserDto.UserEmail,
                    UserBirthDate = registerUserDto.UserBirthDate,
                    UserGender = registerUserDto.UserGender,
                    UserPhone = registerUserDto.UserPhone,
                    UserLocation = registerUserDto.UserLocation,
                    JoinedOn = registerUserDto.JoinedOn,
                    UserStatus = "Active"
                };

                projectOwnerNew.UserPassword = passwordHasher.HashPassword(projectOwnerNew, registerUserDto.UserPassword);
                await _projectOwnerRepository.AddAsync(projectOwnerNew);
                return ApiResponse<string>.Ok("User successfully registered.");
            }
            else
            {
                return ApiResponse<string>.Fail($"Invalid user role to register.");
            }
        }

    }
}
