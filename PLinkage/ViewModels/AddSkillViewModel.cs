using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PLinkage.Interfaces;
using PLinkage.Models;

namespace PLinkage.ViewModels
{
    public partial class AddSkillViewModel : ObservableValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly INavigationService _navigationService;

        public AddSkillViewModel(IUnitOfWork unitOfWork, ISessionService sessionService, INavigationService navigationService)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _navigationService = navigationService;
            TimeAcquired = DateTime.Today;
        }

        // Form Fields
        [ObservableProperty]
        [Required(ErrorMessage = "Skill Name is required.")]
        private string skillName;

        [ObservableProperty]
        [Required(ErrorMessage = "Description is required.")]
        private string skillDescription;

        [ObservableProperty]
        [Required(ErrorMessage = "Skill level is required.")]
        [Range(1, 5, ErrorMessage = "Skill level must be between 1 and 5.")]
        private int skillLevel;

        [ObservableProperty]
        [Required(ErrorMessage = "Time acquired is required.")]
        private DateTime timeAcquired;

        [ObservableProperty]
        [Required(ErrorMessage = "Organization is required.")]
        private string organizationInvolved;

        [ObservableProperty]
        [Required(ErrorMessage = "Years of experience is required.")]
        [Range(1, 100, ErrorMessage = "Years of experience must be between 1 and 100.")]
        private int yearsOfExperience;

        [ObservableProperty]
        private string errorMessage;

        // Save Command
        [RelayCommand]
        private async Task Save()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                ErrorMessage = GetErrors()
                    .OfType<ValidationResult>()
                    .FirstOrDefault()?.ErrorMessage ?? "Validation failed.";
                return;
            }

            var userId = _sessionService.GetCurrentUser().UserId;
            var skillProvider = await _unitOfWork.SkillProvider.GetByIdAsync(userId);
            if (skillProvider == null)
            {
                ErrorMessage = "SkillProvider not found.";
                return;
            }

            var newSkill = new Skill
            {
                SkillName = SkillName,
                SkillDescription = SkillDescription,
                SkillLevel = SkillLevel,
                TimeAcquired = TimeAcquired,
                OrganizationInvolved = OrganizationInvolved,
                YearsOfExperience = YearsOfExperience
            };

            skillProvider.Skills.Add(newSkill);

            await _unitOfWork.SkillProvider.UpdateAsync(skillProvider);
            await _unitOfWork.SaveChangesAsync();
            _sessionService.SetCurrentUser(skillProvider);

            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await _navigationService.GoBackAsync();
        }
    }
}
