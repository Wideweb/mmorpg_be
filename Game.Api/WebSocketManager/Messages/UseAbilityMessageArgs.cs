using Common.Api.WebSocketManager.Messages;
using Game.Api.Constants;

namespace Game.Api.WebSocketManager.Messages
{
    public class UseAbilityMessageArgs : WebSocketMessageArgs
    {
        public string Sid { get; set; }

        public string TargetSid { get; set; }

        public string BulletSid { get; set; }

        public AbilityType AbilityType { get; set; }
    }
}
