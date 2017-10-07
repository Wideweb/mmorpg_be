using Game.Api.Dto;
using Game.Api.Models;
using System.Linq;

namespace Game.Api.Profiles
{
    public static class GameProfiles
    {
        public static RoomDto Map(Room room)
        {
            return new RoomDto
            {
                Name = room.Name,
                IsStarted = room.IsStarted,
                Players = room.Players.Select(Map)
            };
        }

        public static MapDto Map(Dungeon dungeon)
        {
            return new MapDto
            {
                Cells = dungeon.Map.Select(r => r.Select(Map))
            };
        }

        public static MapCellDto Map(MapCell cell)
        {
            return new MapCellDto {
                X = cell.X,
                Y = cell.Y,
                Type = cell.Type
            };
        }

        public static PlayerDto Map(Player player)
        {
            return new PlayerDto
            {
                Sid = player.Sid,
                Name = player.Name,
                JoinedAt = player.JoinedAt,
                Unit = Map(player.Unit)
            };
        }

        public static GameObjectDto Map(GameObject gameObject)
        {
            return new GameObjectDto
            {
                Sid = gameObject.Sid,
                Name = gameObject.Name,
                Health = gameObject.Health,
                MaxHealth = gameObject.MaxHealth,
                Width = gameObject.Width,
                Position = gameObject.Position,
                Type = gameObject.Type,
                ScreenPosition = gameObject.ScreenPosition,
                Target = (gameObject.Target as GameObject)?.Sid,
                TargetPosition = (gameObject.Target as Point)
            };
        }
    }
}
