using Game.Api.Constants;
using Game.Api.Game;
using Game.Api.Models.WebSocket;
using Game.Api.WebSocketManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace Game.Api.Services
{
    public class RoomManager
    {
        private readonly TimeSpan _updateRoomDelay = TimeSpan.FromSeconds(5);

        private readonly DungeonService _dungeonService;
        private readonly WebSocketMessageService _webSocketMessageService;

        private ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();

        public RoomManager(DungeonService dungeonService, WebSocketMessageService webSocketMessageService)
        {
            _dungeonService = dungeonService;
            _webSocketMessageService = webSocketMessageService;
        }

        public void CreateRoom(string name, long dungeonType)
        {
            var dungeon = _dungeonService.GetById(dungeonType);

            foreach(var unit in dungeon.Units)
            {
                unit.OnCellChanged += (s, args) => OnUnitCellChanged(s, args, name);
            }

            var room = new Room
            {
                Name = name,
                Dungeon = dungeon,
                CreatedAt = DateTime.UtcNow
            };

            var cancelSource = new CancellationTokenSource();

            _rooms.TryAdd(name, room);

            Observable
                .Interval(_updateRoomDelay)
                .Subscribe(x => UpdateRoom(x, name, cancelSource), cancelSource.Token);
        }

        private async void OnUnitCellChanged(object s, EventArgs args, string room)
        {
            var unit = s as Unit;
            await _webSocketMessageService.SendMessageToGroupAsync(room, WebSocketEvent.UnitState, new UnitStateMessageArgs
            {
                Id = unit.Sid,
                X = unit.Position.X,
                Y = unit.Position.Y
            });
        }

        public void RemoveRoom(string name)
        {
            Room room;
            _rooms.TryRemove(name, out room);
        }

        public void AddPlayer(string roomName, long userId)
        {
            var room = _rooms[roomName];
            room.Players.Add(new Player
            {
                Sid = userId.ToString(),
                JoinedAt = DateTime.UtcNow,
                X = 1,
                Y = 1
            });
        }

        public List<Player> GetPlayers(string roomName)
        {
            var room = _rooms[roomName];
            return room.Players;
        }

        public List<Unit> GetUnits(string roomName)
        {
            var room = _rooms[roomName];
            return room.Dungeon.Units;
        }

        public void UpdatePlayer(string roomName, string sid, int x, int y)
        {
            var room = _rooms[roomName];
            var player = room.Players.SingleOrDefault(it => it.Sid == sid);
            player.X = x;
            player.Y = y;
        }

        private void UpdateRoom(long elapsed, string name, CancellationTokenSource cancelSource)
        {
            if (!_rooms.ContainsKey(name))
            {
                cancelSource.Cancel();
                return;
            }

            var room = _rooms[name];

            foreach(var unit in room.Dungeon.Units)
            {
                unit.Update(elapsed);
            }
        }
    }
}
