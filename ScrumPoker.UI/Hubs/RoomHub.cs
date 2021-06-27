﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using ScrumPoker.Model.Enums;
using ScrumPoker.Model.Model;
using System;
using System.Linq;
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

        public async Task JoinRoomAsync(Room model, User user)
        {
            Room room = memoryCache.Get<Room>(model.Name);
            if (room == null)
            {
                model.Id = System.Guid.NewGuid();
                model.EndDate = System.DateTime.Now.AddMinutes(20);

                room = memoryCache.Set<Room>(model.Name, model);
            }
            room.Users.Add(user);
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Name);
            await Clients.Group(room.Name).SendAsync("ReceiveMessage", room);
        }

        public async Task SendMessageRoomMateAsync(string roomName, User user, CardPoints cardPoint)
        {
            Room room = memoryCache.Get<Room>(roomName);
            var roomUser = room.Users.FirstOrDefault(p => p.Id == user.Id);
            roomUser.Point = (int)cardPoint;
            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        //public async Task ReceiverMessageRoomMateAsync(string roomName)
        //{
        //    await Clients.Group(roomName).SendAsync("Receiver", $"{Context.ConnectionId} has joined the group {roomName}.");
        //}

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
