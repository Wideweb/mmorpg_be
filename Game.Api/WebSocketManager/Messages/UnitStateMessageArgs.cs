using Game.Api.Game.Models;

namespace Game.Api.WebSocketManager.Messages
{
    public class UnitStateMessageArgs : WebSocketMessageArgs
    {
        public string Sid { get; set; }

        public Point Position { get; set; }
    }
}
