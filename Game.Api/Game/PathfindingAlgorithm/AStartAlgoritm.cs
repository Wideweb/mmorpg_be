using Game.Api.Models.Game;
using System;
using System.Collections.Generic;

namespace Game.Api.Game.PathfindingAlgorithm
{
    public class AStartAlgoritm
    {
        private int[] _dy => new int[8] { 0, 0, 1, -1, 1, -1, 1, -1 };
        private int[] _dx => new int[8] { 1, -1, 0, 0, 1, 1, -1, -1 };

        private readonly Dungeon _map;
        private readonly Point _start;
        private readonly Point _target;
        private readonly Predicate<Point> _ignorePositionFunc;

        private List<AStartCell> _open;
        private List<AStartCell> _closed;
        private AStartCell _targetCell;

        public AStartAlgoritm(Dungeon map, Point start, Point target, Predicate<Point> ignorePositionFunc)
        {
            _open = new List<AStartCell>();
            _closed = new List<AStartCell>();
            _start = start;
            _target = target;
            _map = map;
            _ignorePositionFunc = ignorePositionFunc;
            _targetCell = null;
        }


        public List<MapCell> GetPath()
        {
            if (_target == null || _ignorePositionFunc(_target))
            {
                return new List<MapCell>();
            }

            _targetCell = null;

            var fromStartScore = 0;
            var toTargetScore = GetDistance(_start, _target);

            _open.Add(new AStartCell(_start, null, fromStartScore, toTargetScore));

            while (_open.Count != 0)
            {
                var cell = FindBestOpenCell();
                AddAdjacentCells(cell);

                if (_targetCell != null)
                {
                    return BuldPath(_targetCell);
                }

                var cellIndex = _open.FindIndex(c => c.Position.X == cell.Position.X && c.Position.Y == cell.Position.Y);
                _open.RemoveAt(cellIndex);
                _closed.Add(cell);
            }

            return null;
        }

        private AStartCell FindBestOpenCell()
        {
            var best = _open[0];
            foreach (var cell in _open)
            {
                if (cell.Score < best.Score)
                {
                    best = cell;
                }
            }

            return best;
        }

        private void AddAdjacentCells(AStartCell cell)
        {
            var position = cell.Position;
            for (var i = 0; i < _dy.Length; i++)
            {
                var y = position.Y + _dy[i];
                var x = position.X + _dx[i];
                AddAdjacentCell(new Point { X = x, Y = y }, cell);
            }
        }


        public void AddAdjacentCell(Point position, AStartCell parent)
        {
            if (position.Y < 0 || position.Y >= _map.Height)
            {
                return;
            }

            if (position.X < 0 || position.X >= _map.Width)
            {
                return;
            }

            if (_ignorePositionFunc(position))
            {
                return;
            }

            if (_closed.FindIndex(c => c.Position.X == position.X && c.Position.Y == position.Y) != -1)
            {
                return;
            }

            var delta = GetDistance(parent.Position, position);
            var fromStartScore = parent.FromStartScore + delta;
            var toTargetScore = GetDistance(position, _target);

            var cell = _open.Find(c => c.Position.X == position.X && c.Position.Y == position.Y);

            if (cell != null)
            {
                if (fromStartScore < cell.FromStartScore)
                {
                    cell.Parent = parent;
                }
                return;
            }

            cell = new AStartCell(position, parent, fromStartScore, toTargetScore);

            if (position.X == _target.X && position.Y == _target.Y)
            {
                _targetCell = cell;
            }

            _open.Add(cell);
            return;
        }


        private int GetDistance(Point first, Point second)
        {
            return Math.Abs(first.X - second.X) + Math.Abs(first.Y - second.Y);
        }


        private List<MapCell> BuldPath(AStartCell cell)
        {
            var path = new List<MapCell>();

            while (cell != null)
            {
                path.Add(_map.Map[cell.Position.Y][cell.Position.X]);
                cell = cell.Parent;
            }

            path.Reverse();
            return path;
        }
    }
}