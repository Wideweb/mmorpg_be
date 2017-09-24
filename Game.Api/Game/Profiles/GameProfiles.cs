using Game.Api.Game.Dto;
using Game.Api.Game.Models;
using System.Linq;

namespace Game.Api.Game.Profiles
{
    public static class GameProfiles
    {
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
                JoinedAt = player.JoinedAt,
                Unit = Map(player.Unit)
            };
        }

        public static GameObjectDto Map(GameObject gameObject)
        {
            return new GameObjectDto
            {
                Sid = gameObject.Sid,
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
