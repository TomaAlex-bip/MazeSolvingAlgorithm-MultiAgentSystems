﻿namespace MazeProject.Utils
{
    public static class MazeGenerator
    {
        public static int[,] GenerateMaze(int width, int height, out int actualWidth, out int actualHeight, int seed = 0)
        {
            actualWidth = width;
            actualHeight = height;

            // verify dimensions
            if (actualWidth < 3)
                actualWidth = 3;
            if (actualHeight < 3)
                actualHeight = 3;
            if (actualWidth % 2 == 0)
                actualWidth++;
            if (actualHeight % 2 == 0)
                actualHeight++;

            int[,] maze = new int[actualWidth, actualHeight];
            Random random = new(seed);
            if (seed == 0)
                random = new();
            List<int[]> frontierCells = new();

            // initialize the maze with walls
            for (int i = 0; i < actualWidth; i++)
                for (int j = 0; j < actualHeight; j++)
                    maze[i, j] = (int)MazeCell.Wall;

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
                if (maze[x, y] == (int)MazeCell.Wall)
                {
                    maze[x, y] = (int)MazeCell.Path;
                    maze[ix, iy] = (int)MazeCell.Path;

                    if (x > 2 && maze[x - 2, y] == (int)MazeCell.Wall)
                        frontierCells.Add(new int[] { x - 1, y, x - 2, y });

                    if (y > 2 && maze[x, y - 2] == (int)MazeCell.Wall)
                        frontierCells.Add(new int[] { x, y - 1, x, y - 2 });

                    if (x < actualWidth - 3 && maze[x + 2, y] == (int)MazeCell.Wall)
                        frontierCells.Add(new int[] { x + 1, y, x + 2, y });

                    if (y < actualHeight - 3 && maze[x, y + 2] == (int)MazeCell.Wall)
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
                    while (maze[exitX, exitY + 1] != (int)MazeCell.Path)
                        exitX = random.Next(1, actualWidth);

                    startY = actualHeight - 1;
                    startX = random.Next(actualWidth);
                    while (maze[startX, startY - 1] != (int)MazeCell.Path)
                        startX = random.Next(1, actualWidth);
                    break;

                case 1:
                    exitX = actualWidth - 1;
                    exitY = random.Next(actualHeight);
                    while (maze[exitX - 1, exitY] != (int)MazeCell.Path)
                        exitY = random.Next(actualWidth);

                    startX = 0;
                    startY = random.Next(actualHeight);
                    while (maze[startX + 1, startY] != (int)MazeCell.Path)
                        startY = random.Next(actualWidth);
                    break;

                case 2:
                    exitY = actualHeight - 1;
                    exitX = random.Next(actualWidth);
                    while (maze[exitX, exitY - 1] != (int)MazeCell.Path)
                        exitX = random.Next(1, actualWidth);

                    startY = 0;
                    startX = random.Next(actualWidth);
                    while (maze[startX, startY + 1] != (int)MazeCell.Path)
                        startX = random.Next(1, actualWidth);
                    break;

                default:
                    exitX = 0;
                    exitY = random.Next(actualHeight);
                    while (maze[exitX + 1, exitY] != (int)MazeCell.Path)
                        exitY = random.Next(actualWidth);

                    startX = actualWidth - 1;
                    startY = random.Next(actualHeight);
                    while (maze[startX - 1, startY] != (int)MazeCell.Path)
                        startY = random.Next(actualWidth);
                    break;

            }

            maze[exitX, exitY] = (int)MazeCell.Exit;
            maze[startX, startY] = (int)MazeCell.Start;

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
