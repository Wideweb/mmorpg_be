using Common.Api.WebSocketManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Api.WebSocketManager;
using Identity.Api.Constants;
using Clients.GameClient.Dto;
using Identity.Api.Services.Exceptions;
using Identity.Api.WebSocketManager.Messages;
using Clients.GameClient;

namespace Identity.Api.Services
{
    public class RoomManager
    {   
        private readonly WebSocketMessageService _webSocketMessageService;
        private readonly GameHttpClient _gameHttpClient;
        private readonly IMembershipService _membershipService;
        private readonly IdentityConnectionManager _webSocketConnectionManager;

        private ConcurrentDictionary<string, CreateGameDto> _rooms = new ConcurrentDictionary<string, CreateGameDto>();

        public RoomManager(IdentityMessageService webSocketMessageService, 
            GameHttpClient gameHttpClient, 
            IMembershipService membershipService,
            IdentityConnectionManager webSocketConnectionManager)
        {
            _webSocketMessageService = webSocketMessageService;
            _gameHttpClient = gameHttpClient;
            _membershipService = membershipService;
            _webSocketConnectionManager = webSocketConnectionManager;
        }

        public async Task CreateRoom(string name, long dungeonType)
        {
            if(GetRoom(name) != null)
            {
                return;
            }
            
            var room = new CreateGameDto
            {
                Name = name,
                DungeonType = dungeonType
            };

            _rooms.TryAdd(name, room);

            await _webSocketMessageService.SendMessageToGroupAsync("identity", IdentityWebSocketEvent.RoomAdded, new RoomAddedMessageArgs
            {
                Room = room
            });
        }

        public async Task<bool> StartGame(string roomName)
        {
            var room = GetRoom(roomName);
            if (room == null)
            {
                throw new RoomNotFoundException(roomName);
            }

            if (room.IsStarted)
            {
                return false;
            }

            room.IsStarted = true;
            
            if(!await _gameHttpClient.CreateGame(room))
            {
                room.IsStarted = false;
                return false;
            }

            await _webSocketMessageService.SendMessageToGroupAsync("identity", IdentityWebSocketEvent.RoomRemoved, new RoomRemovedMessageArgs
            {
                RoomName = roomName
            });

            await _webSocketMessageService.SendMessageToGroupAsync(roomName, IdentityWebSocketEvent.GameStarted);

            return true;
        }
        
        public CreateGameDto GetRoom(string name)
        {
            CreateGameDto room;
            _rooms.TryGetValue(name, out room);
            return room;
        }

        public List<CreateGameDto> GetRooms()
        {
            return _rooms.Select(it => it.Value).Where(it => !it.IsStarted).ToList();
        }

        public async Task RemoveRoom(string name)
        {
            CreateGameDto room;
            _rooms.TryRemove(name, out room);

            await _webSocketMessageService.SendMessageToGroupAsync("identity", IdentityWebSocketEvent.RoomRemoved, new RoomRemovedMessageArgs
            {
                RoomName = name
            });
        }

        public async Task AddPlayer(string roomName, string sid, string userName)
        {
            var room = GetRoom(roomName);
            if (room == null)
            {
                throw new RoomNotFoundException(roomName);
            }

            if (room.Players.Any(it => it.Sid == sid))
            {
                return;
            }

            var player = new CreatePlayerDto
            {
                Sid = sid,
                Name = userName,
                JoinedAt = DateTime.UtcNow
            };

            room.Players.Add(player);

            _webSocketConnectionManager.JoinGroup(roomName, sid);
            await _webSocketMessageService.SendMessageToGroupAsync(roomName, IdentityWebSocketEvent.PlayerJoined, new PlayerJoinedMessageArgs
            {
                Player = player
            });
        }

        public async Task RemovePlayer(string roomName, string sid)
        {
            var room = GetRoom(roomName);
            if (room == null)
            {
                return;
            }
            
            var player = room.Players.FirstOrDefault(it => it.Sid == sid);

            if(player == null)
            {
                return;
            }

            room.Players.Remove(player);
            _webSocketConnectionManager.RemoveFromGroup(roomName, sid);

            if (!room.Players.Any())
            {
                await RemoveRoom(roomName);
            }
            else
            {
                await _webSocketMessageService.SendMessageToGroupAsync(roomName, IdentityWebSocketEvent.PlayerLeft, new PlayerLeftMessageArgs
                {
                    Sid = sid
                });
            }
        }
    }
}
