using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PLinkage.Entities
{
    public class Chat
    {
        [BsonRepresentation(BsonType.String)]
        public Guid ChatId { get; set; } = Guid.NewGuid();
        [BsonRepresentation(BsonType.String)]
        public List<Guid> MessengerId { get; set; } = new List<Guid>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }

    public class Message
    {
        [BsonRepresentation(BsonType.String)]
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public int MessageOrder { get; set; } = 0;
        [BsonRepresentation(BsonType.String)]
        public Guid SenderId { get; set; } = Guid.Empty;
        [BsonRepresentation(BsonType.String)]
        public Guid ReceiverId { get; set; } = Guid.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public DateTime MessageDate { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
    }
}
