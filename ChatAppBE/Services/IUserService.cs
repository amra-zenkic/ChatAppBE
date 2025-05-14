using ChatAppBE.Models;
using MongoDB.Bson;

namespace ChatAppBE.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(ObjectId id);
        IEnumerable<User> GetAllActiveUsers();
        IEnumerable<User> GetAllPrivateChatStarted(string id);

        public string GenerateUniqueUsername();
        User AddUser(User newUser);
        void UpdateUserStatusToOffline(string id);
        void DeleteUser(User userToDelete);
        void DeleteAllUsers();
    }
}
