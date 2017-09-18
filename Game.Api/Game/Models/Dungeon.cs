using System.Collections.Generic;

namespace Game.Api.Game.Models
{
    public class Dungeon
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int MaxPlayersNumber { get; set; }

        public MapCell[][] Map { get; set; }

        public int Height => Map.Length;

        public int Width => Map[0].Length;

        public List<Unit> Units { get; set; }

        public MapCell GetCell(int x, int y)
        {
            return Map[y][x];
        }

        public Point OriginPosition { get; set; }
    }
}
