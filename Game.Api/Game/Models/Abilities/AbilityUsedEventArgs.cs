using Game.Api.Game.Constants;
using System;

namespace Game.Api.Game.Models.Abilities
{
    public class AbilityUsedEventArgs : EventArgs
    {
        public AbilityType AbilityType { get; set; }

        public bool IsRanged { get; set; }
    }
}
