using Game.Api.Game.PathfindingAlgorithm;
using Game.Api.WebSocketManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Api.Game
{
    public class Unit : Target
    {
        private Point _position;
        private Point _screenPosition;

        public Guid Sid { get; }

        public event EventHandler<EventArgs> OnCellChanged;

        public Point Position
        {
            get { return _position; }
            set
            {
                _position.X = value.X;
                _position.Y = value.Y;

                _screenPosition = new Point
                {
                    X = _position.X * GameConstants.MapCellWidth,
                    Y = _position.Y * GameConstants.MapCellWidth,
                };
            }
        }

        public Point ScreenPosition
        {
            get { return _screenPosition; }
            set
            {
                _screenPosition = value;
                _position.X = (int)Math.Floor((double)(_screenPosition.X / GameConstants.MapCellWidth));
                _position.Y = (int)Math.Floor((double)(_screenPosition.Y / GameConstants.MapCellWidth));
            }
        }

        public MapCell Cell => _cell;

        public Point ScreenPositionCenter => new Point { X = ScreenPosition.X + GameConstants.MapCellWidth / 2, Y = ScreenPosition.Y + GameConstants.MapCellWidth / 2 };

        private List<MapCell> _path;
        private Point _originPosition;
        private Dungeon _map;
        private MapCell _cell;

        private bool _moving = false;
        private int _speed = 1;
        private int _radius = 10;

        private long _checkPause = 1500;
        private long _checkPauseElapsed = 0;

        private long _updatePathPause = 500;
        private long _updatePathElapsed = 0;

        private Target _target;

        public Unit(Point position, Dungeon map)
        {
            _path = new List<MapCell>();
            _position = position;
            _originPosition = new Point { X = position.X, Y = position.Y };
            _map = map;
            _cell = map.GetCell(_position.X, _position.Y);
            _cell.Unit = this;

            _target = null;

            _screenPosition = new Point
            {
                X = _position.X * GameConstants.MapCellWidth,
                Y = _position.Y * GameConstants.MapCellWidth
            };

            Sid = Guid.NewGuid();
        }


        public void Update(long elapsed)
        {
            _checkPauseElapsed += elapsed;
            _updatePathElapsed += elapsed;

            if (_checkPauseElapsed > _checkPause)
            {
                _checkPauseElapsed = 0;
                Watch();
            }

            if (_updatePathElapsed > _updatePathPause)
            {
                _updatePathElapsed = 0;

                if (this._target is Unit) {
                    UpdatePath();
                }
            }

            if (NeedMove())
            {
                Run(elapsed);
            }
        }


        private void Watch()
        {
            var walls = new List<MapCell>();
            var units = new List<Unit>();

            if (_target is Unit) {
                var target = (Unit)_target;
                var minX = Math.Min(target.Position.X, this._position.X);
                var minY = Math.Min(target.Position.Y, this._position.Y);
                var maxX = Math.Max(target.Position.X, this._position.X);
                var maxY = Math.Max(target.Position.Y, this._position.Y);

                for (var x = minX; x <= maxX; x++)
                {
                    for (var y = minY; y <= maxY; y++)
                    {
                        var cell = this._map.GetCell(x, y);

                        if (cell.Type == 1)
                        {
                            walls.Add(cell);
                        }
                    }
                }

                var visible = walls.All(w => {
                    return ClippingAlgorithm.FindIntersection(
                        ScreenPositionCenter,
                        target.ScreenPositionCenter,
                        w.Polygon
                    ) == null;
                });

                if (!visible)
                {
                    _target = null;
                }
                else
                {
                    return;
                }
            }

            for (var dy = -_radius; dy <= _radius; dy++)
            {
                for (var dx = -_radius; dx <= _radius; dx++)
                {
                    var x = _position.X + dx;
                    var y = _position.Y + dy;

                    if (y < 0 || y >= this._map.Height)
                    {
                        continue;
                    }

                    if (x < 0 || x >= this._map.Width)
                    {
                        continue;
                    }

                    var cell = this._map.GetCell(x, y);

                    if (cell.Type == 1)
                    {
                        walls.Add(cell);
                    }
                    else if (cell.Unit != this)
                    {
                        units.Add(cell.Unit);
                    }
                }
            }
            
            foreach (var unit in units)
            {
                var visible = walls.All(w => {
                    return ClippingAlgorithm.FindIntersection(
                        ScreenPositionCenter,
                        unit.ScreenPositionCenter,
                        w.Polygon
                    ) == null;
                });

                if (visible)
                {
                    _target = unit;
                    break;
                }
            }
        }


        private void Run(long elapsed)
        {
            var next = _path[0];

            _cell.Unit = null;
            next.Unit = this;
            _cell = next;

            if (next != _cell)
            {
                OnCellChanged?.Invoke(this, null);
            }

            _position.X = next.X;
            _position.Y = next.Y;

            int deltaX = next.X * GameConstants.MapCellWidth - _screenPosition.X;
            int deltaY = next.Y * GameConstants.MapCellWidth - _screenPosition.Y;

            var distace = Math.Abs(deltaX) + Math.Abs(deltaY);
            var speed = _speed * elapsed / 10;

            if (distace > speed)
            {
                var angle = Math.Atan2(deltaY, deltaX);

                deltaX = (int)(Math.Cos(angle) * speed);
                deltaY = (int)(Math.Sin(angle) * speed);
            }
            else
            {
                _path.RemoveAt(0);
            }

            _screenPosition.X += deltaX;
            _screenPosition.Y += deltaY;
        }


        private bool NeedMove()
        {
            return _path != null && _path.Count != 0;
        }


        private void UpdatePath()
        {
            if (_target == null)
            {
                _target = _originPosition;
            }

            var isUnit = _target is Unit;

            Point targetPosition;
            if (isUnit)
            {
                targetPosition = (_target as Unit).Position;
            }
            else
            {
                targetPosition = _target as Point; 
            }

            var pathFinder = new AStartAlgoritm(_map, _position, targetPosition, p => _map.GetCell(p.X, p.Y).Type == 1);

            var path = pathFinder.GetPath();

            if (isUnit && NeedMove())
            {
                path.RemoveAt(path.Count - 1);
            }
            _path = path;
        }
    }
}
