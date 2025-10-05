using PLinkageShared.Enums;
using System.Text.Json.Serialization;

namespace PLinkageShared.DTOs
{
    public class SkillProviderDto
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public CebuLocation? UserLocation { get; set; } = null;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
        public UserRole UserRole { get; set; } = UserRole.SkillProvider;
        public string UserStatus { get; set; } = string.Empty;
        public List<Guid> OfferApplicationId { get; set; } = new List<Guid>();
        public List<EducationDto> Educations { get; set; } = new List<EducationDto>();
        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
        public List <Guid> EmployedProjects { get; set; } = new List<Guid>();
        public double UserRating { get; set; } = 0.0;
        public double UserRatingTotal { get; set; } = 0.0;
        public int UserRatingCount { get; set; } = 0;
        public double TempRating { get; set; } = 0.0;
        public DateTime JoinedOn { get; set; } = DateTime.Now;
        public List<Guid> UserMessagesId { get; set; } = new List<Guid>();

        
    }
}
