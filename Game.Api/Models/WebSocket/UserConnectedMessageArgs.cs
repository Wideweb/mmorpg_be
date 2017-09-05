namespace Game.Api.Models.WebSocket
{
    public class UserConnectedMessageArgs : WebSocketMessageArgs
    {
        public string Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
