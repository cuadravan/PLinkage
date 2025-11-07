using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PLinkageShared.DTOs;
using PLinkageShared.Enums;
using PLinkageShared.ApiResponse;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            this.ProjectStatus = projectUpdateDto.ProjectStatus;
            this.ProjectDateUpdated = projectUpdateDto.ProjectDateUpdated;
            if (projectUpdateDto.ProjectMembersChanged)
            {
                this.ProjectMembers.Clear();
                foreach (var member in projectUpdateDto.ProjectMembers)
                {
                    this.ProjectMembers.Add(new ProjectMemberDetail
                    {
                        MemberId = member.MemberId,
                        UserFirstName = member.UserFirstName,
                        UserLastName = member.UserLastName,
                        Email = member.Email,
                        Rate = member.Rate,
                        TimeFrame = member.TimeFrame,
                        IsResigning = member.IsResigning,
                        ResignationReason = member.ResignationReason
                    });
                }
            }
            this.ProjectResourcesAvailable = this.ProjectResourcesNeeded - this.ProjectMembers.Count;
        }

        public void EmployMember(SkillProvider skillProvider, int timeFrame, decimal rate)
        {
            this.ProjectMembers.Add(new ProjectMemberDetail
            {
                MemberId = skillProvider.UserId,
                UserFirstName = skillProvider.UserFirstName,
                UserLastName = skillProvider.UserLastName,
                Email = skillProvider.UserEmail,
                Rate = rate,
                TimeFrame = timeFrame,
                IsResigning = false,
                ResignationReason = string.Empty
            });
            this.ProjectResourcesAvailable -= 1;
        }

        public bool RequestResignationByMember(Guid skillProviderId, string resignationReason)
        {
            if(this.ProjectStatus != PLinkageShared.Enums.ProjectStatus.Active)
            {
                return false;
            }
            var member = this.ProjectMembers.FirstOrDefault(pm => pm.MemberId == skillProviderId);
            if (member == null)
                return false;
            member.IsResigning = true;
            member.ResignationReason = resignationReason;
            return true;
        }

        public bool ProcessResignationOfMember(Guid skillproviderId, bool approveResignation)
        {
            var member = this.ProjectMembers.FirstOrDefault(pm => pm.MemberId == skillproviderId);

            if (member == null)
            {
                return false;
            }

            if (approveResignation)
            {
                this.ProjectMembers.Remove(member);
                this.ProjectResourcesAvailable += 1;
                return true;
            }
            else
            {
                member.IsResigning = false;
                member.ResignationReason = string.Empty;
                return true;
            }

            
            
        }
    }

    public class ProjectMemberDetail
    {
        [BsonRepresentation(BsonType.String)]
        public Guid MemberId { get; set; }
        public string UserFirstName { get; set; } // From SkillProvider
        public string UserLastName { get; set; } // From SkillProvider
        public string Email { get; set; } // From SkillProvider
        public decimal Rate { get; set; } = 0; // e.g. 1000 per hour
        public int TimeFrame { get; set; } = 0; // Hours
        public bool IsResigning { get; set; } = false;
        public string? ResignationReason { get; set; } = string.Empty; // Reason for resignation
    }
}