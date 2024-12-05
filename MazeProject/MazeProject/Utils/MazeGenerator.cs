namespace MazeProject.Utils
{
    public static class MazeGenerator
    {
        public static Maze GenerateMaze(int width, int height, int seed = 0)
        {
            int actualWidth = width;
            int actualHeight = height;

            // verify dimensions
            if (actualWidth < 3)
                actualWidth = 3;
            if (actualHeight < 3)
                actualHeight = 3;
            if (actualWidth % 2 == 0)
                actualWidth++;
            if (actualHeight % 2 == 0)
                actualHeight++;

            Maze maze = new(actualWidth, actualHeight, seed);
            Random random = new(seed);
            if (seed == 0)
                random = new();
            List<int[]> frontierCells = new();

            // initialize the maze with walls
            for (int i = 0; i < actualWidth; i++)
                for (int j = 0; j < actualHeight; j++)
                    maze.Cells[i, j] = new(MazeCell.Wall);

            // choose a random starting point and add it as a frontier cell
            int initX = random.Next(1, actualWidth);
            int initY = random.Next(1, actualHeight);
            while (initX % 2 == 0)
                initX = random.Next(1, actualWidth);
            while (initY % 2 == 0)
                initY = random.Next(1, actualHeight);

            frontierCells.Add(new int[] { initX, initY, initX, initY });

            // compute all frontier cells
            while (frontierCells.Any())
            {
                int[] frontierCell = frontierCells[random.Next(frontierCells.Count)];
                frontierCells.Remove(frontierCell);
                int x = frontierCell[2];
                int y = frontierCell[3];
                int ix = frontierCell[0];
                int iy = frontierCell[1];
                if (maze.Cells[x, y].CellType == MazeCell.Wall)
                {
                    maze.Cells[x, y] = new(MazeCell.Path);
                    maze.Cells[ix, iy] = new(MazeCell.Path);

                    if (x > 2 && maze.Cells[x - 2, y].CellType == MazeCell.Wall)
                        frontierCells.Add(new int[] { x - 1, y, x - 2, y });

                    if (y > 2 && maze.Cells[x, y - 2].CellType == MazeCell.Wall)
                        frontierCells.Add(new int[] { x, y - 1, x, y - 2 });

                    if (x < actualWidth - 3 && maze.Cells[x + 2, y].CellType == MazeCell.Wall)
                        frontierCells.Add(new int[] { x + 1, y, x + 2, y });

                    if (y < actualHeight - 3 && maze.Cells[x, y + 2].CellType == MazeCell.Wall)
                        frontierCells.Add(new int[] { x, y + 1, x, y + 2 });
                }
            }

            int exitX, exitY;
            int startX, startY;
            int randomSide = random.Next(3);
            switch (randomSide)
            {
                case 0:
                    exitY = 0;
                    exitX = random.Next(actualWidth);
                    while (maze.Cells[exitX, exitY + 1].CellType != MazeCell.Path)
                        exitX = random.Next(1, actualWidth);

                    startY = actualHeight - 1;
                    startX = random.Next(actualWidth);
                    while (maze.Cells[startX, startY - 1].CellType != MazeCell.Path)
                        startX = random.Next(1, actualWidth);
                    break;

                case 1:
                    exitX = actualWidth - 1;
                    exitY = random.Next(actualHeight);
                    while (maze.Cells[exitX - 1, exitY].CellType != MazeCell.Path)
                        exitY = random.Next(actualHeight);

                    startX = 0;
                    startY = random.Next(actualHeight);
                    while (maze.Cells[startX + 1, startY].CellType != MazeCell.Path)
                        startY = random.Next(actualHeight);
                    break;

                case 2:
                    exitY = actualHeight - 1;
                    exitX = random.Next(actualWidth);
                    while (maze.Cells[exitX, exitY - 1].CellType != MazeCell.Path)
                        exitX = random.Next(1, actualWidth);

                    startY = 0;
                    startX = random.Next(actualWidth);
                    while (maze.Cells[startX, startY + 1].CellType != MazeCell.Path)
                        startX = random.Next(1, actualWidth);
                    break;

                default:
                    exitX = 0;
                    exitY = random.Next(actualHeight);
                    while (maze.Cells[exitX + 1, exitY].CellType != MazeCell.Path)
                        exitY = random.Next(actualHeight);

                    startX = actualWidth - 1;
                    startY = random.Next(actualHeight);
                    while (maze.Cells[startX - 1, startY].CellType != MazeCell.Path)
                        startY = random.Next(actualHeight);
                    break;

            }

            maze.Cells[exitX, exitY] = new(MazeCell.Exit);
            maze.Cells[startX, startY] = new(MazeCell.Start);

            maze.StartX = startX;
            maze.StartY = startY;


            for (int i = 0; i < actualWidth; i++)
                for (int j = 0; j < actualHeight; j++)
                {
                    if (maze.Cells[i, j].CellType == MazeCell.Wall)
                        continue;

                    if (j - 1 > 0)
                        if (maze.Cells[i, j - 1].CellType == MazeCell.Path)
                            maze.Cells[i, j].UpWeight = 0.5f;

                    if (j + 1 < maze.Height)
                        if (maze.Cells[i, j + 1].CellType == MazeCell.Path)
                            maze.Cells[i, j].DownWeight = 0.5f;

                    if (i - 1 > 0)
                        if (maze.Cells[i - 1, j].CellType == MazeCell.Path)
                            maze.Cells[i, j].LeftWeight = 0.5f;

                    if (i + 1 < maze.Width)
                        if (maze.Cells[i + 1, j].CellType == MazeCell.Path)
                            maze.Cells[i, j].RightWeight = 0.5f;
                }

            return maze;
        }
    }

    public enum MazeCell
    {
        Path = 0, 
        Wall = 1, 
        Exit = 2,
        Start = 3
    }
}
