namespace Game.Api.DataAccess.Models
{
    public class UserCharacter
    {
        public long Id { get; set; }

        public Character Character { get; set; }

        public long UserId { get; set; }

        public long Experience { get; set; }

        public long Level { get; set; }
    }
}
