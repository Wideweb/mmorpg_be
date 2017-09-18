using System;

namespace Game.Api.Game.Dto
{
    public class PlayerDto
    {
        public string Sid { get; set; }

        public DateTime JoinedAt { get; set; }

        public UnitDto Unit { get; set; }
    }
}
