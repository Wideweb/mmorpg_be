using System;
using System.Collections.Generic;

namespace Game.Api.DataAccess.Models
{
    public class Room
    {
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Player> Players { get; set; }

        public Dungeon Dungeon { get; set; }
    }
}
