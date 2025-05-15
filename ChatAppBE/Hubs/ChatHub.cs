namespace ChatAppBE.Hubs
{
    using ChatAppBE.Exceptions;
    using ChatAppBE.Models.Models;
    using ChatAppBE.Services.Services.IService;
    using Microsoft.AspNetCore.SignalR;
    using MongoDB.Bson;

    public class ChatHub : Hub
    {
        private readonly IMessagesService _messageService;
        private readonly IUserService _userService;

        public ChatHub(IMessagesService messagesService, IUserService userService)
        {
            _messageService = messagesService;
            _userService = userService;
        }

        public async Task SendMessage(string userId, string message)
        {
            var newMessage = new Message
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Sender = userId,
                Receiver = string.Empty,
                Content = message,
                Timestamp = DateTime.Now,
            };

            _messageService.AddNewMessage(newMessage);
            var user = _userService.GetUserById(ObjectId.Parse(userId)) ?? throw new UserNotFoundException();

            // TODO: Extract magic strings
            await Clients.All.SendAsync("ReceiveSpecificMessage", user.Username, message, newMessage.Timestamp, "group");
        }

        public async Task JoinSpecificChatRoom()
        {
            var fromUserName = Context.UserIdentifier;
            if (string.IsNullOrEmpty(fromUserName))
            {
                throw new ArgumentException("User not found");
            }

            var user = _userService.GetActiveUserByName(fromUserName);

            await Clients.All
                .SendAsync("ReceiveMessage", "admin", $"User: {fromUserName} has joined group chat", user, DateTime.Now);
        }

        public async Task SendPrivateMessage(string fromUserId, string toUserId, string message)
        {
            var newMessage = new Message
            {
                Sender = fromUserId,
                Receiver = toUserId,
                Content = message,
                Timestamp = DateTime.Now,
            };

            _messageService.AddNewMessage(newMessage);

            var sender = _userService.GetUserById(ObjectId.Parse(fromUserId)) ?? throw new UserNotFoundException();
            var receiver = _userService.GetUserById(ObjectId.Parse(toUserId)) ?? throw new UserNotFoundException();

            await Clients.User(receiver.Username)
                .SendAsync("ReceivePrivateMessage", sender, receiver, message, newMessage.Timestamp, fromUserId);

            await Clients.User(sender.Username)
                .SendAsync("ReceivePrivateMessage", sender, receiver, message, newMessage.Timestamp, toUserId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var username = Context.UserIdentifier;
            if (string.IsNullOrEmpty(username))
            {
                throw new UserNotFoundException();
            }

            var user = _userService.GetActiveUserByName(username);

            if (user != null)
            {
                user.Status = "offline";
                _userService.UpdateUserStatusToOffline(user.Id);
                await Clients.All.SendAsync("ReceiveMessage", "admin", $"User: {user.Username} left the chat", user, DateTime.Now);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}