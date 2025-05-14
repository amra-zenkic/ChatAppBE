using ChatAppBE.DTOs;
using ChatAppBE.Models;
using MongoDB.Bson;

namespace ChatAppBE.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly ChatAppDbContext _context;

        public MessagesService(ChatAppDbContext context)
        {
            _context = context;
        }
        public void AddNewMessage(Message newMessage)
        {
            newMessage.Id = ObjectId.GenerateNewId().ToString();
            _context.Messages.Add(newMessage);
            _context.SaveChanges();
        }

        public void DeleteAllMessages()
        {
            _context.Messages.RemoveRange(_context.Messages);
            _context.SaveChanges();
        }

        public IEnumerable<MessageDto> GetAllGroupMessages()
        {
            // load messages and users in parallel
            var messages = _context.Messages
                .Where(m => string.IsNullOrEmpty(m.Receiver))
                .ToList();

            var users = _context.Users.ToList();

            // optimise user search by creating a dictionary
            var userDict = users.ToDictionary(u => u.Id);

            var result = messages.Select(m =>
            {
                var username = userDict.TryGetValue(m.Sender, out var user)
                    ? user.Username
                    : "Unknown";

                return new MessageDto
                {
                    Username = username,
                    Message = m.Content,
                    SentAt = m.Timestamp
                };
            });

            return result;
        }
        public IEnumerable<MessageDto> GetAllPrivateMessages(string user1, string user2)
        {
            // load all private messages between two users
            var messages = _context.Messages
                .Where(m =>
                    (m.Sender == user1 && m.Receiver == user2) ||
                    (m.Sender == user2 && m.Receiver == user1))
                .OrderBy(m => m.Timestamp)
                .ToList(); 

            // load users
            var userIds = messages.Select(m => m.Sender).Distinct().ToList();
            var users = _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToList();

            var userDict = users.ToDictionary(u => u.Id);

            // Mapiraj u MessageDto sa username-om
            var result = messages.Select(m =>
            {
                var username = userDict.TryGetValue(m.Sender, out var user)
                    ? user.Username
                    : "Unknown";

                return new MessageDto
                {
                    Username = username,
                    Message = m.Content,
                    SentAt = m.Timestamp
                };
            });

            return result;
        }
        



        /*
        public IEnumerable<Message> GetAllPrivateMessages(string user1, string user2)
        {
            return _context.Messages
                .Where(m =>
                    (m.Sender == user1 && m.Receiver == user2) ||
                    (m.Sender == user2 && m.Receiver == user1))
                .OrderBy(m => m.Timestamp)
                .AsEnumerable();
        }
        */

    }
}
