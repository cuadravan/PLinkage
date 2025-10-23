using PLinkageShared.Enums;

namespace PLinkageShared.DTOs
{
    public class ProjectDto
    {
        public Guid ProjectId { get; set; } = Guid.NewGuid();
        public Guid ProjectOwnerId { get; set; } = Guid.Empty;
        public string ProjectOwnerName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectLocation { get; set; } = string.Empty;
        public string ProjectDuration { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public DateTime ProjectStartDate { get; set; } = DateTime.Now;
        public DateTime ProjectEndDate { get; set; } = DateTime.Now;
        public string ProjectStatus { get; set; } = string.Empty;
        public List<string> ProjectSkillsRequired { get; set; } = new List<string>();
        public List<ProjectMemberDetailDto> ProjectMembers { get; set; } = new List<ProjectMemberDetailDto>();
        public string ProjectPriority { get; set; } = string.Empty;
        public int ProjectResourcesNeeded { get; set; } = 0;
        public int ProjectResourcesAvailable { get; set; } = 0;
        public DateTime ProjectDateCreated { get; set; } = DateTime.Now;
        public DateTime ProjectDateUpdated { get; set; } = DateTime.Now;
    }

    public class ProjectCreationDto
    {
        public Guid ProjectOwnerId { get; set; } = Guid.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public CebuLocation? ProjectLocation { get; set; } = null;
        public string ProjectDescription { get; set; } = string.Empty;
        public DateTime ProjectStartDate { get; set; } = DateTime.Now;
        public DateTime ProjectEndDate { get; set; } = DateTime.Now;
        public ProjectStatus? ProjectStatus { get; set; } = null;
        public List<string> ProjectSkillsRequired { get; set; } = new List<string>();
        public string ProjectPriority { get; set; } = string.Empty;
        public int ProjectResourcesNeeded { get; set; } = 0;
        public int ProjectResourcesAvailable { get; set; } = 0;
        public DateTime ProjectDateCreated { get; set; } = DateTime.Now;
        public DateTime ProjectDateUpdated { get; set; } = DateTime.Now;
    }

    public class ProjectMemberDetailDto
    {
        public Guid MemberId { get; set; }
        public string UserName { get; set; }
        public string UserFirstName { get; set; } // From SkillProvider
        public string UserLastName { get; set; } // From SkillProvider
        public string Email { get; set; } // From SkillProvider
        public decimal Rate { get; set; } = 0; // e.g. 1000 per hour
        public int TimeFrame { get; set; } = 0; // Hours
        public bool IsResigning { get; set; } = false;
        public string? ResignationReason { get; set; } = string.Empty; // Reason for resignation
    }

    public class ProjectUpdateDto
    {
        public Guid ProjectId { get; set; } = Guid.NewGuid();
        public string ProjectDescription { get; set; } = string.Empty;
        public DateTime ProjectStartDate { get; set; } = DateTime.Now;
        public string ProjectPriority { get; set; } = string.Empty;
        public List<string> ProjectSkillsRequired { get; set; } = new List<string>();
        public int ProjectResourcesNeeded { get; set; } = 0;
        public ProjectStatus? ProjectStatus { get; set; } = null;
        public DateTime ProjectDateUpdated { get; set; } = DateTime.Now;
    }
    public class ProcessResignationDto
    {
        public Guid ProjectId { get; set; }
        public List<ProcessResignationIndividualDto>? processResignationIndividualDtos { get; set; }
    }

    public class ProcessResignationIndividualDto
    {
        public Guid SkillProviderId { get; set; }
        public bool ApproveResignation { get; set; }
    }

    public class RateSkillProviderDto
    {
        public List<RateSkillProviderIndividualDto>? rateSkillProviderIndividualDtos { get; set; }
    }

    public class RateSkillProviderIndividualDto
    {
        public Guid SkillProviderId { get; set; }
        public double SkillProviderRating { get; set; }
    }

    public class ProjectCardDto
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Slots { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public List<string> Skills { get; set; }
    }
}


