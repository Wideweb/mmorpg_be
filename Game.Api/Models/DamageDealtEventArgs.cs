using System;

namespace Game.Api.Models
{
    public class DamageDealtEventArgs : EventArgs
    {
        public Unit Target { get; set; }

        public int Damage { get; set; }
    }
}
