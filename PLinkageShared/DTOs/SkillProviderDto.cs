using PLinkageShared.Enums;

namespace PLinkageShared.DTOs
{
    public class SkillProviderDto
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        //public CebuLocation? UserLocation { get; set; } = null;
        public string UserLocation { get; set; } = string.Empty;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
        public UserRole UserRole { get; set; } = UserRole.SkillProvider;
        public string UserStatus { get; set; } = string.Empty;
        public List<EducationDto> Educations { get; set; } = new List<EducationDto>();
        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
        public List <SkillProviderProfileProjectsDto> ProfileProjects { get; set; } = new List<SkillProviderProfileProjectsDto>();
        public string UserRating { get; set; } = "0.0";
        public DateTime JoinedOn { get; set; } = DateTime.Now;

        
    }

    public class SkillProviderProfileProjectsDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public int TimeFrame { get; set; }
        public decimal Rate { get; set; }
    }

    public class EducationDto
    {
        public string CourseName { get; set; } = string.Empty;
        public string SchoolAttended { get; set; } = string.Empty;
        public DateTime TimeGraduated { get; set; } = DateTime.Now;
    }

    public class SkillDto
    {
        public string SkillName { get; set; } = string.Empty;
        public string SkillDescription { get; set; } = string.Empty;
        public int SkillLevel { get; set; } // 0-5
        public DateTime TimeAcquired { get; set; } = DateTime.Now;
        public string OrganizationInvolved { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
    }

    public class UserProfileUpdateDto
    {
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public CebuLocation? UserLocation { get; set; } = null;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
    }

    public class SkillProviderCardDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserRating { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public List<string> Skills { get; set; }    
    }

    public class RequestResignationDto
    {
        public Guid ProjectId { get; set; }
        public Guid SkillProviderId { get; set; }
        public string Reason { get; set; }
    }
}
