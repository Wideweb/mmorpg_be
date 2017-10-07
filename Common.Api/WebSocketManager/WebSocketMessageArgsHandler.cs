using Common.Api.WebSocketManager.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Common.Api.WebSocketManager
{
    public static class WebSocketMessageArgsHandler
    {
        private static readonly Dictionary<string, Type> map = new Dictionary<string, Type>
        {
            { WebSocketEvent.JoinGroup, typeof(JoinGroupMessageArgs) },
            { WebSocketEvent.Connected, typeof(ConnectedMessageArgs) },
            { WebSocketEvent.Disconnected, typeof(DisconnectedMessageArgs) }
        };

        public static WebSocketMessageArgs GetWebSocketArgs(string eventName, string args)
        {
            if (!map.ContainsKey(eventName))
            {
                throw new ArgumentException(eventName);
            }

            return (WebSocketMessageArgs)JsonConvert.DeserializeObject(args, map[eventName]);
        }

        public static void AddOrReplaceEvent<T>(string eventName) where T : WebSocketMessageArgs
        {
            if (map.ContainsKey(eventName))
            {
                var oldEventArgsType = map[eventName];
                var newEventArgsType = typeof(T);

                if (!newEventArgsType.IsSubclassOf(oldEventArgsType))
                {
                    throw new ArgumentException("can\'t replace type of event message args");
                }
                
                map[eventName] = typeof(T);
            }

            map.Add(eventName, typeof(T));
        }
    }
}
