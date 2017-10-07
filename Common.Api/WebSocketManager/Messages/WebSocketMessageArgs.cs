using System;

namespace Common.Api.WebSocketManager.Messages
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
