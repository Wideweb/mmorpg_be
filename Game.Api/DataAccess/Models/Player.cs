using System;

namespace Game.Api.DataAccess.Models
{
    public class Player
    {
        public Guid Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public UserCharacter UserCharacter { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
