using Common.Api.WebSocketManager.Messages;
using Game.Api.Dto;

namespace Game.Api.WebSocketManager.Messages
{
    public class PlayerConnectedMessageArgs : WebSocketMessageArgs
    {
        public PlayerDto Player { get; set; }
    }
}
