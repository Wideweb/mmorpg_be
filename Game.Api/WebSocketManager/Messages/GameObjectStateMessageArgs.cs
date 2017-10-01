using Game.Api.Dto;

namespace Game.Api.WebSocketManager.Messages
{
    public class GameObjectStateMessageArgs : WebSocketMessageArgs
    {
        public GameObjectDto GameObject { get; set; }
    }
}
