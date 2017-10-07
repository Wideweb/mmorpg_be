using Common.Api.WebSocketManager.Messages;

namespace Identity.Api.WebSocketManager.Messages
{
    public class PlayerLeftMessageArgs : WebSocketMessageArgs
    {
        public string Sid { get; set; }
    }
}
