namespace Common.Api.WebSocketManager.Messages
{
    public class WebSocketMessage
    {
        public string Event { get; set; }

        public string Data { get; set; }

        public string Token { get; set; }
    }
}
