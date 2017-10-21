namespace Identity.Api.DataAccess.Models
{
    public class Character : EntityBase
    {
        public string Name { get; set; }

        public long Health { get; set; }

        public long Armor { get; set; }

        public long Damage { get; set; }

        public int Speed { get; set; }
    }
}
