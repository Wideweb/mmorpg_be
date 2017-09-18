using Game.Api.Game.Models;

namespace Game.Api.Services
{
    public interface IDungeonService
    {
        Dungeon GetById(long dungeonType);
    }
}
