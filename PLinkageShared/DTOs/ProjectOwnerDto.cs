using PLinkageShared.Enums;

namespace PLinkageShared.DTOs
{
    public class ProjectOwnerDto
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string UserLocation { get; set; } = null;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
        public UserRole UserRole { get; set; }
        public string UserStatus { get; set; } = string.Empty;
        public DateTime JoinedOn { get; set; } = DateTime.Now;
        public List<ProjectOwnerProfileProjectDto> ProfileProjects { get; set; } = new List<ProjectOwnerProfileProjectDto>();
    }

    public class ProjectOwnerProfileProjectDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ProjectStatus? ProjectStatus { get; set; }
        public DateTime ProjectStartDate { get; set; } = DateTime.Now;
        public DateTime ProjectEndDate { get; set; } = DateTime.Now;
    }

    public class ProjectOwnerCardDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserStatus { get; set; }
        public string ProjectCount { get; set; }

    }
}
