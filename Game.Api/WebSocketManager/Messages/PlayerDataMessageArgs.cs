using Common.Api.WebSocketManager.Messages;
using Game.Api.Dto;
using System.Collections.Generic;

namespace Game.Api.WebSocketManager.Messages
{
    public class PlayerDataMessageArgs : WebSocketMessageArgs
    {
        public PlayerDto Player { get; set; }

        public IEnumerable<GameObjectDto> GameObjects { get; set; }
    }
}
