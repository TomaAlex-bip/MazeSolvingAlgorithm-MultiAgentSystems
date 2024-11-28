namespace MazeProject.Utils
{
    public class Cell
    {
        public MazeCell CellType { get; private set; }
        public float? UpWeight { get; set; }
        public float? DownWeight { get; set; }
        public float? RightWeight { get; set; }
        public float? LeftWeight { get; set; }

        public Cell(MazeCell mazeCell)
        {
            CellType = mazeCell;
        }
    }
}
