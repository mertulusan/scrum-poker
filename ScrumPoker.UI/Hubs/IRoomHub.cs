using ScrumPoker.Model.Enums;
using ScrumPoker.Model.Model;
using System.Threading.Tasks;

namespace ScrumPoker.UI.Hubs
{
    public interface IRoomHub
    {
        Task JoinRoomAsync(string roomName);
        Task ReceiveMessage(Room room);


        //Task SendMessageRoomMateAsync(string roomName, User user, CardPoints cardPoint);
        //Task LeaveRoomAsync(string groupName);
        //Task SendMessage(string user, string message);
        //Task AddTimeRoom(string roomName);
    }
}
