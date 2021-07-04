using Microsoft.AspNetCore.SignalR;
using ScrumPoker.Model.Model;
using System.Threading.Tasks;

namespace ScrumPoker.UI.Hubs
{
    public class RoomHub : Hub<IRoomHub>
    {
        public async Task JoinRoomAsync(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendMessage(Room room)
        {
            await Clients.Group(room.Name).ReceiveMessage(room);
        }


        //public async Task SendMessageRoomMateAsync(string roomName, User user, CardPoints cardPoint)
        //{
        //    Room room = memoryCache.Get<Room>(roomName);
        //    var roomUser = room.Users.FirstOrDefault(p => p.Id == user.Id);
        //    roomUser.Point = (int)cardPoint;
        //    //  await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        //}


        //public async Task SendMessage(string user, string message)
        //{
        //    //  await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}

    }
}
