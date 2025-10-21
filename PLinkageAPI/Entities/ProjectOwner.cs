using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PLinkageAPI.Interfaces;
using PLinkageShared.Enums;
using PLinkageShared.DTOs;

namespace PLinkageAPI.Entities
{
    public class ProjectOwner: IUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public CebuLocation? UserLocation { get; set; } = null;
        public DateTime UserBirthDate { get; set; } = DateTime.Now;
        public string UserGender { get; set; } = string.Empty;
        public UserRole UserRole { get; set; } = UserRole.ProjectOwner;
        public string UserStatus { get; set; } = string.Empty;
        public DateTime JoinedOn { get; set; } = DateTime.Now;
        [BsonRepresentation(BsonType.String)]
        public List<Guid> OfferApplicationId { get; set; } = new List<Guid>();
        [BsonRepresentation(BsonType.String)]
        public List<Guid> OwnedProjectId { get; set; } = new List<Guid>();
        [BsonRepresentation(BsonType.String)]
        public List<Guid> UserMessagesId { get; set; } = new List<Guid>();

        public void UpdateProfile(UserProfileUpdateDto skillProviderUpdateDto)
        {
            this.UserFirstName = skillProviderUpdateDto.UserFirstName;
            this.UserLastName = skillProviderUpdateDto.UserLastName;
            this.UserPhone = skillProviderUpdateDto.UserPhone;
            this.UserLocation = skillProviderUpdateDto.UserLocation;
            this.UserBirthDate = skillProviderUpdateDto.UserBirthDate;
            this.UserLocation = skillProviderUpdateDto.UserLocation;
        }

        public void AddProject(Guid projectId)
        {
            if(!this.OwnedProjectId.Contains(projectId))
                OwnedProjectId.Add(projectId);
        }

        public void AddOfferApplication(Guid guid)
        {
            if(!this.OfferApplicationId.Contains(guid))
                OfferApplicationId.Add(guid);
        }

        public bool AddChat(Guid chatId)
        {
            if (this.UserMessagesId.Contains(chatId))
            {
                return false; // Not added
            }
            this.UserMessagesId.Add(chatId);
            return true; // Was added
        }
    }
}
