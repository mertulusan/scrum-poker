using Microsoft.AspNetCore.SignalR;
using ScrumPoker.Model.Enums;
using ScrumPoker.Model.Model;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPoker.UI.Hubs
{
    public class RoomHub : Hub
    {
        public async Task JoinRoomAsync(Room room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Name);
            await Clients.Group(room.Name).SendAsync("ReceiveMessage", room);
        }

        public async Task SendMessageAsync(Room room)
        {
            await Clients.Group(room.Name).SendAsync("ReceiveMessage", room);
        }

        //public async Task GetGroupMessages(Room room)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, room.Name);
        //    await Clients.Group(room.Name).SendAsync("ReceiveMessage", room);
        //}

        //public async Task LeaveRoomAsync(string groupName)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        //}

        //public async Task AddTimeRoom(Room room)
        //{
        //    await Clients.Group(room.Name).SendAsync("ReceiveMessage", room);
        //}

    }
}
