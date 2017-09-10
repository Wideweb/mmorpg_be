namespace Game.Api.Game.PathfindingAlgorithm
{
    public class AStartCell
    {
        public AStartCell(Point position, AStartCell parent, int toTargetScore, int fromStartScore)
        {
            Position = position;
            Parent = parent;
            ToTargetScore = toTargetScore;
            FromStartScore = fromStartScore;
        }

        public Point Position { get; set; }

        public AStartCell Parent { get; set; }

        public int Score => this.ToTargetScore + this.FromStartScore;

        public int ToTargetScore { get; set; }

        public int FromStartScore { get; set; }
    }
}
