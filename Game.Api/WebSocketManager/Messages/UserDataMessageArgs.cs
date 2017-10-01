using Game.Api.Dto;
using System.Collections.Generic;

namespace Game.Api.WebSocketManager.Messages
{
    public class UserDataMessageArgs : WebSocketMessageArgs
    {
        public PlayerDto Player { get; set; }

        public IEnumerable<GameObjectDto> GameObjects { get; set; }
    }
}
