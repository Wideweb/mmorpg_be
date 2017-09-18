using Game.Api.Game.Models;

namespace Game.Api.WebSocketManager.Messages
{
    public class SetTargetMessageArgs : WebSocketMessageArgs
    {
        public string Sid { get; set; }

        public Point Position { get; set; }
    }
}
