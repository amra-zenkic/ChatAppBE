namespace ChatAppBE.Models.DTOs
{
    public class MessageDto
    {
        public string? Username { get; set; }

        public string? Message { get; set; }

        public DateTime SentAt { get; set; }
    }
}
