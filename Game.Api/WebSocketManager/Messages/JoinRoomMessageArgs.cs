namespace Game.Api.WebSocketManager.Messages
{
    public class JoinRoomMessageArgs : WebSocketMessageArgs
    {
        public string Room { get; set; }
    }
}
