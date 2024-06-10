namespace MessageExchanger.Abstractions.Models
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string? Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
