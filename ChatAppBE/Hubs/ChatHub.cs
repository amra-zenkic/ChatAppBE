using ChatAppBE.DataService;
using ChatAppBE.Models;
using Microsoft.AspNetCore.SignalR;
using System;

namespace ChatAppBE.Hubs
{

    public class ChatHub : Hub
    {
        private readonly SharedDB _sharedDB;

        public ChatHub(SharedDB sharedDB)
        {
            _sharedDB = sharedDB;
        }

        public async Task SendMessage(string message)
        {
            if (_sharedDB.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
            {
                await Clients.Group(conn.ChatRoom)
                    .SendAsync("ReceiveSpecificMessage", conn.Username, message, DateTime.Now);
            }
        }

        public async Task JoinSpecificChatRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);

            _sharedDB.connections[Context.ConnectionId] = conn; // dodajemo konekciju sa podacima u bazu

            await Clients.Group(conn.ChatRoom)
                .SendAsync("ReceiveMessage", "admin", $"User: {conn.Username} has joined {conn.ChatRoom}", DateTime.Now);
        }



    }
}