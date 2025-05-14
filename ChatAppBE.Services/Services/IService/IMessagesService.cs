using ChatAppBE.Models.Models;
using ChatAppBE.Models.DTOs;

namespace ChatAppBE.Services.Services.IService
{
    public interface IMessagesService
    {
        void AddNewMessage(Message newMessage); // for both group and private messages
        IEnumerable<MessageDto> GetAllGroupMessages();
        IEnumerable<MessageDto> GetAllPrivateMessages(string user1, string user2);
        void DeleteAllMessages(); // for both group and private messages
    }
}
