using Game.Api.Constants;
using System;

namespace Game.Api.Models.Abilities
{
    public abstract class Ability
    {
        protected Unit _unit;
        protected long _cooldown;
        protected long _castTime;
        protected long _timeAfterUseElapsed;
        protected bool _isReady;
        protected bool _isRanged;
        protected int _range;
        protected AbilityType _abilityType;

        public event EventHandler<AbilityUsedEventArgs> OnUsed;
        public event EventHandler<AbilityCastEventArgs> OnCast;

        public bool IsRanged => _isRanged;
        public int Range => _range;

        private bool _casting = false;
        private long _castTimeElapsed = 0;

        public Ability (Unit unit, long cooldown, long castTime)
        {
            _unit = unit;
            _cooldown = cooldown;
            _castTime = castTime;
        }

        public virtual void Update(long elapsed)
        {
            if (_casting)
            {
                _castTimeElapsed += elapsed;
                if (_castTimeElapsed > _castTime)
                {
                    CancelCast();
                    Use();
                }
                return;
            }

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
            return _isReady && !_casting;
        }

        public virtual void Cast()
        {
            if (!CanUse())
            {
                return;
            }

            _casting = true;

            OnCast?.Invoke(this, new AbilityCastEventArgs
            {
                AbilityType = _abilityType,
                CastTime = _castTime
            });
        }

        public virtual void CancelCast()
        {
            _casting = false;
            _castTimeElapsed = 0;
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
