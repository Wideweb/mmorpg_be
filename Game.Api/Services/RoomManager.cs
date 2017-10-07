using Common.Api.WebSocketManager;
using Game.Api.Constants;
using Game.Api.Models;
using Game.Api.Models.Abilities;
using Game.Api.Profiles;
using Game.Api.Models.GameEventArgs;
using Game.Api.Services.Exceptions;
using Game.Api.WebSocketManager;
using Game.Api.WebSocketManager.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Clients.IdentityClient;
using Clients.GameClient.Dto;

namespace Game.Api.Services
{
    public class RoomManager
    {
        private readonly TimeSpan _updateRoomDelay = TimeSpan.FromMilliseconds(30);

        private readonly IDungeonService _dungeonService;
        private readonly WebSocketMessageService _webSocketMessageService;
        private readonly IdentityHttpClient _identityHttpClient;

        private ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();

        public RoomManager(IDungeonService dungeonService, 
            GameRoomMessageService webSocketMessageService,
            IdentityHttpClient identityHttpClient)
        {
            _dungeonService = dungeonService;
            _webSocketMessageService = webSocketMessageService;
            _identityHttpClient = identityHttpClient;
        }

        public void CreateRoom(CreateGameDto startGameDto)
        {
            var room = AddRoom(startGameDto.Name, startGameDto.DungeonType);

            if(room == null)
            {
                return;
            }

            foreach(var player in startGameDto.Players)
            {
                AddPlayer(room, player.Sid, player.Name);
            }

            StartGame(room);
        }

        private Room AddRoom(string roomName, long dungeonType)
        {
            if (GetRoom(roomName) != null)
            {
                return null;
            }

            var dungeon = _dungeonService.GetById(dungeonType);

            foreach (var gameObject in dungeon.GameObjects.Where(it => it.Value.Type == GameObjectType.Unit))
            {
                var unit = (gameObject.Value as Unit);
                unit.OnCellChanged += (s, args) => OnUnitCellChanged(s, args, roomName);
                unit.OnAbilityUsed += (s, args) => OnUnitAbilityUsed(s, args, roomName);
                unit.OnDied += (s, args) => OnUnitDied(s, args, roomName);
            }

            var room = new Room
            {
                Name = roomName,
                Dungeon = dungeon,
                CreatedAt = DateTime.UtcNow,
                Clock = new Stopwatch()
            };

            if(_rooms.TryAdd(roomName, room))
            {
                return room;
            }

            return null;
        }

        private void AddPlayer(Room room, string sid, string name)
        {
            if (room.Players.Any(it => it.Sid == sid))
            {
                return;
            }

            var unit = new Unit(room.Dungeon.OriginPosition.Clone(), room.Dungeon, sid, false, 100);
            unit.Name = name;
            unit.OnCellChanged += (s, args) => OnUnitCellChanged(s, args, room.Name);
            unit.OnAbilityUsed += (s, args) => OnUnitAbilityUsed(s, args, room.Name);
            unit.OnDied += (s, args) => OnUnitDied(s, args, room.Name);
            room.Dungeon.GameObjects.TryAdd(sid, unit);

            var player = new Player
            {
                Sid = sid,
                Name = name,
                JoinedAt = DateTime.UtcNow,
                Unit = unit
            };

            room.Players.Add(player);
        }

        private void StartGame(Room room)
        {
            if (room.IsStarted)
            {
                return;
            }

            room.IsStarted = true;
            room.Clock.Start();
            
            var cancelSource = new CancellationTokenSource();

            Observable
                .Interval(_updateRoomDelay)
                .Subscribe(x => UpdateRoom(room.Name, cancelSource), cancelSource.Token);
        }

        private async void OnUnitCellChanged(object s, CellChangedEventArgs args, string room)
        {
            var unit = s as Unit;
            await _webSocketMessageService.SendMessageToGroupAsync(room, GameWebSocketEvent.GameObjectState, new GameObjectStateMessageArgs
            {
                GameObject = GameProfiles.Map(unit),
                Immediately = false
            });
        }

        private async void OnUnitDied(object s, EventArgs args, string room)
        {
            var unit = s as Unit;
            await _webSocketMessageService.SendMessageToGroupAsync(room, GameWebSocketEvent.GameObjectState, new GameObjectStateMessageArgs
            {
                GameObject = GameProfiles.Map(unit),
                Immediately = true
            });
        }

        private async void OnUnitAbilityUsed(object s, AbilityUsedEventArgs args, string roomName)
        {
            var unit = s as Unit;
            var bulletSid = Guid.NewGuid().ToString();

            if (args.IsRanged)
            {
                var room = GetRoom(roomName);
                var bullet = new Bullet(bulletSid, unit.ScreenPositionCenter, unit.Target as Unit, 25);
                bullet.OnDamageDealt += (sBullet, e) => OnDamageDealt(sBullet, e, room);
                room.Dungeon.GameObjects.TryAdd(bulletSid, bullet);
            }

            await _webSocketMessageService.SendMessageToGroupAsync(roomName, GameWebSocketEvent.UseAbility, new UseAbilityMessageArgs
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

            var targetUnit = gameObject.Target as Unit;

            await _webSocketMessageService.SendMessageToGroupAsync(room.Name, GameWebSocketEvent.DealDamage, new DealDamageMessageArgs
            {
                TargetSid = args.Target.Sid,
                TargetHealth = targetUnit.Health,
                Damage = args.Damage
            });
        }

        public Room GetRoom(string name)
        {
            Room room;
            _rooms.TryGetValue(name, out room);
            return room;
        }

        public List<Room> GetRooms()
        {
            return _rooms.Select(it => it.Value).ToList();
        }

        public async Task RemoveRoom(string name)
        {
            Room room;
            _rooms.TryRemove(name, out room);
        }

        public async Task RemovePlayer(string roomName, long userId)
        {
            var room = GetRoom(roomName);
            if (room == null)
            {
                throw new RoomNotFoundException(roomName);
            }

            var sid = userId.ToString();
            var player = room.Players.FirstOrDefault(it => it.Sid == sid);

            if(player == null)
            {
                throw new PlayerNotFoundException(sid, roomName);
            }

            room.Players.Remove(player);

            if (!room.Players.Any())
            {
                await RemoveRoom(roomName);
            }
        }

        public void SetUnitTarget(string roomName, string sid, int x, int y)
        {
            var room = _rooms[roomName];
            var player = room.Players.SingleOrDefault(it => it.Sid == sid);

            if(player == null)
            {
                throw new PlayerNotFoundException(sid, roomName);
            }

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
