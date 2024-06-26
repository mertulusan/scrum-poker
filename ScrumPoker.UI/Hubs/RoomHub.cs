﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using ScrumPoker.Common;
using ScrumPoker.Exceptions;
using ScrumPoker.Model.Enums;
using ScrumPoker.Model.Model;
using System;
using System.Collections.Generic;
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
                model.EndDate = DateTime.Now.AddMinutes(20);

                room = memoryCache.Set<Room>(model.Name, model, DateTime.Now.AddMinutes(20));
            }

            user.ConnectionId = Context.ConnectionId;
            room.Users.Add(user);
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Name);
            await Clients.Group(room.Name).SendAsync("ReceiveMessage", room);
        }

        public async Task SendMessageRoomMateAsync(string roomName, User user, CardPoints cardPoint)
        {
            Room room = memoryCache.Get<Room>(roomName);
            var roomUser = room.Users.FirstOrDefault(p => p.Name == user.Name);
            roomUser.Point = cardPoint;

            if (!room.Users.Any(p => p.Point == null && p.Role == RoleType.DEV))
            {
                room.VotingTask.Status = JiraTaskStatus.Completed;
                var votedUsers = room.Users.Where(p => p.Role == RoleType.DEV && (int)p.Point >= 0).ToList();
                room.VotingTask.Average = Convert.ToDecimal(votedUsers.Any() ? votedUsers.Average(p => (int)p.Point) : 0);
            }

            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        public async Task GetGroupMessages(string roomName)
        {
            Room room = memoryCache.Get<Room>(roomName);
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Name);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        public async Task StartVoting(string roomName, JiraTask task)
        {
            Room room = memoryCache.Get<Room>(roomName);

            var taskId = !room.VotedTaskList.Any() ? 1 : room.VotedTaskList.Max(p => p.Id) + 1;
            task.Id = taskId;
            task.Name = $"Task {taskId}";

            room.VotingTask = task;

            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        public async Task RestartVoting(string roomName)
        {
            var room = memoryCache.Get<Room>(roomName);
            room.Users.ForEach(x => { x.Point = null; });
            room.VotingTask = new JiraTask { Status = JiraTaskStatus.Voting };

            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        public async Task StopVoting(string roomName)
        {
            Room room = memoryCache.Get<Room>(roomName);

            room.VotingTask.Status = JiraTaskStatus.Completed;
            var votedUsers = room.Users.Where(p => p.Role == RoleType.DEV && p.Point != null && (int)p.Point >= 0).ToList();
            room.VotingTask.Average = Convert.ToDecimal(votedUsers.Any() ? votedUsers.Average(p => (int)p.Point) : 0);

            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        public async Task SaveTask(string roomName, string votedTaskName, CardPoints comfirmedPoint)
        {
            Room room = memoryCache.Get<Room>(roomName);

            if (room.VotingTask != null)
            {
                room.VotingTask.Name = !string.IsNullOrEmpty(votedTaskName) ? votedTaskName : $"Task - {room.VotingTask.Id}";
                room.VotingTask.ComfirmedPoint = comfirmedPoint;
                room.VotingTask.UserVotes = new List<UserVote>();
                room.Users.ForEach(f =>
                {
                    if (f.Point != null)
                        room.VotingTask.UserVotes.Add(new UserVote { Username = f.Name, Vote = (int)f.Point });
                });
                room.VotedTaskList.Add(room.VotingTask);
            }

            room.VotingTask = null;
            room.Users.ForEach(f => f.Point = null);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task AddTimeRoom(string roomName)
        {
            Room room = memoryCache.Get<Room>(roomName);
            room.EndDate = room.EndDate.AddMinutes(20);
            memoryCache.Set<Room>(roomName, room, DateTime.Now.AddMinutes(20));
            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        public async Task LeaveRoomAsync(string roomName, string userName)
        {
            Room room = memoryCache.Get<Room>(roomName);
            var user = room.Users.FirstOrDefault(p => p.Name.Equals(userName));
            room.Users.Remove(user);
            await Groups.RemoveFromGroupAsync(user?.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        public async Task CloseRoomAsync(string roomName)
        {
            Room room = memoryCache.Get<Room>(roomName);

            if (room != null)
                memoryCache.Remove(roomName);
            
            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }

        public async Task FinishSession(string roomName)
        {
            Room room = memoryCache.Get<Room>(roomName);
            room.SessionIsEnd = true;
            try
            {
                room.Winner = VotePointHelper.GetBestPredictionUsers(room.VotedTaskList);
            }
            catch (PredictionException ex)
            {
                room.Winner = ex.Message;
            }
            await Clients.Group(roomName).SendAsync("ReceiveMessage", room);
        }
    }
}
