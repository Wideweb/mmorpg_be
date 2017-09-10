using Game.Api.Game;

namespace Game.Api.Services
{
    public interface IDungeonService
    {
        Dungeon GetById(long dungeonType);
    }
}
