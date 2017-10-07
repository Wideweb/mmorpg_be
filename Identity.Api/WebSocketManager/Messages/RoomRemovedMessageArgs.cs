using Common.Api.WebSocketManager.Messages;

namespace Identity.Api.WebSocketManager.Messages
{
    public class RoomRemovedMessageArgs : WebSocketMessageArgs
    {
        public string RoomName { get; set; }
    }
}
