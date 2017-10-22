using Game.Api.Constants;
using System;

namespace Game.Api.Models.Abilities
{
    public class AbilityCastEventArgs : EventArgs
    {
        public AbilityType AbilityType { get; set; }

        public long CastTime { get; set; }
    }
}
