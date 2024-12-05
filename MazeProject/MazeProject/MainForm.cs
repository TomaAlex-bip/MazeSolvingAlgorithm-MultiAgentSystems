using MazeProject.Agents;
using MazeProject.Data;
using MazeProject.Utils;

namespace MazeProject
{
    public partial class MainForm : Form
    {
        private Bitmap? _mazeImage;
        private Bitmap? _agentsImage;
        private Maze? _maze;
        private List<MazeAgent> _agents = new();
        private MazeEnvironment? _environment;
        private Thread? _simulationThread;
        private int _noTurns = 0;
        private DateTime _startTime;
        private DateTime _endTime;
        private TimeSpan _elapsedTime;
        private bool _updatesEnabled = true;

        public MainForm()
        {
            InitializeComponent();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (_mazeImage == null)
                return;

            Bitmap mazeBitmap = new(_mazeImage);
            e.Graphics.DrawImage(mazeBitmap, 0, 0);

            if (_agentsImage == null)
                return;

            Bitmap agentsBitmap = new(_agentsImage);
            e.Graphics.DrawImage(agentsBitmap, 0, 0);
        }

        private void buttonGenerateMaze_Click(object sender, EventArgs e)
        {
            if (_environment != null)
            {
                MessageBox.Show("Simulation in progress, cannot generate a new maze!");
                return;
            }

            if (_agentsImage != null)
            {
                _agentsImage.Dispose();
                _agentsImage = null;
                GC.Collect(); // prevents memory leaks
            }

            var mazeWidth = (int)numericMazeWidth.Value;
            var mazeHeight = (int)numericMazeHeight.Value;
            var mazeSeed = (int)numericMazeSeed.Value;

            _maze = MazeGenerator.GenerateMaze(mazeWidth, mazeHeight, mazeSeed);

            _mazeImage = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(_mazeImage);
            DrawMaze(g, _maze);
            pictureBox.Refresh();
        }

        private void buttonStartSimulation_Click(object sender, EventArgs e)
        {
            if (_maze == null)
            {
                MessageBox.Show("Generate a maze first!");
                return;
            }

            int noAgents = (int)numericNoAgents.Value;
            if (noAgents <= 0)
            {
                MessageBox.Show("At least one agent is needed!");
                return;
            }

            if (_environment != null)
            {
                var confirmResult = MessageBox.Show("A simulation is still in progress, abort?", "Confirm", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        StopSimulation();
                        StartSimulation(noAgents);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not start simulation.\r\n{ex.Message}", "Error");
                    }
                }
            }
            else
            {
                try
                {
                    StartSimulation(noAgents);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not start simulation.\r\n{ex.Message}", "Error");
                }
            }
        }

        private void buttonStopSimulation_Click(object sender, EventArgs e)
        {
            try
            {
                StopSimulation();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not stop simulation.\r\n{ex.Message}", "Error");
            }
        }

        private void Environment_OnAgentMoveEvent()
        {
            HandleTimeElapsed();

            if (_agentsImage != null)
            {
                _agentsImage.Dispose();
                GC.Collect(); // prevents memory leaks
            }

            _agentsImage = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(_agentsImage);
            DrawAgents(g);
            this.Invoke(() =>
            {
                pictureBox.Refresh();
            });
        }

        private void Agent_OnFoundExitEvent(MazeAgent agent)
        {
            StopSimulation();
            MessageBox.Show($"Agent {agent.Name} found the exit in {_noTurns} turns / {_elapsedTime.ToString("mm\\:ss")} minutes", "Info");
        }

        private void Agent_OnAgentMoveEvent(MoveData moveData)
        {
            _maze?.UpdateMazeWeight(moveData.X, moveData.Y, moveData.OldX, moveData.OldY, moveData.WeightChange);

            if (checkBoxMazeWeights.Checked)
            {
                this.Invoke(() =>
                {
                    if (_mazeImage == null)
                        return;
                    if (_maze == null)
                        return;

                    Graphics g = Graphics.FromImage(_mazeImage);
                    DrawMaze(g, _maze);
                    pictureBox.Refresh();
                });
            }
        }

        private void HandleTimeElapsed()
        {
            if (!_updatesEnabled)
                return;

            _noTurns++;
            _elapsedTime = DateTime.Now - _startTime;

            this.Invoke(() =>
            {
                labelTurnsToFindExit.Text = _noTurns.ToString();
                labelTimeToFindExit.Text = _elapsedTime.ToString("mm\\:ss");
            });
        }

        private void StartSimulation(int noAgents)
        {
            var turnTime = (int)numericTurnTime.Value;
            if (turnTime < 0)
                turnTime = 0;

            if (_maze == null)
                return;

            _maze.ResetMazeWeights();

            _environment = new(0, turnTime);
            _agents.Clear();
            for (int i = 0; i < noAgents; i++)
            {
                // create agents and add them to the environment
                var agent = new MazeAgent(_maze, $"agent_{i}");
                _environment.Add(agent);
                _agents.Add(agent);
                agent.OnFoundExitEvent += Agent_OnFoundExitEvent;
                agent.OnAgentMoveEvent += Agent_OnAgentMoveEvent;
                _environment.OnAgentMoveEvent += agent.Move;
            }
            _environment.OnAgentMoveEvent += Environment_OnAgentMoveEvent; ;

            _simulationThread = new Thread(() =>
            {
                try
                {
                    _environment.Start();
                }
                catch (Exception) { }
            });
            _simulationThread.Start();
            _noTurns = 0;
            _updatesEnabled = true;
            _startTime = DateTime.Now;
        }

