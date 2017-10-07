using Clients.GameClient.Dto;
using Common.Api.WebSocketManager.Messages;

namespace Identity.Api.WebSocketManager.Messages
{
    public class PlayerJoinedMessageArgs : WebSocketMessageArgs
    {
        public CreatePlayerDto Player { get; set; }
    }
}
