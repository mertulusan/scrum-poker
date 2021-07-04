using ScrumPoker.Model.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrumPoker.UI.Services
{
    public interface IRoomService
    {
        Task<bool> JoinRoom(Room model, User user);

        Task<List<User>> GetUsers(string roomName);
    }
}
