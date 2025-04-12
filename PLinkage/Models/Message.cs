namespace PLinkage.Models
{
    public class Message
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public Guid SenderId { get; set; } = Guid.Empty;
        public Guid ReceiverId { get; set; } = Guid.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public DateTime MessageDate { get; set; } = DateTime.Now;
    }
}
