using Game.Api.Constants;
using Game.Api.Models.WebSocket;
using Game.Api.WebSocketManager;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Game.Api.Handlers
{
    public class GameRoomHandler : WebSocketHandler
    {
        public GameRoomHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);
            var otherSocketIds = WebSocketConnectionManager
                .GetAll()
                .Where(it => it.Key != socketId)
                .Select(it => it.Key);

            var userDataMessageArgs = new UserDataMessageArgs
            {
                Id = socketId,
                X = 1,
                Y = 1,
                Enemies = otherSocketIds,
                EnemiesX = otherSocketIds.Select(it => 1),
                EnemiesY = otherSocketIds.Select(it => 1)
            };
            await SendMessageAsync(socketId, WebSocketEvent.UserData, userDataMessageArgs);

            var userConnectedMessageArgs = new UserConnectedMessageArgs
            {
                Id = socketId,
                X = 1,
                Y = 1
            };
            await SendMessageToAsync(WebSocketEvent.UserConnected, userConnectedMessageArgs, it => it != socketId);
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string eventName, WebSocketMessageArgs eventArgs)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);

            switch (eventName)
            {
                case WebSocketEvent.UnitState:
                    await SendMessageToAllAsync(eventName, eventArgs);
                    break;
                default:
                    break;
            }
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            await base.OnDisconnected(socket);

            var messageArgs = new UserDisconnectedMessageArgs
            {
                Id = socketId
            };
            await SendMessageToAllAsync(WebSocketEvent.UserDisconnected, messageArgs);
        }

    }
}
