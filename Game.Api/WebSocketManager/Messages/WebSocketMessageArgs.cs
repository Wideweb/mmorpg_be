using System;

namespace Game.Api.WebSocketManager.Messages
{
    public class WebSocketMessageArgs
    {
        public DateTime CreatedAt { get; set; }

        public WebSocketMessageArgs()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
