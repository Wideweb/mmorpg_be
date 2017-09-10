using System.Collections.Generic;

namespace Game.Api.Game
{
    public class MapCell
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Type { get; set; }

        public Unit Unit { get; set; }

        public List<Point> Polygon { get; }

        public MapCell()
        {
            var screenX = X * GameConstants.MapCellWidth;
            var screenY = Y * GameConstants.MapCellWidth;

            Polygon = new List<Point>();
            Polygon.Add(new Point{ X = screenX, Y = screenY });
            Polygon.Add(new Point{ X = screenX + GameConstants.MapCellWidth, Y = screenY });
            Polygon.Add(new Point{ X = screenX + GameConstants.MapCellWidth, Y = screenY + GameConstants.MapCellWidth });
            Polygon.Add(new Point{ X = screenX, Y = screenY + GameConstants.MapCellWidth });
        }
    }
}
