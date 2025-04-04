using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkage.Models
{
    internal class Project
    {
        public Guid ProjectId { get; set; } = Guid.NewGuid();
        public Guid ProjectOwnerId { get; set; } = Guid.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDescription { get; set; } = string.Empty;
        public DateTime ProjectStartDate { get; set; } = DateTime.Now;
        public DateTime ProjectEndDate { get; set; } = DateTime.Now;
        public string ProjectStatus { get; set; } = string.Empty;
        public List<string> ProjectSkillsRequired { get; set; } = new List<string>();
        public List<Guid> ProjectMembersId { get; set; } = new List<Guid>();
        public string ProjectPriority { get; set; } = string.Empty;
        public int ProjectResourcesNeeded { get; set; } = 0;
        public DateTime ProjectDateCreated { get; set; } = DateTime.Now;
        public DateTime ProjectDateUpdated { get; set; } = DateTime.Now;
    }
}
