using Common.Api.WebSocketManager.Messages;

namespace Game.Api.WebSocketManager.Messages
{
    public class PlayerLeftMessageArgs : WebSocketMessageArgs
    {
        public string Sid { get; set; }
    }
}
