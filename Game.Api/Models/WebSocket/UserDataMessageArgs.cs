using System.Collections.Generic;

namespace Game.Api.Models.WebSocket
{
    public class UserDataMessageArgs : WebSocketMessageArgs
    {
        public string Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public IEnumerable<string> Enemies { get; set; }

        public IEnumerable<int> EnemiesX { get; set; }

        public IEnumerable<int> EnemiesY { get; set; }
    }
}
