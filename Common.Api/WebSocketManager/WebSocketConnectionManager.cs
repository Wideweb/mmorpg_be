using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Api.WebSocketManager
{
    public abstract class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>> _sockets;

        public WebSocketConnectionManager()
        {
            _sockets = new ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>>();
        }

        public WebSocket GetSocketById(string id)
        {
            return _sockets.SelectMany(it => it.Value).FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll(string group)
        {
            ConcurrentDictionary<string, WebSocket> sockets;
            _sockets.TryGetValue(group, out sockets);
            return sockets;
        }

        public string GetId(WebSocket socket)
        {
            var sid = _sockets.SelectMany(it => it.Value).FirstOrDefault(it => it.Value == socket).Key;
            return sid;
        }

        public string GetId(WebSocket socket, string group)
        {
            var groupSockets = _sockets[group];
            var sid = groupSockets.SingleOrDefault(it => it.Value == socket).Key;

            return sid;
        }

        public string GetGroup(WebSocket socket)
        {
            return _sockets.FirstOrDefault(it => it.Value.Any(s => s.Value == socket)).Key;
        }

        public void AddSocket(WebSocket socket, string group, string id)
        {
            var socketsGroup = _sockets.GetOrAdd(group, new ConcurrentDictionary<string, WebSocket>());
            socketsGroup.TryAdd(id, socket);
        }

        public void JoinGroup(string group, string id)
        {
            var socket = GetSocketById(id);
            if(socket == null)
            {
                throw new ArgumentException(nameof(id));
            }
            AddSocket(socket, group, id);
        }

        public async Task RemoveSocket(WebSocket socket)
        {
            var group = GetGroup(socket);
            if(group == null)
            {
                return;
            }

            var groupSockets = _sockets[group];
            var sid = groupSockets.SingleOrDefault(it => it.Value == socket).Key;

            groupSockets.TryRemove(sid, out socket);

            if (socket != null)
            {
                await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                        statusDescription: "Closed by the WebSocketManager",
                                        cancellationToken: CancellationToken.None);
            }
        }

        public void RemoveFromGroup(string group, string sid)
        {
            WebSocket socket;
            var groupSockets = _sockets[group];
            groupSockets.TryRemove(sid, out socket);
            if(groupSockets.Count == 0)
            {
                ConcurrentDictionary<string, WebSocket> groupToDelete;
                _sockets.TryRemove(group, out groupToDelete);
            }
        }
    }
}
