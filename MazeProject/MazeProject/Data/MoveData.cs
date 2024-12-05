namespace MazeProject.Data
{
    public class MoveData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int OldX { get; set; }
        public int OldY { get; set; }
        public float WeightChange { get; set; }

        public MoveData(int x, int y, int oldX, int oldY, float weightChange)
        {
            X = x;
            Y = y;
            OldX = oldX;
            OldY = oldY;
            WeightChange = weightChange;
        }
    }
}
