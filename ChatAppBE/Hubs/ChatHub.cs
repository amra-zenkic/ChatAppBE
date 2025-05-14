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
        public async Task SendMessage(string userId, string message)
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

            await Clients.All
                    .SendAsync("ReceiveSpecificMessage", fromUserName, message, DateTime.Now);
        }
        
        public async Task JoinSpecificChatRoom()
        {
            var fromUserName = Context.UserIdentifier;
            await Clients.All
                .SendAsync("ReceiveMessage", "admin", $"User: {fromUserName} has joined chat", fromUserName, DateTime.Now);
        }

        public async Task SendPrivateMessage(string fromUserId, string toUserId, string message)
        {
            var fromUserName = Context.UserIdentifier;

            // save the message to the database
            var newMessage = new Message
            {
                Sender = fromUserId,
                Receiver = toUserId,
                Content = message,
                Timestamp = DateTime.Now
            };
            _messageService.AddNewMessage(newMessage);
            var username1 = _userService.GetUserById(ObjectId.Parse(fromUserId));
            var username2 = _userService.GetUserById(ObjectId.Parse(toUserId));

            // Pošalji poruku primatelju
            await Clients.User(username1.Username).SendAsync("ReceivePrivateMessage", fromUserName, message, DateTime.Now);

            // Pošalji i pošiljaocu (echo)
            await Clients.User(username2.Username).SendAsync("ReceivePrivateMessage", fromUserName, message, DateTime.Now);
        }



    }
}