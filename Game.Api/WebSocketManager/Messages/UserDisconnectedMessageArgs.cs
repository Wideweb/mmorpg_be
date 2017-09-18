namespace Game.Api.WebSocketManager.Messages
{
    public class UserDisconnectedMessageArgs : WebSocketMessageArgs
    {
        public string Sid { get; set; }
    }
}
