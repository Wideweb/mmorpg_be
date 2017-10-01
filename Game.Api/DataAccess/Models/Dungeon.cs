using System.Collections.Generic;

namespace Game.Api.DataAccess.Models
{
    public class Dungeon
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int MaxPlayersNumber { get; set; }

        public List<MapCell> Map { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public List<Unit> Units { get; set; }

        public int OriginPositionX { get; set; }

        public int OriginPositionY { get; set; }
    }
}
