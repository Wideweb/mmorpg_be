using System;

namespace Game.Api.Models.Game
{
    public class Unit
    {
        public long Id { get; set; }

        public long UnitType { get; set; }

        public long Health { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
