using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SignalRAPI.Models.Tabel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAPI.Hubs
{
    public class MessageHub : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", message);
        }

        public Task SendCaller(string message)
        {
            return Clients.Caller.SendAsync("SendCaller", message);
        }

        public Task SendMessageToUser(string connectionId, string message)
        {
            return Clients.Client(connectionId).SendAsync("SendMessageToUser", message);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public override Task OnConnectedAsync()
        {
            ConnectedUser.Ids.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            ConnectedUser.Ids.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }

        public static class ConnectedUser
        {
            public static List<string> Ids = new List<string>();
        }
    }

}
