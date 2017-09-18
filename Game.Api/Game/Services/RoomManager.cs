using Game.Api.Constants;
using Game.Api.Game.Models;
using Game.Api.Services;
using Game.Api.Services.Utils;
using Game.Api.WebSocketManager;
using Game.Api.WebSocketManager.Messages;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace Game.Api.Game.Services
{
    public class RoomManager
    {
        private readonly TimeSpan _updateRoomDelay = TimeSpan.FromMilliseconds(30);

        private readonly IDungeonService _dungeonService;
        private readonly WebSocketMessageService _webSocketMessageService;

        private ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();

        public RoomManager(IDungeonService dungeonService, GameRoomMessageService webSocketMessageService)
        {
            _dungeonService = dungeonService;
            _webSocketMessageService = webSocketMessageService;
        }

        public void CreateRoom(string name, long dungeonType)
        {
            if(GetRoom(name) != null)
            {
                return;
            }

            var dungeon = _dungeonService.GetById(dungeonType);

            foreach(var unit in dungeon.Units)
            {
                unit.OnCellChanged += (s, args) => OnUnitCellChanged(s, args, name);
                unit.OnFired += (s, args) => OnUnitFired(s, args, name);
            }

            var room = new Room
            {
                Name = name,
                Dungeon = dungeon,
                CreatedAt = DateTime.UtcNow,
                Clock = new Stopwatch()
            };

            var cancelSource = new CancellationTokenSource();

            _rooms.TryAdd(name, room);
            room.Clock.Start();

            Observable
                .Interval(_updateRoomDelay)
                .Subscribe(x => UpdateRoom(name, cancelSource), cancelSource.Token);
        }

        private async void OnUnitCellChanged(object s, EventArgs args, string room)
        {
            var unit = s as Unit;
            await _webSocketMessageService.SendMessageToGroupAsync(room, WebSocketEvent.UnitState, new UnitStateMessageArgs
            {
                Sid = unit.Sid,
                Position = unit.Position
            });
        }

        private async void OnUnitFired(object s, EventArgs args, string room)
        {
            var unit = s as Unit;
            await _webSocketMessageService.SendMessageToGroupAsync(room, WebSocketEvent.UnitFired, new UnitFiredMessageArgs
            {
                Sid = unit.Sid,
                TargetSid = (unit.Target as Unit).Sid
            });
        }

        public Room GetRoom(string name)
        {
            Room room;
            _rooms.TryGetValue(name, out room);
            return room;
        }

        public void RemoveRoom(string name)
        {
            Room room;
            _rooms.TryRemove(name, out room);
        }

        public void AddPlayer(string roomName, long userId)
        {
            var room = GetRoom(roomName);
            var sid = userId.ToString();

            if(room == null || room.Players.Any(it => it.Sid == sid))
            {
                return;
            }

            var unit = new Unit(room.Dungeon.OriginPosition.Clone(), room.Dungeon, sid, false);
            unit.OnCellChanged += (s, args) => OnUnitCellChanged(s, args, roomName);
            unit.OnFired += (s, args) => OnUnitFired(s, args, roomName);
            room.Dungeon.Units.Add(unit);

            room.Players.Add(new Player
            {
                Sid = sid,
                JoinedAt = DateTime.UtcNow,
                Unit = unit
            });
        }

        public void UpdatePlayer(string roomName, string sid, int x, int y)
        {
            var room = _rooms[roomName];
            var player = room.Players.SingleOrDefault(it => it.Sid == sid);
        }

        public void SetUnitTarget(string roomName, string sid, int x, int y)
        {
            var room = _rooms[roomName];
            var player = room.Players.SingleOrDefault(it => it.Sid == sid);
            player.Unit.Target = new Point { X = x, Y = y };
        }

        private void UpdateRoom(string name, CancellationTokenSource cancelSource)
        {
            var room = GetRoom(name);
            if (room == null)
            {
                cancelSource.Cancel();
                return;
            }

            /*
            room.Clock.Stop();
            var elapsed = room.Clock.Elapsed;
            room.Clock.Start();
            */

            foreach (var unit in room.Dungeon.Units)
            {
                unit.Update(_updateRoomDelay.Milliseconds);
            }
        }
    }
}
