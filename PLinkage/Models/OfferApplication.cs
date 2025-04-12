namespace PLinkage.Models
{
    public class OfferApplication
    {
        public Guid OfferApplicationId { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; } = Guid.Empty;
        public Guid SenderId { get; set; } = Guid.Empty;
        public Guid ReceiverId { get; set; } = Guid.Empty;
        public string OfferApplicationStatus { get; set; } = string.Empty;
        public string OfferApplicationRate { get; set; } = string.Empty; // e.g. 1000 pesos per day
        public string OfferApplicationTimeFrame { get; set; } = string.Empty; // 10 days
    }
}
