using Game.Api.Constants;

namespace Game.Api.Models
{
    public abstract class GameObject : Target
    {
        public string Sid { get; set; }

        public string Name { get; set; }

        public long Health { get; set; }

        public long MaxHealth { get; set; }

        public int Width { get; set; }

        public int Speed { get; set; }

        public abstract GameObjectType Type { get; }

        public virtual Point Position { get; set; }

        public virtual Target Target { get; set; }

        public virtual Point ScreenPosition { get; set; }

        public virtual void Update(long elapsed) { }

        public string SpriteFileName { get; set; }
    }
}
