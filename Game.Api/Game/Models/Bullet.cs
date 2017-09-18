using System;

namespace Game.Api.Game.Models
{
    public class Bullet : GameObject
    {
        public Bullet()
        {
            Speed = 5;
        }

        public override void Update(long elapsed)
        {
            Run(elapsed);
        }

        private void Run(long elapsed)
        {
            int deltaX = (Target as Unit).ScreenPositionCenter.X - ScreenPosition.X;
            int deltaY = (Target as Unit).ScreenPositionCenter.X - ScreenPosition.Y;

            var distace = Math.Abs(deltaX) + Math.Abs(deltaY);
            var speed = Speed * elapsed / 10;

            if (distace > speed)
            {
                var angle = Math.Atan2(deltaY, deltaX);

                deltaX = (int)(Math.Cos(angle) * speed);
                deltaY = (int)(Math.Sin(angle) * speed);
            }

            ScreenPosition.X += deltaX;
            ScreenPosition.Y += deltaY;
        }
    }
}
