using System.Collections.Generic;

namespace Game.Api.Game.Models
{
    public class MapCell
    {
        public int X { get; }

        public int Y { get; }

        public int Type { get; }

        public Unit Unit { get; set; }

        public List<Point> Polygon { get; }
        
        public MapCell(int x, int y, int type, Unit unit = null)
        {
            X = x;
            Y = y;
            Type = type;
            Unit = unit;

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
