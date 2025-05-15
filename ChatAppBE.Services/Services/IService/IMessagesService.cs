namespace ChatAppBE.Services.Services.IService
{
    using ChatAppBE.Models.DTOs;
    using ChatAppBE.Models.Models;

    public interface IMessagesService
    {
        void AddNewMessage(Message newMessage);

        IEnumerable<MessageDto> GetAllGroupMessages();

        void DeleteAllMessages();

        public (IEnumerable<MessageDto> Messages, bool HasMore) GetGroupMessages(int skip = 0, int take = 20);

        public (IEnumerable<MessageDto> Messages, bool HasMore) GetPrivateMessages(string user1, string user2, int skip, int take);
    }
}
