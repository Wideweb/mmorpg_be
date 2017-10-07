using Common.Api.WebSocketManager;
using Common.Api.WebSocketManager.Messages;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Identity.Api.Services;
using Identity.Api.Services.Exceptions;
using System.Linq;

namespace Identity.Api.WebSocketManager
{
    public class IdentityHandler : WebSocketHandler
    {
        private readonly RoomManager _roomManager;

        public IdentityHandler(IdentityConnectionManager webSocketConnectionManager,
            IdentityMessageService webSocketMessageService, 
            RoomManager roomManager) : base(webSocketConnectionManager, webSocketMessageService)
        {
            _roomManager = roomManager;
        }

        public override async Task OnConnected(WebSocket socket, string group, string sid)
        {
            await base.OnConnected(socket, group, sid);
        }

        public override async Task ReceiveAsync(WebSocket socket, string sid, WebSocketReceiveResult result, string eventName, WebSocketMessageArgs eventArgs)
        {
            var group = WebSocketConnectionManager.GetGroup(socket);
            if(group == null)
            {
                throw new RoomNotFoundException(group);
            }
        }

        protected override async Task OnSocketRemoved(string sid, string group)
        {
            await _roomManager.RemovePlayer(group, sid);
        }
    }
}
