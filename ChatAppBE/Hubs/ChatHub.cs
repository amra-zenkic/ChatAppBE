using ChatAppBE.Models.Models;
using ChatAppBE.Services.Services.IService;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using System;

namespace ChatAppBE.Hubs
{

    public class ChatHub : Hub
    {
        private readonly IMessagesService _messageService;
        private readonly IUserService _userService;
        public ChatHub(IMessagesService messagesService, IUserService userService)
        {
            _messageService = messagesService;
            _userService = userService;
        }
        public async Task SendMessage(string userId, string message) // group messages
        {
            
            var fromUserName = Context.UserIdentifier;

            // save the message to the database
            var newMessage = new Message
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Sender = userId,
                Receiver = "",
                Content = message,
                Timestamp = DateTime.Now
            };
            _messageService.AddNewMessage(newMessage);
            var user = _userService.GetUserById(ObjectId.Parse(userId));
            await Clients.All.SendAsync("ReceiveSpecificMessage", user.Username, message, newMessage.Timestamp, "group");

            // await Clients.All
            //        .SendAsync("ReceiveSpecificMessage", fromUserName, message, DateTime.Now);
        }

        public async Task JoinSpecificChatRoom()
        {
            var fromUserName = Context.UserIdentifier;
            var user = _userService.GetActiveUserByName(fromUserName);

            await Clients.All
                .SendAsync("ReceiveMessage", "admin", $"User: {fromUserName} has joined group chat", user, DateTime.Now);
            //.SendAsync("ReceiveMessage", "admin", $"User: {username} has joined group chat", fromUserName, DateTime.Now);
        }

        public async Task SendPrivateMessage(string fromUserId, string toUserId, string message)
        {
            var newMessage = new Message
            {
                Sender = fromUserId,
                Receiver = toUserId,
                Content = message,
                Timestamp = DateTime.Now
            };
            _messageService.AddNewMessage(newMessage);

            var sender = _userService.GetUserById(ObjectId.Parse(fromUserId));
            var receiver = _userService.GetUserById(ObjectId.Parse(toUserId));

            await Clients.User(receiver.Username)
                .SendAsync("ReceivePrivateMessage", sender, receiver, message, newMessage.Timestamp, fromUserId);

            await Clients.User(sender.Username)
                .SendAsync("ReceivePrivateMessage", sender, receiver, message, newMessage.Timestamp, toUserId);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.UserIdentifier;

            //var user = _userService.GetUserById(ObjectId.Parse(userId)); // ako koristiš username kao identifier
            var user = _userService.GetActiveUserByName(username);

            if (user != null)
            {
                user.Status = "offline";
                _userService.UpdateUserStatusToOffline(user.Id);
                await Clients.All.SendAsync("UserLeft", user.Id, user.Username);
            }

            await base.OnDisconnectedAsync(exception);
        }





    }
}