using Game.Api.Models.WebSocket;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Api.WebSocketManager
{
    public abstract class WebSocketMessageService
    {
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }

        public WebSocketMessageService(WebSocketConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public async Task SendMessageAsync(WebSocket socket, string eventName, WebSocketMessageArgs eventArgs)
        {
            if (socket.State != WebSocketState.Open)
                return;

            var message = new WebSocketMessage
            {
                Event = eventName,
                Data = JsonConvert.SerializeObject(eventArgs)
            };

            var stringMessage = JsonConvert.SerializeObject(message);

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(stringMessage),
                                                                  offset: 0,
                                                                  count: stringMessage.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(string sid, string eventName, WebSocketMessageArgs eventArgs)
        {
            var socket = WebSocketConnectionManager.GetSocketById(sid);
            await SendMessageAsync(socket, eventName, eventArgs);
        }

        public async Task SendMessageToGroupAsync(string group, string eventName, WebSocketMessageArgs eventArgs)
        {
            var sockets = WebSocketConnectionManager.GetAll(group);
            var tasks = sockets.Select(s => SendMessageAsync(s.Value, eventName, eventArgs));
            await Task.WhenAll(tasks);
        }
    }
}
