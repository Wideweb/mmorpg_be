using Game.Api.DataAccess;
using Game.Api.Game;
using System.Collections.Generic;

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
                    map[y][x] = new MapCell
                    {
                        X = x,
                        Y = y,
                        Type = dbCell.Type
                    };
                }
            }

            var dungeon = new Dungeon
            {
                Id = dbDungeon.Id,
                Name = dbDungeon.Name,
                MaxPlayersNumber = dbDungeon.MaxPlayersNumber,
                Map = map
            };

            var units = new List<Unit>();

            foreach(var dbUnit in dbDungeon.Units)
            {
                var unit = new Unit(new Point { X = dbUnit.X, Y = dbUnit.Y }, dungeon);
            }

            dungeon.Units = units;

            return dungeon;
        }
    }
}
