using PLinkageShared.Enums;

namespace PLinkageShared.DTOs
{
    public class OfferApplicationDto
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

    public class OfferApplicationCreationDto
    {
        public UserRole UserRoleOfCreator { get; set; }
        public string OfferApplicationType { get; set; } = string.Empty; // e.g. "Application", "Offer"
        public Guid SenderId { get; set; } = Guid.Empty;
        public Guid ReceiverId { get; set; } = Guid.Empty;
        public Guid ProjectId { get; set; } = Guid.Empty;
        public decimal OfferApplicationRate { get; set; } = 0; // e.g. 1000
        public int OfferApplicationTimeFrame { get; set; } = 0; // hours
    }

    public class OfferApplicationPageDto
    {
        public List<OfferApplicationDisplayDto> ReceivedPending { get; set; }
        public List<OfferApplicationDisplayDto> SentPending { get; set; }
        public List<OfferApplicationDisplayDto> ReceivedHistory { get; set; }
        public List<OfferApplicationDisplayDto> SentHistory { get; set; }
    }

    public class OfferApplicationDisplayDto
    {
        public Guid OfferApplicationId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string OfferApplicationType { get; set; } = string.Empty;
        public string OfferApplicationStatus { get; set; } = string.Empty;
        public string FormattedRate { get; set; } = string.Empty;
        public string FormattedTimeFrame { get; set; } = string.Empty;
    }

    public class OfferApplicationProcessDto
    {
        public Guid OfferApplicationId { get; set; }
        public string Process { get; set; } // Approve, Reject, Negotiate
        public string Type { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ProjectId { get; set; }
        public decimal NegotiatedRate { get; set; } = 0;
        public int NegotiatedTimeFrame { get; set; } = 0;
    }
}



