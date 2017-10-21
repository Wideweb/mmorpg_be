using Common.Api.WebSocketManager.Messages;

namespace Identity.Api.WebSocketManager.Messages
{
    public class CharacterChosenMessageArgs : WebSocketMessageArgs
    {
        public string PlayerSid { get; set; }

        public long CharacterId { get; set; }

        public string CharacterName { get; set; }
    }
}
