namespace ChatAppBE.Services.Services
{
    using ChatAppBE.Models.Models;
    using ChatAppBE.Services.Services.IService;
    using Microsoft.EntityFrameworkCore;
    using MongoDB.Bson;

    public class UserService : IUserService
    {
        private readonly ChatAppDbContext _context;

        public UserService(ChatAppDbContext context)
        {
            _context = context;
        }

        public string GenerateUniqueUsername()
        {
            var adjectives = new[] { "Fast", "Smart", "Happy", "Crazy", "Silent", "Golden", "Secret" };
            var nouns = new[] { "Lion", "Tiger", "Dragon", "Wolf", "Bear", "Falcon", "Lynx" };
            var random = new Random();
            string username;
            User? existingUser = null;

            do
            {
                var adjective = adjectives[random.Next(adjectives.Length)];
                var noun = nouns[random.Next(nouns.Length)];
                var timePart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString().Substring(9);
                var number = random.Next(0, 100).ToString("D2");

                username = $"{adjective}{noun}{timePart}{number}";
                existingUser = _context.Users.FirstOrDefault(u => u.Username == username && u.Status == "online");
            }
            while (existingUser != null);

            return username;
        }

        public User AddUser(User newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Id))
            {
                newUser.Id = ObjectId.GenerateNewId().ToString();
            }

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return newUser;
        }

        public void DeleteUser(User userToDelete)
        {
            var user = _context.Users.Where(c => c.Id == userToDelete.Id).FirstOrDefault();
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("The user to delete cannot be found");
            }
        }

        public IEnumerable<User> GetAllActiveUsers()
        {
            return _context.Users.AsNoTracking().Where(c => c.Status == "online").AsEnumerable();
        }

        public IEnumerable<User> GetAllPrivateChatStarted(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null || user.PrivateChatUserIds == null || user.PrivateChatUserIds.Count == 0)
            {
                return [];
            }

            var chatUsers = _context.Users
                .Where(u => user.PrivateChatUserIds.Contains(u.Id) && u.Status == "online")
                .ToList();

            return chatUsers;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.AsNoTracking().AsEnumerable();
        }

        public User? GetUserById(ObjectId id)
        {
            return _context.Users.FirstOrDefault(c => c.Id == id.ToString());
        }

        public void DeleteAllUsers()
        {
            var users = _context.Users.ToList();
            foreach (var user in users)
            {
                _context.Users.Remove(user);
            }

            _context.SaveChanges();
        }

        public void UpdateUserStatusToOffline(string? id)
        {
            if (id == null)
            {
                throw new ArgumentException("The user id cannot be null or empty");
            }

            var user = _context.Users.Where(c => c.Id == id).FirstOrDefault();
            if (user != null)
            {
                user.Status = "offline";
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("The user to update cannot be found");
            }
        }

        public User GetActiveUserByName(string username)
        {
            var activeUser = _context.Users.Where(c => c.Username == username && c.Status == "online").FirstOrDefault()
                ?? throw new ArgumentNullException(nameof(username), "No user.");

            return activeUser;
        }
    }
}
