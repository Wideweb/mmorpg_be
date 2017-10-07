namespace Game.Api.Constants
{
    public static class GameWebSocketEvent
    {
        public const string PlayerConnected = "PLAYER_CONNECTED";
        public const string PlayerData = "PLAYER_DATA";
        public const string GameObjectState = "GAME_OBJECT_STATE";
        public const string SetTarget = "SET_TARGET";
        public const string UseAbility = "USE_ABILITY";
        public const string DealDamage = "DEAL_DAMAGE";

        public const string RoomAdded = "ROOM_ADDED";
        public const string RoomRemoved = "ROME_REMOVED";
        public const string PlayerJoined = "PLAYER_JOINED";
        public const string PlayerLeft = "PLAYER_LEFT";
    }
}
