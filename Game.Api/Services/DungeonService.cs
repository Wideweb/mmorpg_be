using Game.Api.DataAccess;
using Game.Api.Game;
using Game.Api.Game.Models;
using System.Collections.Generic;
using System.Linq;

namespace Game.Api.Services
{
    public class DungeonService : IDungeonService
    {
        private readonly DungeonRepository _dungeonRepository;

        public DungeonService(DungeonRepository dungeonRepository)
        {
            _dungeonRepository = dungeonRepository;
        }

        public Dungeon GetById(long dungeonType)
        {
            var dbDungeon = _dungeonRepository.GetById(dungeonType);

            var map = new MapCell[dbDungeon.Height][];

            for (var y = 0; y < dbDungeon.Height; y++)
            {
                map[y] = new MapCell[dbDungeon.Width];
                for (var x = 0; x < dbDungeon.Width; x++)
                {
                    var dbCell = dbDungeon.Map[y * dbDungeon.Width + x];
                    map[y][x] = new MapCell(x, y, dbCell.Type);
                }
            }

            var dungeon = new Dungeon
            {
                Id = dbDungeon.Id,
                Name = dbDungeon.Name,
                MaxPlayersNumber = dbDungeon.MaxPlayersNumber,
                Map = map,
                OriginPosition = new Point { X = dbDungeon.OriginPositionX, Y = dbDungeon.OriginPositionY }
            };
            
            foreach(var dbUnit in dbDungeon.Units)
            {
                var sid = dbUnit.Id.ToString();
                dungeon.GameObjects.TryAdd(sid, new Unit(new Point { X = dbUnit.X, Y = dbUnit.Y }, dungeon, sid, true));
            }

            return dungeon;
        }
    }
}
