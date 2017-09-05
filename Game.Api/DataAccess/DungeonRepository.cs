using Game.Api.Models.Game;
using Game.Api.Services.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Game.Api.DataAccess
{
    public class DungeonRepository
    {
        private static int[][] map = new int[][] 
        {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
            new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        private static List<Dungeon> dungeons;

        public DungeonRepository()
        {
            dungeons = new List<Dungeon>();

            var dungeonMap = new List<MapCell>();

            for(var y = 0; y < map.Length; y++)
            {
                for(var x = 0; x < map[y].Length; x++)
                {
                    dungeonMap.Add(new MapCell
                    {
                        X = x,
                        Y = y,
                        Type = map[y][x]
                    });
                }
            }

            var dungeonUnits = new List<Unit>();
            dungeonUnits.Add(new Unit
            {
                Id = 1,
                Health = 100,
                UnitType = 1,
                X = 5,
                Y = 5
            });

            var dungeon = new Dungeon
            {
                Id = 1,
                Name = "Test",
                MaxPlayersNumber = 2,
                Map = dungeonMap,
                Units = dungeonUnits
            };

            dungeons.Add(dungeon);
        }

        public Dungeon GetById(long id)
        {
            return dungeons.FirstOrDefault(it => it.Id == id)?.Clone();
        }

        public Dungeon GetByName(string name)
        {
            return dungeons.FirstOrDefault(it => it.Name == name)?.Clone();
        }
    }
}
