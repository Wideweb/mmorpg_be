namespace Game.Api.Constants
{
    public static class WebSocketEvent
    {
        public const string JoinRoom = "JOIN_ROOM";
        public const string UserConnected = "USER_CONNECTED";
        public const string UserData = "USER_DATA";
        public const string UserDisconnected = "USER_DISCONNECTED";
        public const string GameObjectState = "GAME_OBJECT_STATE";
        public const string SetTarget = "SET_TARGET";
        public const string UseAbility = "USE_ABILITY";
        public const string DealDamage = "DEAL_DAMAGE";
    }
}
