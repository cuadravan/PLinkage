namespace PLinkageShared.DTOs
{
    public class SendMessageDto
    {
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
    }

    public class ChatSummaryDto
    {
        public Guid ChatId { get; set; }
        public string ReceiverFullName { get; set; }
        public Guid ReceiverId { get; set; }
        public string MostRecentMessage { get; set; }
        public DateTime MessageDate { get; set; } 
    }

    public class ChatMessageDto
    {
        public Guid MessageId { get; set; } = Guid.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsFromCurrentUser { get; set; } = false;
        public Guid SenderId { get; set; } = Guid.Empty;
        public bool IsRead { get; set; } = false;
        public Guid ChatId { get; set; } = Guid.Empty;
    }
}
