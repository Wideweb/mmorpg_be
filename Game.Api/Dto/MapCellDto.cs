namespace Game.Api.Dto
{
    public class MapCellDto
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Type { get; set; }

        public bool IsWalkable { get; set; }

        public bool IsTransparent { get; set; }
    }
}
