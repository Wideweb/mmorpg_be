﻿using System;
using System.Collections.Generic;

namespace Game.Api.Game.PathfindingAlgorithm
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

            var tLower = 0;
            var tUpper = 1;
            var t = 0;

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

                var dScalar = d.X * n.X + d.Y * n.Y;
                var wScalar = w.X * n.X + w.Y * n.Y;

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
                    X = p1.X + (p2.X - p1.X) * tLower,
                    Y = p1.Y + (p2.Y - p1.Y) * tLower
                };

                var upperPoint = new Point
                {
                    X = p1.X + (p2.X - p1.X) * tUpper,
                    Y = p1.Y + (p2.Y - p1.Y) * tUpper
                };

                return new List<Point> { lowerPoint, upperPoint };
            }

            return null;
        }
    }
}
