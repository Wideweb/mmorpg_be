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
        protected WebSocketMessageService WebSocketMessageService { get; set; }

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager, WebSocketMessageService webSocketMessageService)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
            WebSocketMessageService = webSocketMessageService;
        }

        public virtual async Task OnConnected(WebSocket socket, string group, string sid)
        {
            WebSocketConnectionManager.AddSocket(socket, group, sid);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await WebSocketConnectionManager.RemoveSocket(socket);
        }

        public abstract Task ReceiveAsync(WebSocket socket, string group, string userId, WebSocketReceiveResult result, string eventName, WebSocketMessageArgs eventArgs);
    }
}
