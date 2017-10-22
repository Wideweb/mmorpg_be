using Game.Api.Constants;

namespace Game.Api.Models.Abilities
{
    public class RangeAttack : RangeAbility
    {
        public RangeAttack(Unit unit, long cooldown, int range, long castTime)
            : base(unit, cooldown, range, castTime)
        {
            _abilityType = AbilityType.RangeAttack;
        }
    }
}
