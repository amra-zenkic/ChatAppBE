﻿namespace ChatAppBE.Services.Services
{
    using Microsoft.AspNetCore.SignalR;

    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext()?.Request.Query["userId"];
        }
    }
}