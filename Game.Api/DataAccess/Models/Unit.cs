namespace Game.Api.DataAccess.Models
{
    public class Unit
    {
        public long Id { get; set; }

        public long UnitType { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Character Character { get; set; }
    }
}
