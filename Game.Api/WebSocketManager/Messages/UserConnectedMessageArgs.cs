using Game.Api.Dto;

namespace Game.Api.WebSocketManager.Messages
{
    public class UserConnectedMessageArgs : WebSocketMessageArgs
    {
        public PlayerDto Player { get; set; }
    }
}
