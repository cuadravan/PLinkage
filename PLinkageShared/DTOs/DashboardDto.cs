using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinkageShared.DTOs
{
    public class SkillProviderDashboardDto
    {
        public int PendingSentApplications { get; set; }
        public int ReceivedOffers { get; set; }
        public int ActiveProjects { get; set; }
    }
    public class ProjectOwnerDashboardDto
    {
        public int PendingSentOffers { get; set; }
        public int ReceivedApplications { get; set; }
        public int ActiveProjects { get; set; }
        public int ReportedResignations { get; set; }
        public int ReportedNegotiations { get; set; }
    }
    public class AdminDashboardDto
    {
        public int OverallActiveProjects { get; set; }
        public int OverallCompleteProjects { get; set; }
        public double EmploymentRatio { get; set; }
    }
}
