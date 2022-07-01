
using ApiTest.Authentication;
using ApiTest.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTest.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        internal static List<ChatResponse> messages = new List<ChatResponse>();
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
            await Clients.All.SendAsync("giveMessage", chatResponse.Message, chatResponse.Username, chatResponse.time);
        }

        public async Task GetUserMessages()
        {   
            foreach(ChatResponse c in messages)
            {
                await Clients.Caller.SendAsync("backlogMessages", c.Message, c.Username, c.time);
            }    
        }

        internal static List<string> Users = new List<string>();

        public override async Task OnConnectedAsync()
        {
            var connId = Context.User.Identity.Name;
            if (!Users.Any(x => x == connId))
            {
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

                Users.Add(connId);
                Debug.WriteLine("\n added user " + connId);
                await Clients.Others.SendAsync("setClientMessage", "A connection with ID '" + connId + "' has just connected", DateTime.Now.ToString("h:mm:ss tt"));
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.User.Identity.Name;
            if (Users.Remove(id))
            {
                Debug.WriteLine("\n removed user " + id);
                await Clients.Others.SendAsync("setClientMessage", "A connection with ID '" + id + "' has just disconnected", DateTime.Now.ToString("h:mm:ss tt"));
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
