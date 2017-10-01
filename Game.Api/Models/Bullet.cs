using Game.Api.Constants;
using System;

namespace Game.Api.Game.Models
{
    public class Bullet : GameObject
    {
        public override GameObjectType Type => GameObjectType.Bullet;

        public event EventHandler<DamageDealtEventArgs> OnDamageDealt;

        private bool _isTargetRiched = false;
        private int _damage;

        public Bullet(string sid, Point screenPosition, Unit target, int damage)
        {
            Speed = 5;
            ScreenPosition = screenPosition;
            Target = target;
            _damage = damage;
            Sid = sid;
        }

        public override void Update(long elapsed)
        {
            Run(elapsed);
        }

        private void Run(long elapsed)
        {
            if (_isTargetRiched)
            {
                return;
            }

            var target = Target as Unit;

            int deltaX = target.ScreenPositionCenter.X - ScreenPosition.X;
            int deltaY = target.ScreenPositionCenter.X - ScreenPosition.Y;

            var distace = Math.Abs(deltaX) + Math.Abs(deltaY);
            var speed = Speed * elapsed / 10;

            if (distace > speed)
            {
                var angle = Math.Atan2(deltaY, deltaX);

                deltaX = (int)(Math.Cos(angle) * speed);
                deltaY = (int)(Math.Sin(angle) * speed);

                target.TakeDamage(_damage);

                _isTargetRiched = true;

                OnDamageDealt?.Invoke(this, new DamageDealtEventArgs
                {
                    Target = target,
                    Damage = _damage
                });
            }

            ScreenPosition.X += deltaX;
            ScreenPosition.Y += deltaY;
        }
    }
}
