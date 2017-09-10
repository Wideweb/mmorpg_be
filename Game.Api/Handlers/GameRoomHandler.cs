using Game.Api.Constants;
using Game.Api.Models.WebSocket;
using Game.Api.Services;
using Game.Api.WebSocketManager;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Game.Api.Handlers
{
    public class GameRoomHandler : WebSocketHandler
    {
        private readonly RoomManager _roomManager;

        public GameRoomHandler(WebSocketConnectionManager webSocketConnectionManager, 
            WebSocketMessageService webSocketMessageService, 
            RoomManager roomManager) : base(webSocketConnectionManager, webSocketMessageService)
        {
            _roomManager = roomManager;
        }

        public override async Task OnConnected(WebSocket socket, string group, string sid)
        {
            await base.OnConnected(socket, group, sid);

            var players = _roomManager.GetPlayers(group);
            var player = players.SingleOrDefault(it => it.Sid == sid);
            var units = _roomManager.GetUnits(group);

            var userDataMessageArgs = new UserDataMessageArgs
            {
                Id = sid,
                X = player.X,
                Y = player.Y,
                Enemies = units.Select(it => it.Sid.ToString()),
                EnemiesX = units.Select(it => it.Position.X),
                EnemiesY = units.Select(it => it.Position.Y)
            };
            await WebSocketMessageService.SendMessageAsync(sid, WebSocketEvent.UserData, userDataMessageArgs);

            var userConnectedMessageArgs = new UserConnectedMessageArgs
            {
                Id = sid,
                X = player.X,
                Y = player.Y
            };
            await WebSocketMessageService.SendMessageToGroupAsync(group, WebSocketEvent.UserConnected, userConnectedMessageArgs);
        }

        public override async Task ReceiveAsync(WebSocket socket, string group, string sid, WebSocketReceiveResult result, string eventName, WebSocketMessageArgs eventArgs)
        {
            var socketId = WebSocketConnectionManager.GetId(socket, group);

            switch (eventName)
            {
                case WebSocketEvent.UnitState:
                    var args = (UnitStateMessageArgs)eventArgs;
                    _roomManager.UpdatePlayer(group, sid, args.X, args.Y);
                    await WebSocketMessageService.SendMessageToGroupAsync(group, eventName, eventArgs);
                    break;
                default:
                    break;
            }
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var group = WebSocketConnectionManager.GetGroup(socket);
            var sid = WebSocketConnectionManager.GetId(socket, group);

            await base.OnDisconnected(socket);

            var messageArgs = new UserDisconnectedMessageArgs
            {
                Id = sid
            };
            await WebSocketMessageService.SendMessageToGroupAsync(group, WebSocketEvent.UserDisconnected, messageArgs);
        }
    }
}
