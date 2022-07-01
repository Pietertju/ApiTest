
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ApiTest.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendConnectionId(string connectionId)
        {
            await Clients.All.SendAsync("setClientMessage", "A connection with ID '" + connectionId + "' has just connected");
        }
    }
}
