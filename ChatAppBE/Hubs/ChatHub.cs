using ChatAppBE.Models;
using ChatAppBE.Services;
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
            var username = _userService.GetUserById(ObjectId.Parse(fromUserName)).Username;
            await Clients.All
                .SendAsync("ReceiveMessage", "admin", $"User: {username} has joined chat", fromUserName, DateTime.Now);
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

            await Clients.User(receiver.Id)
                .SendAsync("ReceivePrivateMessage", sender.Username, message, newMessage.Timestamp, fromUserId);

            await Clients.User(sender.Id)
                .SendAsync("ReceivePrivateMessage", sender.Username, message, newMessage.Timestamp, toUserId);
        }




    }
}