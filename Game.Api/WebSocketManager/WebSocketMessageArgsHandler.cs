using Game.Api.Constants;
using Game.Api.Models.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Game.Api.WebSocketManager
{
    public static class WebSocketMessageArgsHandler
    {
        private static readonly Dictionary<string, Type> map = new Dictionary<string, Type>
        {
            { WebSocketEvent.UnitState, typeof(UnitStateMessageArgs) },
            { WebSocketEvent.UserConnected, typeof(UserConnectedMessageArgs) },
            { WebSocketEvent.UserData, typeof(UserDataMessageArgs) },
            { WebSocketEvent.UserDisconnected, typeof(UserDisconnectedMessageArgs) },
            { WebSocketEvent.JoinRoom, typeof(JoinRoomMessageArgs) }
        };

        public static WebSocketMessageArgs GetWebSocketArgs(string eventName, string args)
        {
            if (!map.ContainsKey(eventName))
            {
                throw new ArgumentException(eventName);
            }

            return (WebSocketMessageArgs)JsonConvert.DeserializeObject(args, map[eventName]);
        }
    }
}
