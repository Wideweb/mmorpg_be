using Clients.GameClient.Dto;
using Common.Api.WebSocketManager.Messages;

namespace Identity.Api.WebSocketManager.Messages
{
    public class RoomAddedMessageArgs : WebSocketMessageArgs
    {
        public CreateGameDto Room { get; set; }
    }
}
