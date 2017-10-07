using System.Collections.Generic;

namespace Clients.GameClient.Dto
{
    public class CreateGameDto
    {
        public string Name { get; set; }

        public long DungeonType { get; set; }

        public bool IsStarted { get; set; }

        public List<CreatePlayerDto> Players { get; set; }

        public CreateGameDto()
        {
            Players = new List<CreatePlayerDto>();
        }
    }
}
