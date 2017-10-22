using Common.Api.WebSocketManager.Messages;
using Game.Api.Constants;

namespace Game.Api.WebSocketManager.Messages
{
    public class CastAbilityMessageArgs : WebSocketMessageArgs
    {
        public string Sid { get; set; }

        public AbilityType AbilityType { get; set; }

        public long CastTime { get; set; }
    }
}
