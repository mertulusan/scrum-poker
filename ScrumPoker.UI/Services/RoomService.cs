using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using ScrumPoker.Model.Model;
using ScrumPoker.UI.Hubs;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace ScrumPoker.UI.Services
{
    public class RoomService : IRoomService
    {
        readonly IMemoryCache memoryCache;
        readonly private IHubContext<RoomHub> hubContext;

        public RoomService(IMemoryCache memoryCache, IHubContext<RoomHub> hubContext)
        {
            this.memoryCache = memoryCache;
            this.hubContext = hubContext;
        }

        public async Task<bool> JoinRoom(Room model, User user)
        {
            try
            {
                Room room = memoryCache.Get<Room>(model.Name);
                if (room == null)
                {
                    room = model;
                    room.Id = Guid.NewGuid();
                    room.EndDate = DateTime.Now.AddMinutes(20);
                    room.Users = new List<User>();
                    room.VotedTaskList = new List<JiraTask>();
                }
                room.Users.Add(user);
                memoryCache.Set<Room>(room.Name, room, DateTime.Now.AddMinutes(20));

                await this.hubContext.Clients.Group(room.Name).SendAsync("JoinRoomAsync", room);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public async Task<List<User>> GetUsers(string roomName)
        //{
        //    Room room = memoryCache.Get<Room>(roomName);
        //    if (room == null)
        //        return null;

        //    return room.Users;
        //}

        public async Task GetGroupMessages(string roomName)
        {
            Room room = memoryCache.Get<Room>(roomName);
            await hubContext.Clients.Group(room.Name).SendAsync("JoinRoomAsync", room);
        }

        public async Task StartVoting(string roomName, JiraTask task)
        {
            Room room = memoryCache.Get<Room>(roomName);
            room.VotingTask = task;
            await hubContext.Clients.Group(room.Name).SendAsync("SendMessageAsync", room);
        }

        public async Task AddTimeRoom(string roomName)
        {
            Room room = memoryCache.Get<Room>(roomName);
            room.EndDate = room.EndDate.AddMinutes(20);
            memoryCache.Set<Room>(room.Name, room, DateTime.Now.AddMinutes(20));

            await hubContext.Clients.Group(room.Name).SendAsync("SendMessageAsync", room);
        }

    }
}
