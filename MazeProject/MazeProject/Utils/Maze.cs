using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
