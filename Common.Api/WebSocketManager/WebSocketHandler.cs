using Common.Api.WebSocketManager.Messages;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Common.Api.WebSocketManager
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
            
            var connectedMessageArgs = new ConnectedMessageArgs
            {
                Sid = sid
            };
            await WebSocketMessageService.SendMessageToGroupAsync(group, WebSocketEvent.Connected, connectedMessageArgs);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            var group = WebSocketConnectionManager.GetGroup(socket);

            var sid = WebSocketConnectionManager.GetId(socket);

            if (sid == null)
            {
                return;
            }

            await WebSocketConnectionManager.RemoveSocket(socket);

            await OnSocketRemoved(sid, group);

            var messageArgs = new DisconnectedMessageArgs
            {
                Sid = sid
            };
            await WebSocketMessageService.SendMessageToGroupAsync(group, WebSocketEvent.Disconnected, messageArgs);
        }

        protected virtual Task OnSocketRemoved(string sid, string group)
        {
            return Task.FromResult(0);
        }

        public abstract Task ReceiveAsync(WebSocket socket, string userId, WebSocketReceiveResult result, string eventName, WebSocketMessageArgs eventArgs);
    }
}
