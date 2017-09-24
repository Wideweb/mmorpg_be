using Game.Api.Game.Constants;

namespace Game.Api.Game.Models.Abilities
{
    public class RangeAttack : RangeAbility
    {
        public RangeAttack(Unit unit, long cooldown, int range)
            : base(unit, cooldown, range)
        {
            _abilityType = AbilityType.RangeAttack;
        }
    }
}
