using System;

namespace Game.Api.Models.WebSocket
{
    public class UnitStateMessageArgs : WebSocketMessageArgs
    {
        public Guid Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
