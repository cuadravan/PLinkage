using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;

namespace PLinkageAPI.Entities
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid ProjectId { get; set; } = Guid.NewGuid();
        [BsonRepresentation(BsonType.String)]
        public Guid ProjectOwnerId { get; set; } = Guid.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public CebuLocation? ProjectLocation { get; set; } = null;
        public string ProjectDescription { get; set; } = string.Empty;
        public DateTime ProjectStartDate { get; set; } = DateTime.Now;
        public DateTime ProjectEndDate { get; set; } = DateTime.Now;
        public ProjectStatus? ProjectStatus { get; set; } = null;
        public List<string> ProjectSkillsRequired { get; set; } = new List<string>();
        public List<ProjectMemberDetail> ProjectMembers { get; set; } = new List<ProjectMemberDetail>();
        public string ProjectPriority { get; set; } = string.Empty;
        public int ProjectResourcesNeeded { get; set; } = 0;
        public int ProjectResourcesAvailable { get; set; } = 0;
        public DateTime ProjectDateCreated { get; set; } = DateTime.Now;
        public DateTime ProjectDateUpdated { get; set; } = DateTime.Now;

        public void UpdateProject(ProjectUpdateDto projectUpdateDto)
        {
            this.ProjectDescription = projectUpdateDto.ProjectDescription;
            this.ProjectPriority = projectUpdateDto.ProjectPriority;
            this.ProjectStartDate = projectUpdateDto.ProjectStartDate;
            this.ProjectSkillsRequired = projectUpdateDto.ProjectSkillsRequired;
            this.ProjectResourcesNeeded = projectUpdateDto.ProjectResourcesNeeded;
            this.ProjectResourcesAvailable = this.ProjectResourcesNeeded - this.ProjectMembers.Count;
            this.ProjectStatus = projectUpdateDto.ProjectStatus;
            this.ProjectDateUpdated = projectUpdateDto.ProjectDateUpdated;
        }
    }
}