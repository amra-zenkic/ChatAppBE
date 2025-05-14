using ChatAppBE.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBE.Controllers
{
    [Route("messages")]
    public class MessageController : Controller
    {
        private readonly IMessagesService _messageService;
        public MessageController(IMessagesService messagesService)
        {
            _messageService = messagesService;
        }

        [HttpPost("send")]
        public IActionResult SendMessage(string sender, string receiver, string content)
        {
            var newMessage = new Models.Message
            {
                Sender = sender,
                Receiver = receiver,
                Content = content
            };
            _messageService.AddNewMessage(newMessage);
            return Ok();
        }
        [HttpGet("group")]
        public IActionResult GetAllGroupMessages()
        {
            var messages = _messageService.GetAllGroupMessages();
            return Ok(messages);
        }
        [HttpGet("private/{user1}/{user2}")]
        public IActionResult GetAllPrivateMessages(string user1, string user2)
        {
            var messages = _messageService.GetAllPrivateMessages(user1, user2);
            return Ok(messages);
        }
        [HttpDelete("delete/all")]
        public IActionResult DeleteAllMessages()
        {
            _messageService.DeleteAllMessages();
            return Ok();
        }

    }
}
