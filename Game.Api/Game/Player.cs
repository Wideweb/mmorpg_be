using System;

namespace Game.Api.Game
{
    public class Player
    {
        public string Sid { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
