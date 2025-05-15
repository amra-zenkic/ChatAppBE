namespace ChatAppBE.Controllers
{
    using ChatAppBE.Models.Models;
    using ChatAppBE.Services.Services.IService;
    using Microsoft.AspNetCore.Mvc;

    [Route("messages")]
    public class MessageController : Controller
    {
        private readonly IMessagesService _messageService;

        public MessageController(IMessagesService messagesService)
        {
            _messageService = messagesService;
        }

        [HttpPost("send")] // TODO: Use DTO for message
        public IActionResult SendMessage(string sender, string receiver, string content)
        {
            var newMessage = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Content = content,
            };

            _messageService.AddNewMessage(newMessage);
            return Ok();
        }

        [HttpGet("group/all")]
        public IActionResult GetAllGroupMessages()
        {
            var messages = _messageService.GetAllGroupMessages();
            return Ok(messages);
        }

        [HttpGet("group")]
        public IActionResult GetGroupMessages([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var (messages, hasMore) = _messageService.GetGroupMessages(skip, take);

            return Ok(new
            {
                messages,
                hasMore,
            });
        }

        [HttpGet("private/{user1}/{user2}")]
        public IActionResult GetAllPrivateMessages(string user1, string user2, int skip = 0, int take = 20)
        {
            var (messages, hasMore) = _messageService.GetPrivateMessages(user1, user2, skip, take);
            return Ok(new
            {
                messages,
                hasMore,
            });
        }

        [HttpDelete("delete/all")]
        public IActionResult DeleteAllMessages()
        {
            _messageService.DeleteAllMessages();
            return Ok();
        }
    }
}
