using ChatAppBE.Models;
using ChatAppBE.DTOs;

namespace ChatAppBE.Services
{
    public interface IMessagesService
    {
        void AddNewMessage(Message newMessage); // for both group and private messages
        IEnumerable<MessageDto> GetAllGroupMessages();
        IEnumerable<MessageDto> GetAllPrivateMessages(string user1, string user2);
        void DeleteAllMessages(); // for both group and private messages
    }
}
