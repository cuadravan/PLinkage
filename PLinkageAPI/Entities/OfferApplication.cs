using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PLinkageAPI.Entities
{
    public class OfferApplication
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid OfferApplicationId { get; set; } = Guid.NewGuid();
        public string OfferApplicationType { get; set; } = string.Empty; // e.g. "Application", "Offer"

        [BsonRepresentation(BsonType.String)]
        public Guid SenderId { get; set; } = Guid.Empty;

        [BsonRepresentation(BsonType.String)]
        public Guid ReceiverId { get; set; } = Guid.Empty;

        [BsonRepresentation(BsonType.String)]
        public Guid ProjectId { get; set; } = Guid.Empty;
        public string OfferApplicationStatus { get; set; } = string.Empty; // "Accepted", "Rejected"
        public decimal OfferApplicationRate { get; set; } = 0; // e.g. 1000
        public int OfferApplicationTimeFrame { get; set; } = 0; // hours
    }
}