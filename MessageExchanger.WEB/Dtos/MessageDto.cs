namespace MessageExchanger.WEB.Dtos
{
    public class MessageDto
    {
        public int MessageIndex { get; set; }
        public string? Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
