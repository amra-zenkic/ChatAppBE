using Microsoft.AspNetCore.SignalR;
using System;

public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        // reads userId from the query string
        return connection.GetHttpContext()?.Request.Query["userId"];
    }
}
