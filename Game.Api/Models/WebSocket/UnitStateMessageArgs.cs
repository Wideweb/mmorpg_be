using System;

namespace Game.Api.Models.WebSocket
{
    public class UnitStateMessageArgs : WebSocketMessageArgs
    {
        public Guid Id { get; set; }

        public long X { get; set; }

        public long Y { get; set; }
    }
}
