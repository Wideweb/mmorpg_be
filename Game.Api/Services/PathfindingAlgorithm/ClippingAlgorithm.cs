using Game.Api.Models;
using System;
using System.Collections.Generic;

namespace Game.Api.Services.PathfindingAlgorithm
{
    public static class ClippingAlgorithm
    {
        const double EPSILON = 0.000001;

        public static List<Point> FindIntersection(Point p1, Point p2, List<Point> polygon)
        {
            var d = new Point
            {
                X = p2.X - p1.X,
                Y = p2.Y - p1.Y
            };

            float tLower = 0.0f;
            float tUpper = 1.0f;
            float t = 0.0f;

            for (var i = 0; i < polygon.Count; i++)
            {
                var n = new Point
                {
                    X = polygon[i].Y - polygon[(i + 1) % polygon.Count].Y,
                    Y = polygon[(i + 1) % polygon.Count].X - polygon[i].X
                };
                var w = new Point
                {
                    X = p1.X - polygon[i].X,
                    Y = p1.Y - polygon[i].Y
                };

                float dScalar = d.X * n.X + d.Y * n.Y;
                float wScalar = w.X * n.X + w.Y * n.Y;

                if (Math.Abs(dScalar) < EPSILON)
                {
                    if (wScalar < 0)
                    {
                        tLower = 2;
                        break;
                    }
                }
                else
                {
                    t = -wScalar / dScalar;
                    if (dScalar > 0)
                    {
                        if (t > 1)
                        {
                            tLower = 2;
                            break;
                        }
                        tLower = Math.Max(t, tLower);
                        continue;
                    }
                    if (t < 0)
                    {
                        tLower = 2;
                        break;
                    }
                    tUpper = Math.Min(t, tUpper);
                }
            }
            if (tLower < tUpper)
            {
                var lowerPoint = new Point
                {
                    X = (int)(p1.X + (p2.X - p1.X) * tLower),
                    Y = (int)(p1.Y + (p2.Y - p1.Y) * tLower)
                };

                var upperPoint = new Point
                {
                    X = (int)(p1.X + (p2.X - p1.X) * tUpper),
                    Y = (int)(p1.Y + (p2.Y - p1.Y) * tUpper)
                };

                return new List<Point> { lowerPoint, upperPoint };
            }

            return null;
        }
    }
}
