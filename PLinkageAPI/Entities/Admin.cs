using PLinkageShared.Enums;
using PLinkageAPI.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PLinkageAPI.Entities
{
    public class Admin : IUser
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
        public UserRole UserRole { get; set; } = UserRole.Admin;
        public string UserStatus { get; set; } = string.Empty;
        public DateTime JoinedOn { get; set; } = DateTime.Now;
        [BsonRepresentation(BsonType.String)]
        public List<Guid> UserMessagesId { get; set; } = new List<Guid>();

        //Useless stuff on Admin that can never be used
        [BsonRepresentation(BsonType.String)]
        public List<Guid> OfferApplicationId { get; set; } = new List<Guid>();
        public void AddOfferApplication(Guid guid)
        {

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
