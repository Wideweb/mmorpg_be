using Game.Api.Game.Dto;

namespace Game.Api.WebSocketManager.Messages
{
    public class UserConnectedMessageArgs : WebSocketMessageArgs
    {
        public PlayerDto Player { get; set; }
    }
}
