using Game.Api.Constants;
using System;

namespace Game.Api.Game.Models.Abilities
{
    public abstract class Ability
    {
        protected Unit _unit;
        protected long _cooldown;
        protected long _timeAfterUseElapsed;
        protected bool _isReady;
        protected bool _isRanged;
        protected int _range;
        protected AbilityType _abilityType;

        public event EventHandler<AbilityUsedEventArgs> OnUsed;

        public bool IsRanged => _isRanged;
        public int Range => _range;

        public Ability (Unit unit, long cooldown)
        {
            _unit = unit;
            _cooldown = cooldown;
        }

        public virtual void Update(long elapsed)
        {
            if (_isReady)
            {
                return;
            }

            _timeAfterUseElapsed += elapsed;
            if (_timeAfterUseElapsed < _cooldown)
            {
                return;
            }

            _isReady = true;
        }

        public virtual bool CanUse()
        {
            return _isReady;
        }

        public virtual void Use()
        {
            if (!CanUse())
            {
                return;
            }

            OnUsed?.Invoke(this, new AbilityUsedEventArgs
            {
                AbilityType = _abilityType,
                IsRanged = _isRanged
            });

            _isReady = false;
            _timeAfterUseElapsed = 0;
        }
    }
}
