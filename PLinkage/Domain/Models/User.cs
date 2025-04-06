using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PLinkage.Domain.Models
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string UserLocation { get; set; } = string.Empty;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public string UserStatus { get; set; } = string.Empty;
        public List<Guid> OfferApplicationId { get; set; } = new List<Guid>();

        // For Skill Providers
        public List<Education> Educations { get; set; } = new List<Education>();
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public double UserRating { get; set; } = 0.0;

        // For Project Owners
        public List<Guid> EmployedProjectId { get; set; } = new List<Guid>();
        public List<Guid> OwnedProjectId { get; set; } = new List<Guid>();
        public List<Guid> UserMessagesId { get; set; } = new List<Guid>();
    }
}
