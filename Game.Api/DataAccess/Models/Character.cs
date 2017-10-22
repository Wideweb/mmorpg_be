namespace Game.Api.DataAccess.Models
{
    public class Character
    {
        public long Id { get; set; }

        public long Health { get; set; }

        public long Armor { get; set; }

        public long Damage { get; set; }

        public int Speed { get; set; }

        public string Name { get; set; }

        public int WatchRange { get; set; }

        public int AttackRange { get; set; }

        public int AttackSpeed { get; set; }

        public int AbilityCastSpeed { get; set; }
    }
}
