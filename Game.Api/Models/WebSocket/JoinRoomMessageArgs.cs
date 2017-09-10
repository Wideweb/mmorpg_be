namespace Game.Api.Models.WebSocket
{
    public class JoinRoomMessageArgs : WebSocketMessageArgs
    {
        public string Room { get; set; }
    }
}
