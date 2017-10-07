using Common.Api.WebSocketManager.Messages;
using Game.Api.Models;

namespace Game.Api.WebSocketManager.Messages
{
    public class SetTargetMessageArgs : WebSocketMessageArgs
    {
        public string Sid { get; set; }

        public Point Position { get; set; }
    }
}
