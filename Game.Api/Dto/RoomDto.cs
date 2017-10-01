using System.Collections.Generic;

namespace Game.Api.Dto
{
    public class RoomDto
    {
        public string Name { get; set; }

        public bool IsStarted { get; set; }

        public IEnumerable<PlayerDto> Players { get; set; }
    }
}
