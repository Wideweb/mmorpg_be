using Game.Api.Game.Services;

namespace Game.Api.Game.Models.Abilities
{
    public abstract class RangeAbility : Ability
    {
        public RangeAbility(Unit unit, long cooldown, int range)
            : base(unit, cooldown)
        {
            _isRanged = true;
            _range = range;
        }

        public override bool CanUse()
        {
            if (!base.CanUse())
            {
                return false;
            }

            var target = _unit.Target as Unit;

            if (target == null)
            {
                return false;
            }

            var distance = Utils.GetDistance(_unit.Position, target.Position);

            return distance <= _range;
        }
    }
}
