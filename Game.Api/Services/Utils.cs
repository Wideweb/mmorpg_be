using Game.Api.Game.Models;
using System;

namespace Game.Api.Game.Services
{
    public class Utils
    {
        public static int GetDistance(Point first, Point second)
        {
            return Math.Abs(first.X - second.X) + Math.Abs(first.Y - second.Y);
        }
    }
}
