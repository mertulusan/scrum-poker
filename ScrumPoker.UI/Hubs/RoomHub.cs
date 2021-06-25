using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using ScrumPoker.UI.Model;
using System.Threading.Tasks;

namespace ScrumPoker.UI.Hubs
{
    public class RoomHub : Hub
    {
        readonly IMemoryCache memoryCache;

        public RoomHub(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public async Task JoinRoomAsync(Room model, string userInput)
        {
            User user = new User() { Name = userInput };

            Room room = memoryCache.Get<Room>(model.Name);
            if (room != null)
            {
                room.Users.Add(user);
                await Clients.Group(model.Name).SendAsync("ReceiveMessage", userInput, $"{Context.ConnectionId} has joined the group {model.Name}.");
            }
            else
            {
                model.Id = System.Guid.NewGuid();
                model.EndDate = System.DateTime.Now.AddMinutes(20);
                model.Users.Add(user);

                memoryCache.Set<Room>(model.Name, model);
                await Groups.AddToGroupAsync(Context.ConnectionId, model.Name);
            }
        }

        public async Task SendMessageRoomMateAsync(string roomName)
        {
            await Clients.Group(roomName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {roomName}.");
        }

        public async Task ReceiverMessageRoomMateAsync(string roomName)
        {
            await Clients.Group(roomName).SendAsync("Receiver", $"{Context.ConnectionId} has joined the group {roomName}.");
        }

        public async Task LeaveRoomAsync(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
