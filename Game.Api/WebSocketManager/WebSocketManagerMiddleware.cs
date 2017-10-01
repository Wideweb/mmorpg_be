using Common.Api.Auth;
using Common.Api.Exceptions;
using Game.Api.Constants;
using Game.Api.WebSocketManager.Messages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Api.WebSocketManager
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandler _webSocketHandler { get; set; }
        private CustomJwtDataFormat _customJwtDataFormat { get; set; }

        public WebSocketManagerMiddleware(RequestDelegate next,
                                          WebSocketHandler webSocketHandler,
                                          CustomJwtDataFormat customJwtDataFormat)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
            _customJwtDataFormat = customJwtDataFormat;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var eventString = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var eventMessage = JsonConvert.DeserializeObject<WebSocketMessage>(eventString);

                    var tiket = _customJwtDataFormat.Unprotect(eventMessage.Token);
                    if (tiket == null)
                    {
                        return;
                    }

                    context.User = tiket.Principal;
                    var sid = context.UserSid();

                    var args = WebSocketMessageArgsHandler.GetWebSocketArgs(eventMessage.Event, eventMessage.Data);

                    if (eventMessage.Event == WebSocketEvent.JoinRoom)
                    {
                        await _webSocketHandler.OnConnected(socket, ((JoinRoomMessageArgs)args).Room, sid);
                    }
                    else
                    {
                        await _webSocketHandler.ReceiveAsync(socket, sid, result, eventMessage.Event, args);
                    }

                    return;
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocketHandler.OnDisconnected(socket);
                    return;
                }
            });

            //TODO - investigate the Kestrel exception thrown when this is the last middleware
            //await _next.Invoke(context);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                       cancellationToken: CancellationToken.None);

                try
                {
                    handleMessage(result, buffer);
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
