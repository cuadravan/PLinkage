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
        public string ProjectName { get; set; } = string.Empty; // Name of the project
        public Guid ConcernedId { get; set; }
        public string FormattedConcernedName { get; set; } = string.Empty; // Sent to: Van Cuadra or Received by: Van Cuadra
        public string OfferApplicationType { get; set; } = string.Empty; // Offer or Application
        public string OfferApplicationStatus { get; set; } = string.Empty; // Pending, Accepted, Declined
        public string FormattedRate { get; set; } = string.Empty; // The offered rate
        public string FormattedTimeFrame { get; set; } = string.Empty; // The offered time frame
        public bool AwaitingResponse { get; set; } = false; // Whether there should be Accept and Decline buttons
        public bool IsNegotiating { get; set; } = false; // Negotiating indicator
        public bool IsNegotiable { get; set; } = false; // Whether there should be a Negotiate button

        // Purpose of the properties below are for processing only
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ProjectId { get; set; }
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



