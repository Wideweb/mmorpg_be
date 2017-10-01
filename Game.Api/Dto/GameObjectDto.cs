using Game.Api.Constants;
using Game.Api.Game.Models;

namespace Game.Api.Dto
{
    public class GameObjectDto
    {
        public string Sid { get; set; }

        public int Width { get; set; }

        public Point Position { get; set; }

        public GameObjectType Type { get; set; }

        public string Target { get; set; }

        public Point TargetPosition { get; set; }

        public Point ScreenPosition { get; set; }
    }
}
