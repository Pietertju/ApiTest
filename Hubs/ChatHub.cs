
using ApiTest.Authentication;
using ApiTest.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiTest.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        internal static List<ChatResponse> messages = new List<ChatResponse>();
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();
        internal static List<string> Users = new List<string>();

        public async Task SendUserMessage(string message)
        {
            var chatResponse = new ChatResponse()
            {
                Username = Context.User.Identity.Name,
                Message = message,
                time = DateTime.Now.ToString("h:mm:ss tt")
            };

            messages.Add(chatResponse);
            if(messages.Count() > 10)
            {
                messages.RemoveAt(0);
            }

            await Clients.All.SendAsync("giveMessage", chatResponse);
        }

        public async Task SendAdminMessage(string message)
        {
            var chatResponse = new ChatResponse()
            {
                Username = Context.User.Identity.Name,
                Message = message,
                time = DateTime.Now.ToString("h:mm:ss tt")
            };

            if (messages.Count() > 10)
            {
                messages.RemoveAt(0);
            }

            await Clients.Group(UserRoles.Admin).SendAsync("giveMessage", chatResponse);       
        }

        public async Task SendMessageToUser(string username, string message)
        {
            var chatResponse = new ChatResponse()
            {
                Username = Context.User.Identity.Name,
                Message = message,
                time = DateTime.Now.ToString("h:mm:ss tt")
            };

            foreach (var connectionId in _connections.GetConnections(username))
            {
                await Clients.Client(connectionId).SendAsync("giveMessage", chatResponse);
            }
        }
        public async Task GetUserMessages()
        {   
            foreach(ChatResponse c in messages)
            {
                await Clients.Caller.SendAsync("backlogMessages", c.Message, c.Username, c.time);
            }    
        }

        public override async Task OnConnectedAsync()
        {

            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);

            if(Context.User.IsInRole(UserRoles.Admin))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, UserRoles.Admin);
            }

            if(Context.User.IsInRole(UserRoles.User))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, UserRoles.User);
            }

            if(Context.User.IsInRole(UserRoles.Guest))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, UserRoles.Guest);
            }

            await Clients.Others.SendAsync("setClientMessage", "A connection with username '" + name + "' has just connected", DateTime.Now.ToString("h:mm:ss tt"));
            await Clients.All.SendAsync("setUsersConnected", _connections.GetConnectedUsers());
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);

            await Clients.Others.SendAsync("setClientMessage", "A connection with username '" + name + "' has just disconnected", DateTime.Now.ToString("h:mm:ss tt"));
            await Clients.All.SendAsync("setUsersConnected", _connections.GetConnectedUsers());
            
            await base.OnDisconnectedAsync(exception);
        }

    }
}
