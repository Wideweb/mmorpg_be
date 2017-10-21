using System;

namespace Clients.GameClient.Dto
{
    public class CreatePlayerDto
    {
        public string Sid { get; set; }

        public string Name { get; set; }

        public long? CharacterId { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
