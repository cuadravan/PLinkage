using PLinkageShared.Enums;

namespace PLinkageShared.DTOs
{
    public class ProjectOwnerDto
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public CebuLocation? UserLocation { get; set; } = null;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
        public string UserRole { get; set; } = "ProjectOwner";
        public string UserStatus { get; set; } = string.Empty;
        public DateTime JoinedOn { get; set; } = DateTime.Now;
        public List<Guid> OfferApplicationId { get; set; } = new List<Guid>();
        public List<Guid> OwnedProjectId { get; set; } = new List<Guid>();
        public List<Guid> UserMessagesId { get; set; } = new List<Guid>();
    }

    public class ProjectOwnerCardDto
    {
        public string UserName { get; set; }
        public string UserStatus { get; set; }
        public string ProjectCount { get; set; }

    }
}
