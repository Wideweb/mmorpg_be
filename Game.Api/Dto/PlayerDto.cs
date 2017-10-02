using System;

namespace Game.Api.Dto
{
    public class PlayerDto
    {
        public string Sid { get; set; }

        public string Name { get; set; }

        public DateTime JoinedAt { get; set; }

        public GameObjectDto Unit { get; set; }
    }
}
