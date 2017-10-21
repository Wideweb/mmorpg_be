using Game.Api.Constants;
using System.Collections.Generic;

namespace Game.Api.Models
{
    public class MapCell
    {
        public int X { get; }

        public int Y { get; }

        public int Type { get; }

        public bool IsWalkable { get; set; }

        public bool IsTransparent { get; set; }

        public Unit Unit { get; set; }

        public List<Point> Polygon { get; }
        
        public MapCell(int x, int y, int type, bool isWalkable, bool isTransparent, Unit unit = null)
        {
            X = x;
            Y = y;
            Type = type;
            IsWalkable = isWalkable;
            IsTransparent = isTransparent;
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