        private void StopSimulation()
        {
            if (_environment == null)
                return;

            foreach (var agent in _environment.AllAgents())
                _environment.Remove(agent);

            _environment = null;
            _endTime = DateTime.Now;
            _elapsedTime = _endTime - _startTime;
            HandleTimeElapsed();
            _updatesEnabled = false;
        }

        private void DrawMaze(Graphics g, Maze maze, bool useWeights = true)
        {
            int minXY = Math.Min(pictureBox.Width, pictureBox.Height);
            int maxMazeXY = Math.Max(maze.Cells.GetLength(0), maze.Cells.GetLength(1));
            int cellSize = minXY / maxMazeXY;

            for (int x = 0; x < maze.Cells.GetLength(0); x++)
            {
                for (int y = 0; y < maze.Cells.GetLength(1); y++)
                {
                    switch (maze.Cells[x, y].CellType)
                    {
                        case MazeCell.Wall:
                            g.FillRectangle(Brushes.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                            break;
                        case MazeCell.Start:
                            g.FillRectangle(Brushes.Red, x * cellSize, y * cellSize, cellSize, cellSize);
                            break;
                        case MazeCell.Exit:
                            g.FillRectangle(Brushes.LimeGreen, x * cellSize, y * cellSize, cellSize, cellSize);
                            break;
                        case MazeCell.Path:
                            if (!useWeights)
                            {
                                g.FillRectangle(Brushes.White, x * cellSize, y * cellSize, cellSize, cellSize);
                                break;
                            }
                            Color c = GetPathColor(maze, x, y);
                            g.FillRectangle(new SolidBrush(c), x * cellSize, y * cellSize, cellSize, cellSize);
                            break;
                    }
                }
            }
        }

        private Color GetPathColor(Maze maze, int x, int y)
        {
            int maxColor = 250;
            int minColor = 50;
            float w = 0f;
            int c = 0;
            if (maze.Cells[x, y + 1].UpWeight != null)
            {
                w += (float)maze.Cells[x, y + 1].UpWeight!;
                c++;
            }
            if (maze.Cells[x, y - 1].DownWeight != null)
            {
                w += (float)maze.Cells[x, y - 1].DownWeight!;
                c++;
            }
            if (maze.Cells[x + 1, y].LeftWeight != null)
            {
                w += (float)maze.Cells[x + 1, y].LeftWeight!;
                c++;
            }
            if (maze.Cells[x - 1, y].RightWeight != null)
            {
                w += (float)maze.Cells[x - 1, y].RightWeight!;
                c++;
            }
            if (c > 0)
                w /= c;

            if (w < 0.5)
            {
                int colorValue = minColor + (int)(maxColor * w);
                return Color.FromArgb(255, colorValue, colorValue);
            }
            else if (w > 0.5)
            {
                int colorValue = (int)(maxColor * (w - 0.5));
                return Color.FromArgb(colorValue, 255, colorValue);
            }

            return Color.White;
        }

        private void DrawAgents(Graphics g)
        {
            if (_maze == null)
                return;

            int minXY = Math.Min(pictureBox.Width, pictureBox.Height);
            int maxMazeXY = Math.Max(_maze.Cells.GetLength(0), _maze.Cells.GetLength(1));
            int cellSize = minXY / maxMazeXY;
            int agentSize = (int)((minXY / maxMazeXY) * 0.9);

            foreach (var agent in _agents)
            {
                g.FillEllipse(Brushes.Blue, agent.X * cellSize, agent.Y * cellSize, agentSize, agentSize);
                AnimateAgent(agent);
            }

        }

        private void AnimateAgent(MazeAgent agent)
        {
            if (!checkBoxAnimations.Checked)
                return;

            if (_maze == null)
                return;

            if (_mazeImage == null)
                return;

            Bitmap final = new(_mazeImage.Width, _mazeImage.Height);
            Graphics g = Graphics.FromImage(final);

            int minXY = Math.Min(pictureBox.Width, pictureBox.Height);
            int maxMazeXY = Math.Max(_maze.Cells.GetLength(0), _maze.Cells.GetLength(1));
            int cellSize = minXY / maxMazeXY;
            int agentSize = (int)((minXY / maxMazeXY) * 0.9);

            int animationSteps = 2;
            if (agent.X != agent.OldX)
            {
                for (int a = 1; a < animationSteps; a++)
                {
                    double dx;
                    if (agent.X > agent.OldX)
                        dx = -(animationSteps - a) / (double)animationSteps;
                    else
                        dx = (animationSteps - a) / (double)animationSteps;

                    g.FillEllipse(Brushes.Blue, (int)((agent.X + dx) * cellSize), agent.Y * cellSize, agentSize, agentSize);

                    Graphics pbg = pictureBox.CreateGraphics();
                    pbg.DrawImage(final, 0, 0);

                }
            }
            else if (agent.Y != agent.OldY)
            {
                for (int a = 0; a <= animationSteps; a++)
                {
                    double dy;
                    if (agent.Y > agent.OldY)
                        dy = -(animationSteps - a) / (double)animationSteps;
                    else
                        dy = (animationSteps - a) / (double)animationSteps;

                    g.FillEllipse(Brushes.Blue, agent.X * cellSize, (int)((agent.Y + dy) * cellSize), agentSize, agentSize);


                    Graphics pbg = pictureBox.CreateGraphics();
                    pbg.DrawImage(final, 0, 0);
                }
            }
        }

        private void checkBoxMazeWeights_CheckedChanged(object sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                if (_maze == null)
                    return;

                if (_mazeImage == null)
                    return;

                Graphics g = Graphics.FromImage(_mazeImage);
                DrawMaze(g, _maze, false);
                pictureBox.Refresh();
            });
        }
    }
}
