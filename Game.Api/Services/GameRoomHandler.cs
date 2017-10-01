using Game.Api.Constants;
using Game.Api.Game.Profiles;
using Game.Api.Services.Exceptions;
using Game.Api.WebSocketManager;
using Game.Api.WebSocketManager.Messages;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Game.Api.Game.Services
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

            var userDataMessageArgs = new UserDataMessageArgs
            {
                Player = GameProfiles.Map(player),
                GameObjects = gameObjects.Select(it => GameProfiles.Map(it.Value))
            };
            await WebSocketMessageService.SendMessageAsync(sid, WebSocketEvent.UserData, userDataMessageArgs);

            var userConnectedMessageArgs = new UserConnectedMessageArgs
            {
                Player = GameProfiles.Map(player)
            };
            await WebSocketMessageService.SendMessageToGroupAsync(group, WebSocketEvent.UserConnected, userConnectedMessageArgs);
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
                case WebSocketEvent.SetTarget:
                    var setTargetArgs = (SetTargetMessageArgs)eventArgs;
                    _roomManager.SetUnitTarget(group, sid, setTargetArgs.Position.X, setTargetArgs.Position.Y);
                    break;
                default:
                    break;
            }
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var group = WebSocketConnectionManager.GetGroup(socket);

            if(group == null)
            {
                return;
            }

            var sid = WebSocketConnectionManager.GetId(socket, group);

            if (sid == null)
            {
                return;
            }

            await base.OnDisconnected(socket);

            var messageArgs = new UserDisconnectedMessageArgs
            {
                Sid = sid
            };
            await WebSocketMessageService.SendMessageToGroupAsync(group, WebSocketEvent.UserDisconnected, messageArgs);
        }
    }
}
