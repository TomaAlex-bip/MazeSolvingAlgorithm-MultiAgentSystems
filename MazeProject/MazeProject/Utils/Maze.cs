namespace MazeProject.Utils
{
    public class Maze
    {
        public Cell[,] Cells { get; private set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Seed { get; private set; }

        public Maze(int width, int height, int seed)
        {
            Width = width;
            Height = height;
            Seed = seed;
            Cells = new Cell[Width, Height];
        }

        public Maze(Maze maze) 
        {
            Width = maze.Width;
            Height = maze.Height;
            StartX = maze.StartX;
            StartY = maze.StartY;
            Seed = maze.Seed;
            Cells = new Cell[Width, Height];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Cells[i, j] = new(maze.Cells[i, j]);
        }

        public void ResetMazeWeights()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    Cells[i, j].ResetWeights();
        }

        public void UpdateMazeWeight(int x, int y, int oldX, int oldY, float weightChange)
        {
            if (x > oldX) // moved right
            {
                Cells[oldX, oldY].RightWeight += weightChange;
                if (Cells[oldX, oldY].RightWeight > 1f)
                    Cells[oldX, oldY].RightWeight = 1f;
                if (Cells[oldX, oldY].RightWeight < 0f)
                    Cells[oldX, oldY].RightWeight = 0f;
            }
            if (x < oldX) // moved left
            {
                Cells[oldX, oldY].LeftWeight += weightChange;
                if (Cells[oldX, oldY].LeftWeight > 1f)
                    Cells[oldX, oldY].LeftWeight = 1f;
                if (Cells[oldX, oldY].LeftWeight < 0f)
                    Cells[oldX, oldY].LeftWeight = 0f;
            }
            if (y > oldY) // moved down
            {
                Cells[oldX, oldY].DownWeight += weightChange;
                if (Cells[oldX, oldY].DownWeight > 1f)
                    Cells[oldX, oldY].DownWeight = 1f;
                if (Cells[oldX, oldY].DownWeight < 0f)
                    Cells[oldX, oldY].DownWeight = 0f;
            }
            if (y < oldY) // moved up
            {
                Cells[oldX, oldY].UpWeight += weightChange;
                if (Cells[oldX, oldY].UpWeight > 1f)
                    Cells[oldX, oldY].UpWeight = 1f;
                if (Cells[oldX, oldY].UpWeight < 0f)
                    Cells[oldX, oldY].UpWeight = 0f;
            }
        }
    }
}
