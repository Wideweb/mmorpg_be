using System;

namespace Game.Api.Game.Models
{
    public class Player
    {
        public string Sid { get; set; }

        public string Name { get; set; }

        public DateTime JoinedAt { get; set; }

        public Unit Unit { get; set; }
    }
}
