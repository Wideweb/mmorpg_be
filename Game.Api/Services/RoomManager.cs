using Game.Api.DataAccess;
using Game.Api.Models.Game;
using Game.Api.Services.Utils;
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Threading;

namespace Game.Api.Services
{
    public class RoomManager
    {
        private readonly TimeSpan _updateRoomDelay = TimeSpan.FromSeconds(5);

        private readonly DungeonRepository _dungeonRepository;

        private ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();

        public RoomManager(DungeonRepository dungeonRepository)
        {
            _dungeonRepository = dungeonRepository;
        }

        public void CreateRoom(string name, long dungeonType)
        {
            var dungeon = _dungeonRepository.GetById(dungeonType);
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
                .Subscribe(x => UpdateRoom(name, cancelSource), cancelSource.Token);
        }

        public void RemoveRoom(string name)
        {
            Room room;
            _rooms.TryRemove(name, out room);
        }

        private void UpdateRoom(string name, CancellationTokenSource cancelSource)
        {
            if (!_rooms.ContainsKey(name))
            {
                cancelSource.Cancel();
                return;
            }

            var room = _rooms[name];
        }
    }
}
