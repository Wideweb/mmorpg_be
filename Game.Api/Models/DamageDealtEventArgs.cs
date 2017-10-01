using System;

namespace Game.Api.Game.Models
{
    public class DamageDealtEventArgs : EventArgs
    {
        public Unit Target { get; set; }

        public int Damage { get; set; }
    }
}
