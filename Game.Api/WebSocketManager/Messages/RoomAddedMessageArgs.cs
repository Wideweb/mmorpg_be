using Common.Api.WebSocketManager.Messages;
using Game.Api.Dto;

namespace Game.Api.WebSocketManager.Messages
{
    public class RoomAddedMessageArgs : WebSocketMessageArgs
    {
        public RoomDto Room { get; set; }
    }
}
