using System.ComponentModel.DataAnnotations;

namespace PLinkageShared.DTOs
{
    public class SendMessageDto
    {
        [Required]
        public Guid ReceiverId { get; set; }

        [Required]
        [MinLength(1)]
        public string Content { get; set; }
    }

    public class ChatSummaryDto
    {
        public Guid ChatId { get; set; }
        public string ReceiverFullName { get; set; }
        public string MostRecentMessage { get; set; }
        public DateTime MessageDate { get; set; } 
    }

    public class ChatMessageDto
    {
        public Guid MessageId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public bool IsFromCurrentUser { get; set; }
        public Guid SenderId { get; set; }
        public bool IsRead { get; set; }
    }
}
