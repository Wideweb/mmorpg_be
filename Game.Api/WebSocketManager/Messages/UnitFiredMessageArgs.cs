namespace Game.Api.WebSocketManager.Messages
{
    public class UnitFiredMessageArgs : WebSocketMessageArgs
    {
        public string Sid { get; set; }

        public string TargetSid { get; set; }
    }
}
