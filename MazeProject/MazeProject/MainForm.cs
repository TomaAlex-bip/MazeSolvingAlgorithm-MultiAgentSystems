using MazeProject.Utils;
using System.Drawing;

namespace MazeProject
{
    public partial class MainForm : Form
    {
        private Bitmap? _mazeImage;
        //private Bitmap _agentsImage;
        private int[,]? _maze;

        public MainForm()
        {
            InitializeComponent();
        }

        private void DrawMaze(Graphics g, int[,] maze)
        {
            int minXY = Math.Min(pictureBox.Width, pictureBox.Height);
            int maxMazeXY = Math.Max(maze.GetLength(0), maze.GetLength(1));
            int cellSize = minXY / maxMazeXY;

            for (int x = 0; x < maze.GetLength(0); x++)
            {
                for (int y = 0; y < maze.GetLength(1); y++)
                {
                    switch (maze[x, y])
                    {
                        case (int)MazeCell.Wall:
                            g.FillRectangle(new SolidBrush(Color.Black), x * cellSize, y * cellSize, cellSize, cellSize);
                            break;
                        case (int)MazeCell.Start:
                            g.FillRectangle(new SolidBrush(Color.Red), x * cellSize, y * cellSize, cellSize, cellSize);
                            break;
                        case (int)MazeCell.Exit:
                            g.FillRectangle(new SolidBrush(Color.LimeGreen), x * cellSize, y * cellSize, cellSize, cellSize);
                            break;
                    }
                }
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (_mazeImage == null)
                return;

            Bitmap mazeBitmap = new (_mazeImage);
            e.Graphics.DrawImage(mazeBitmap, 0, 0);
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {

        }

        private void buttonGenerateMaze_Click(object sender, EventArgs e)
        {
            var mazeWidth = (int)numericMazeWidth.Value;
            var mazeHeight = (int)numericMazeHeight.Value;
            var mazeSeed = (int)numericMazeSeed.Value;

            _maze = MazeGenerator.GenerateMaze(mazeWidth, mazeHeight, out var _, out var _, mazeSeed);
            
            _mazeImage = new Bitmap(pictureBox.Width, pictureBox.Height);

            Graphics g = Graphics.FromImage(_mazeImage);

            DrawMaze(g, _maze);

            pictureBox.Refresh();
        }
    }
}
