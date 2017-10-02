namespace Game.Api.WebSocketManager.Messages
{
    public class DealDamageMessageArgs : WebSocketMessageArgs
    {
        public string TargetSid { get; set; }

        public int Damage { get; set; }

        public long TargetHealth { get; set; }
    }
}
