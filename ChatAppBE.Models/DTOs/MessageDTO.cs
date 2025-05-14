namespace ChatAppBE.Models.DTOs
{
    public class MessageDto
    {
        //public string ChatRoom { get; set; } = "group"; // or id of the user that our user is chatting with
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }

}
