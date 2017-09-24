using Game.Api.Constants;
using Game.Api.Game.Models;
using Game.Api.Game.Models.Abilities;
using Game.Api.Game.Profiles;
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

            foreach(var gameObject in dungeon.GameObjects.Where(it => it.Value.Type == GameObjectType.Unit))
            {
                var unit = (gameObject.Value as Unit);
                unit.OnCellChanged += (s, args) => OnUnitCellChanged(s, args, name);
                unit.OnAbilityUsed += (s, args) => OnUnitAbilityUsed(s, args, name);
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
            await _webSocketMessageService.SendMessageToGroupAsync(room, WebSocketEvent.GameObjectState, new GameObjectStateMessageArgs
            {
                GameObject = GameProfiles.Map(unit)
            });
        }

        private async void OnUnitAbilityUsed(object s, AbilityUsedEventArgs args, string roomName)
        {
            var unit = s as Unit;
            var bulletSid = Guid.NewGuid().ToString();

            if (args.IsRanged)
            {
                var room = GetRoom(roomName);
                var bullet = new Bullet(bulletSid, unit.ScreenPositionCenter, unit.Target as Unit, 5);
                bullet.OnDamageDealt += (sBullet, e) => OnDamageDealt(sBullet, e, room);
                room.Dungeon.GameObjects.TryAdd(bulletSid, bullet);
            }

            await _webSocketMessageService.SendMessageToGroupAsync(roomName, WebSocketEvent.UseAbility, new UseAbilityMessageArgs
            {
                Sid = unit.Sid,
                TargetSid = (unit.Target as Unit).Sid,
                BulletSid = bulletSid,
                AbilityType = args.AbilityType
            });
        }

        private async void OnDamageDealt(object s, DamageDealtEventArgs args, Room room)
        {
            var gameObject = s as GameObject;
            room.Dungeon.GameObjects.TryRemove(gameObject.Sid, out gameObject);

            await _webSocketMessageService.SendMessageToGroupAsync(room.Name, WebSocketEvent.DealDamage, new DealDamageMessageArgs
            {
                TargetSid = args.Target.Sid,
                Damage = args.Damage
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
            unit.OnAbilityUsed += (s, args) => OnUnitAbilityUsed(s, args, roomName);
            room.Dungeon.GameObjects.TryAdd(sid, unit);

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

            foreach (var gameObject in room.Dungeon.GameObjects)
            {
                gameObject.Value.Update(_updateRoomDelay.Milliseconds);
            }
        }
    }
}
