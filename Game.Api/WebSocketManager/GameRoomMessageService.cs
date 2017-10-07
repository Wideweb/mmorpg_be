using Common.Api.WebSocketManager;

namespace Game.Api.WebSocketManager
{
    public class GameRoomMessageService : WebSocketMessageService
    {
        public GameRoomMessageService(GameRoomConnectionManager webSocketConnectionManager) 
            : base(webSocketConnectionManager)
        {
        }
    }
}
