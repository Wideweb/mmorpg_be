using Game.Api.Constants;
using System;

namespace Game.Api.Models.Abilities
{
    public class AbilityUsedEventArgs : EventArgs
    {
        public AbilityType AbilityType { get; set; }

        public bool IsRanged { get; set; }
    }
}
