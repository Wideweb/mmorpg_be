using System;
using System.Collections.Generic;

namespace Game.Api.Services
{
    public class RoomUpdatedEventArgs : EventArgs
    {
        public string Room { get; set; }

        public IEnumerable<string> Enemies { get; set; }

        public IEnumerable<int> EnemiesX { get; set; }

        public IEnumerable<int> EnemiesY { get; set; }
    }
}
