using System.Collections.Generic;

namespace Game.Api.Models.Game
{
    public class Dungeon
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int MaxPlayersNumber { get; set; }

        public List<MapCell> Map { get; set; }

        public List<Unit> Units { get; set; }
    }
}
