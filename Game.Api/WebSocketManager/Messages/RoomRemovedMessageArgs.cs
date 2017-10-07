using Common.Api.WebSocketManager.Messages;
using Game.Api.Dto;

namespace Game.Api.WebSocketManager.Messages
{
    public class RoomRemovedMessageArgs : WebSocketMessageArgs
    {
        public string RoomName { get; set; }
    }
}
