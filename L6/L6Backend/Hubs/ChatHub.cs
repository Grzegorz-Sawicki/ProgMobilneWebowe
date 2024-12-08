using L6Backend.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace L6Backend.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string groupName, string user, string message)
        {
            Console.WriteLine("Sending message: " + message + " from user: " + user);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", groupName, user, message);
        }

        public async Task CreateGroup(string groupName)
        {
            Console.WriteLine("Creating group: " + groupName);
            await Clients.All.SendAsync("CreatedGroup", groupName);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
