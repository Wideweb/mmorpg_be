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
    public abstract class WebSocketHandler
    {
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            WebSocketConnectionManager.AddSocket(socket);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
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

        public async Task SendMessageAsync(string socketId, string eventName, WebSocketMessageArgs eventArgs)
        {
            await SendMessageAsync(WebSocketConnectionManager.GetSocketById(socketId), eventName, eventArgs);
        }

        public async Task SendMessageToAllAsync(string eventName, WebSocketMessageArgs eventArgs)
        {
            await SendMessageToAsync(eventName, eventArgs, it => true);
        }

        public async Task SendMessageToAsync(string eventName, WebSocketMessageArgs eventArgs, Predicate<string> filter)
        {
            var sockets = WebSocketConnectionManager
                .GetAll()
                .Where(it => filter(it.Key));

            foreach (var pair in sockets)
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, eventName, eventArgs);
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, string eventName, WebSocketMessageArgs eventArgs);
    }
}
