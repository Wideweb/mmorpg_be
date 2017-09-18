namespace Game.Api.Game.Models
{
    public abstract class GameObject : Target
    {
        public int Speed { get; set; }

        public virtual Point Position { get; set; }

        public virtual Target Target { get; set; }

        public virtual Point ScreenPosition { get; set; }

        public virtual void Update(long elapsed) { }
    }
}
