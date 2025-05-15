namespace ChatAppBE.Services.Services.IService
{
    using ChatAppBE.Models.Models;
    using MongoDB.Bson;

    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();

        User? GetUserById(ObjectId id);

        IEnumerable<User> GetAllActiveUsers();

        IEnumerable<User> GetAllPrivateChatStarted(string id);

        User GetActiveUserByName(string username);

        public string GenerateUniqueUsername();

        User AddUser(User newUser);

        void UpdateUserStatusToOffline(string id);

        void DeleteUser(User userToDelete);

        void DeleteAllUsers();
    }
}
