using Game.Api.DataAccess.Models.Identity;

namespace Game.Api.DataAccess.Models.Game
{
    public class UserCharacter
    {
        public long Id { get; set; }

        public Character Character { get; set; }

        public User User { get; set; }

        public long Experience { get; set; }

        public long Level { get; set; }
    }
}
