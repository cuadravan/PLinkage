namespace PLinkage.Entities
{
    public class OfferApplication
    {
        public Guid OfferApplicationId { get; set; } = Guid.NewGuid();
        public string OfferApplicationType { get; set; } = string.Empty; // e.g. "Application", "Offer"
        public Guid SenderId { get; set; } = Guid.Empty;
        public Guid ReceiverId { get; set; } = Guid.Empty;
        public Guid ProjectId { get; set; } = Guid.Empty;
        public string OfferApplicationStatus { get; set; } = string.Empty; // "Accepted", "Rejected"
        public decimal OfferApplicationRate { get; set; } = 0; // e.g. 1000
        public int OfferApplicationTimeFrame { get; set; } = 0; // hours
    }
}