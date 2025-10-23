using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using PLinkageAPI.Entities;
using PLinkageAPI.Interfaces;
using PLinkageShared.ApiResponse;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

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
                    if (skillProvider.UserStatus == "Deactivated")
                        return ApiResponse<LoginResultDto>.Fail("Your account has been deactivated.");

                    var result = passwordHasherSP.VerifyHashedPassword(skillProvider, skillProvider.UserPassword, password);
                    if (result == PasswordVerificationResult.Failed)
                        return ApiResponse<LoginResultDto>.Fail("Incorrect password for account.");

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
                    if (projectOwner.UserStatus == "Deactivated")
                        return ApiResponse<LoginResultDto>.Fail("Your account has been deactivated.");

                    var result = passwordHasherPO.VerifyHashedPassword(projectOwner, projectOwner.UserPassword, password);
                    if (result == PasswordVerificationResult.Failed)
                        return ApiResponse<LoginResultDto>.Fail("Incorrect password for account.");

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
                        return ApiResponse<LoginResultDto>.Fail("Incorrect password for account.");

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

        public async Task<UserRole?> DetermineUserRoleAsync(Guid userId)
        {
            var skillProvider = await _skillProviderRepository.GetByIdAsync(userId);

            if (skillProvider != null)
            {
                return UserRole.SkillProvider;
            }

            var projectOwner = await _projectOwnerRepository.GetByIdAsync(userId);

            if (projectOwner != null)
            {
                return UserRole.ProjectOwner;
            }

            var admin = await _adminRepository.GetByIdAsync(userId);

            if (admin != null)
            {
                return UserRole.Admin;
            }

            return null;
        }

        public async Task<ApiResponse<string>> ActivateDeactivateUserAsync(Guid userId)
        {
            var skillProvider = await _skillProviderRepository.GetByIdAsync(userId);

            if (skillProvider != null)
            {
                skillProvider.UserStatus = (skillProvider.UserStatus == "Active") ? "Deactivated" : "Active";
                await _skillProviderRepository.UpdateAsync(skillProvider);
                return ApiResponse<string>.Ok($"Skill provider account with ID {skillProvider.UserId} is now {skillProvider.UserStatus}");
            }

            var projectOwner = await _projectOwnerRepository.GetByIdAsync(userId);

            if (projectOwner != null)
            {
                projectOwner.UserStatus = (projectOwner.UserStatus == "Active") ? "Deactivated" : "Active";
                await _projectOwnerRepository.UpdateAsync(projectOwner);
                return ApiResponse<string>.Ok($"Project Owner account with ID {projectOwner.UserId} is now {projectOwner.UserStatus}");
            }

            return ApiResponse<string>.Fail("No account found with that ID.");
        }

        public async Task<ApiResponse<Guid>> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var newId = Guid.NewGuid();
            
            var skillProviderFilter = Builders<SkillProvider>.Filter.Eq(sp => sp.UserEmail, registerUserDto.UserEmail);
            var skillProvider = (await _skillProviderRepository.FindAsync(skillProviderFilter)).FirstOrDefault();

            if (skillProvider != null)
            {
                return ApiResponse<Guid>.Fail($"This email is already in use. Try another email.");
            }

            var projectOwnerFilter = Builders<ProjectOwner>.Filter.Eq(po => po.UserEmail, registerUserDto.UserEmail);
            var projectOwner = (await _projectOwnerRepository.FindAsync(projectOwnerFilter)).FirstOrDefault();

            if (projectOwner != null)
            {
                return ApiResponse<Guid>.Fail($"This email is already in use. Try another email.");
            }

            var adminFilter = Builders<Admin>.Filter.Eq(a => a.UserEmail, registerUserDto.UserEmail);
            var admin = (await _adminRepository.FindAsync(adminFilter)).FirstOrDefault();

            if (admin != null)
            {
                return ApiResponse<Guid>.Fail($"This email is already in use. Try another email.");
            }

            if(registerUserDto.UserRole == PLinkageShared.Enums.UserRole.SkillProvider)
            {
                var passwordHasher = new PasswordHasher<SkillProvider>();

                SkillProvider skillProviderNew = new SkillProvider
                {
                    UserId = newId,
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
                return ApiResponse<Guid>.Ok(newId);
            }
            else if (registerUserDto.UserRole == PLinkageShared.Enums.UserRole.ProjectOwner)
            {
                var passwordHasher = new PasswordHasher<ProjectOwner>();

                ProjectOwner projectOwnerNew = new ProjectOwner
                {
                    UserId = newId,
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
                return ApiResponse<Guid>.Ok(newId);
            }
            else
            {
                return ApiResponse<Guid>.Fail($"Invalid user role to register.");
            }
        }
         
    }
}
