using Game.Api.Models;

namespace Game.Api.Services
{
    public interface IDungeonService
    {
        Dungeon GetById(long dungeonType);
    }
}
