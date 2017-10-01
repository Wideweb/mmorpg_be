using Game.Api.WebSocketManager;

namespace Game.Api.Game.Services
{
    public class GameRoomMessageService : WebSocketMessageService
    {
        public GameRoomMessageService(GameRoomConnectionManager webSocketConnectionManager) 
            : base(webSocketConnectionManager)
        {
        }
    }
}
