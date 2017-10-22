using Game.Api.Services;

namespace Game.Api.Models.Abilities
{
    public abstract class RangeAbility : Ability
    {
        public RangeAbility(Unit unit, long cooldown, int range, long castTime)
            : base(unit, cooldown, castTime)
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
