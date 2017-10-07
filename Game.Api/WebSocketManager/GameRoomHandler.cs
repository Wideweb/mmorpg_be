using Common.Api.WebSocketManager;
using Common.Api.WebSocketManager.Messages;
using Game.Api.Constants;
using Game.Api.Profiles;
using Game.Api.Services;
using Game.Api.Services.Exceptions;
using Game.Api.WebSocketManager.Messages;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Game.Api.WebSocketManager
{
    public class GameRoomHandler : WebSocketHandler
    {
        private readonly RoomManager _roomManager;

        public GameRoomHandler(GameRoomConnectionManager webSocketConnectionManager,
            GameRoomMessageService webSocketMessageService, 
            RoomManager roomManager) : base(webSocketConnectionManager, webSocketMessageService)
        {
            _roomManager = roomManager;
        }

        public override async Task OnConnected(WebSocket socket, string group, string sid)
        {
            await base.OnConnected(socket, group, sid);

            var room = _roomManager.GetRoom(group);
            if(room == null)
            {
                throw new RoomNotFoundException(group);
            }

            var player = room.Players.SingleOrDefault(it => it.Sid == sid);
            if (player == null)
            {
                throw new PlayerNotFoundException(sid, group);
            }

            var gameObjects = room.Dungeon.GameObjects;

            var userDataMessageArgs = new PlayerDataMessageArgs
            {
                Player = GameProfiles.Map(player),
                GameObjects = gameObjects.Select(it => GameProfiles.Map(it.Value))
            };
            await WebSocketMessageService.SendMessageAsync(sid, GameWebSocketEvent.PlayerData, userDataMessageArgs);

            var userConnectedMessageArgs = new PlayerConnectedMessageArgs
            {
                Player = GameProfiles.Map(player)
            };
            await WebSocketMessageService.SendMessageToGroupAsync(group, GameWebSocketEvent.PlayerConnected, userConnectedMessageArgs);
        }

        public override async Task ReceiveAsync(WebSocket socket, string sid, WebSocketReceiveResult result, string eventName, WebSocketMessageArgs eventArgs)
        {
            var group = WebSocketConnectionManager.GetGroup(socket);
            if(group == null)
            {
                throw new RoomNotFoundException(group);
            }

            switch (eventName)
            {
                case GameWebSocketEvent.SetTarget:
                    var setTargetArgs = (SetTargetMessageArgs)eventArgs;
                    _roomManager.SetUnitTarget(group, sid, setTargetArgs.Position.X, setTargetArgs.Position.Y);
                    break;
                default:
                    break;
            }
        }
    }
}
