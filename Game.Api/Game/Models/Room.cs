using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game.Api.Game.Models
{
    public class Room
    {
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Player> Players { get; set; }

        public Dungeon Dungeon { get; set; }

        public Stopwatch Clock { get; set; }

        public Room()
        {
            Players = new List<Player>();
        }
    }
}
