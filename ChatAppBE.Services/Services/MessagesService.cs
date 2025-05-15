namespace ChatAppBE.Services.Services
{
    using ChatAppBE.Models.DTOs;
    using ChatAppBE.Models.Models;
    using ChatAppBE.Services.Services.IService;
    using MongoDB.Bson;

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
            var messages = _context.Messages
                .Where(m => string.IsNullOrEmpty(m.Receiver))
                .ToList();

            var users = _context.Users.ToList();

            var userDict = users.ToDictionary(u => u.Id!);

            var result = messages.Select(m =>
            {
                var username = userDict.TryGetValue(m.Sender!, out var user)
                    ? user.Username
                    : "Unknown";

                return new MessageDto
                {
                    Username = username,
                    Message = m.Content,
                    SentAt = m.Timestamp,
                };
            });

            return result;
        }

        public (IEnumerable<MessageDto> Messages, bool HasMore) GetGroupMessages(int skip = 0, int take = 20)
        {
            var query = _context.Messages
                .Where(m => string.IsNullOrEmpty(m.Receiver))
                .OrderByDescending(m => m.Timestamp);

            int totalCount = query.Count();

            var messages = query
                .Skip(skip)
                .Take(take)
                .ToList();

            var userIds = messages.Select(m => m.Sender).Distinct().ToList();
            var users = _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToList();

            var userDict = users.ToDictionary(u => u.Id!);

            var result = messages
                .OrderBy(m => m.Timestamp)
                .Select(m =>
                {
                    var username = userDict.TryGetValue(m.Sender!, out var user)
                        ? user.Username
                        : "Unknown";

                    return new MessageDto
                    {
                        Username = username,
                        Message = m.Content,
                        SentAt = m.Timestamp,
                    };
                });

            return (result, totalCount > skip + take);
        }

        public (IEnumerable<MessageDto> Messages, bool HasMore) GetPrivateMessages(string user1, string user2, int skip = 0, int take = 20)
        {
            var query = _context.Messages
                .Where(m =>
                    (m.Sender == user1 && m.Receiver == user2) ||
                    (m.Sender == user2 && m.Receiver == user1))
                .OrderByDescending(m => m.Timestamp);

            int totalCount = query.Count();

            var messages = query
                .Skip(skip)
                .Take(take)
                .ToList();

            var userIds = messages.Select(m => m.Sender).Distinct().ToList();
            var users = _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToList();
            var userDict = users.ToDictionary(u => u.Id);

            var orderedMessages = messages
                .OrderBy(m => m.Timestamp)
                .Select(m => new MessageDto
                {
                    Username = userDict.TryGetValue(m.Sender!, out var user)
                        ? user.Username
                        : "Unknown",
                    Message = m.Content,
                    SentAt = m.Timestamp,
                });

            return (orderedMessages, totalCount > skip + take);
        }
    }
}
