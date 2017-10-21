using Game.Api.Constants;
using Game.Api.Models.Abilities;
using Game.Api.Models.GameEventArgs;
using Game.Api.Services;
using Game.Api.Services.PathfindingAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Api.Models
{
    public class Unit : GameObject
    {
        private Point _position;
        private Point _screenPosition;

        public event EventHandler<CellChangedEventArgs> OnCellChanged;
        public event EventHandler<AbilityUsedEventArgs> OnAbilityUsed;
        public event EventHandler<EventArgs> OnDied;

        public override GameObjectType Type => GameObjectType.Unit;

        public override Point Position
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

        public override Point ScreenPosition
        {
            get { return _screenPosition; }
            set
            {
                _screenPosition = value;
                _position.X = (int)Math.Floor((double)(_screenPosition.X / GameConstants.MapCellWidth));
                _position.Y = (int)Math.Floor((double)(_screenPosition.Y / GameConstants.MapCellWidth));
            }
        }

        public override Target Target
        {
            set {
                _target = value;
                UpdatePath();
            }
            get => _target;
        }

        public MapCell Cell => _cell;

        public Point ScreenPositionCenter => new Point { X = ScreenPosition.X + GameConstants.MapCellWidth / 2, Y = ScreenPosition.Y + GameConstants.MapCellWidth / 2 };

        private List<MapCell> _path;
        private Point _originPosition;
        private Dungeon _map;
        private MapCell _cell;
        
        private int _speed = 1;
        private int _watchRange = 5;
        private int _attackRange = 3;
        private int _cooldown = 2000;

        private long _checkPause = 1500;
        private long _checkPauseElapsed = 0;
        private bool _watch;

        private long _updatePathPause = 500;
        private long _updatePathElapsed = 0;

        private Ability _currentAbility;
        private Target _target;

        public Unit(Point position, Dungeon map, string sid, bool watch, long health, int speed)
        {
            _path = new List<MapCell>();
            _position = position;
            _originPosition = new Point { X = position.X, Y = position.Y };
            _map = map;
            _cell = map.GetCell(_position.X, _position.Y);
            _cell.Unit = this;
            _watch = watch;

            _target = null;

            _screenPosition = new Point
            {
                X = _position.X * GameConstants.MapCellWidth,
                Y = _position.Y * GameConstants.MapCellWidth
            };

            Sid = sid;
            Health = health;
            MaxHealth = health;
            Width = GameConstants.MapCellWidth;
            _currentAbility = new RangeAttack(this, _cooldown, _attackRange);
            _currentAbility.OnUsed += (s, e) => OnAbilityUsed?.Invoke(this, e);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if(Health <= 0)
            {
                //Position = new Point { X = 1, Y = 1 };
                Health = MaxHealth;
                OnDied?.Invoke(this, null);
            }
        }

        public override void Update(long elapsed)
        {
            if(_target != null && _currentAbility != null && _target is Unit)
            {
                _currentAbility.Update(elapsed);
                if (_currentAbility.CanUse())
                {
                    _currentAbility.Use();
                }
            }
            
            if (_watch)
            {
                _checkPauseElapsed += elapsed;

                if (_checkPauseElapsed > _checkPause)
                {
                    _checkPauseElapsed = 0;
                    Watch();
                }
            }

            _updatePathElapsed += elapsed;
            if (_updatePathElapsed > _updatePathPause)
            {
                _updatePathElapsed = 0;

                if (_target is Unit) {
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
                var minX = Math.Min(target.Position.X, _position.X);
                var minY = Math.Min(target.Position.Y, _position.Y);
                var maxX = Math.Max(target.Position.X, _position.X);
                var maxY = Math.Max(target.Position.Y, _position.Y);

                for (var x = minX; x <= maxX; x++)
                {
                    for (var y = minY; y <= maxY; y++)
                    {
                        var cell = this._map.GetCell(x, y);

                        if (!cell.IsTransparent)
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

            for (var dy = -_watchRange; dy <= _watchRange; dy++)
            {
                for (var dx = -_watchRange; dx <= _watchRange; dx++)
                {
                    var x = _position.X + dx;
                    var y = _position.Y + dy;

                    if (y < 0 || y >= _map.Height)
                    {
                        continue;
                    }

                    if (x < 0 || x >= _map.Width)
                    {
                        continue;
                    }

                    var cell = _map.GetCell(x, y);

                    if (!cell.IsTransparent)
                    {
                        walls.Add(cell);
                    }
                    else if (cell.Unit != null && cell.Unit != this)
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

            _position.X = next.X;
            _position.Y = next.Y;

            if (next != _cell)
            {
                OnCellChanged?.Invoke(this, null);
            }

            _cell = next;
            
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
            var target = _target as Unit;

            if (target != null && _currentAbility.IsRanged)
            {
                var distance = Utils.GetDistance(_position, target.Position);
                if(distance <= _currentAbility.Range)
                {
                    return false;
                }
            }

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

            var pathFinder = new AStartAlgoritm(_map, _position, targetPosition, p => !_map.GetCell(p.X, p.Y).IsWalkable);

            var path = pathFinder.GetPath();

            if (path != null && isUnit)
            {
                path.RemoveAt(path.Count - 1);
            }
            _path = path;
        }

        private void StopMoving()
        {
            _path = null;
        }
    }
}
