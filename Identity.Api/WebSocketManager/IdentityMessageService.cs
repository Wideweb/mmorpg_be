using Common.Api.WebSocketManager;

namespace Identity.Api.WebSocketManager
{
    public class IdentityMessageService : WebSocketMessageService
    {
        public IdentityMessageService(IdentityConnectionManager webSocketConnectionManager) 
            : base(webSocketConnectionManager)
        {
        }
    }
}
