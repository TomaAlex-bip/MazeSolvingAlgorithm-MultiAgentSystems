using MazeProject.Agents;
using MazeProject.Data;
using MazeProject.Utils;
using System.Text;

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

        private async void buttonStartMultipleSimulations_Click(object sender, EventArgs e)
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

            int noSimulations = (int)numericNoSimulations.Value;
            if (noSimulations <= 0)
            {
                MessageBox.Show("At least one simulation round is needed!");
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
                        var simulationResults = await StartSimulation(noAgents, noSimulations);
                        ShowAndSaveSimulationResults(simulationResults);
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
                    var simulationResults = await StartSimulation(noAgents, noSimulations);
                    ShowAndSaveSimulationResults(simulationResults);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not start simulation.\r\n{ex.Message}", "Error");
                }
            }
        }

        private void ShowAndSaveSimulationResults(List<SimulationResults> simulationResults)
        {
            StringBuilder sb = new();
            var avgTurns = 0;
            foreach (var simulationresult in simulationResults)
            {
                sb.AppendLine(simulationresult.ToString());
                avgTurns += simulationresult.NoTurns;
            }
            avgTurns /= simulationResults.Count;
            sb.AppendLine($"\nAverage turns to find exit: {avgTurns}");

            var status = MessageBox.Show($"{sb}\r\n\r\n\tSave results?", "Simulation finished", MessageBoxButtons.YesNo);
            if (status != DialogResult.Yes)
                return;

            using var sfd = new SaveFileDialog();
            sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.FilterIndex = 1;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, sb.ToString());
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
                var agent = new MazeAgent(_maze, $"agent_{i}", checkBoxAgentsRandomMovement.Checked);
                _environment.Add(agent);
                _agents.Add(agent);
                agent.OnFoundExitEvent += Agent_OnFoundExitEvent;
                agent.OnAgentMoveEvent += Agent_OnAgentMoveEvent;
                _environment.OnAgentMoveEvent += agent.MakeTurn;
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

        private async Task<List<SimulationResults>> StartSimulation(int noAgents, int noSimulations)
        {
            var randomMovement = checkBoxAgentsRandomMovement.Checked;
            List<SimulationResults> simulationResults = new();

            if (_maze == null)
                throw new Exception("maze not generated");

            for (int simulationTurn = 0; simulationTurn < noSimulations; simulationTurn++)
            {
                _maze.ResetMazeWeights();
                _noTurns = 0;
                _environment = new();
                _agents.Clear();
                for (int i = 0; i < noAgents; i++)
                {
                    // create agents and add them to the environment
                    var agent = new MazeAgent(_maze, $"agent_{i}", randomMovement);
                    _environment.Add(agent);
                    _agents.Add(agent);
                    agent.OnFoundExitEvent += (MazeAgent _) => { StopSimulation(); };
                    agent.OnAgentMoveEvent += Agent_OnAgentMoveEvent;
                    _environment.OnAgentMoveEvent += agent.MakeTurn;
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
                _updatesEnabled = true;
                _startTime = DateTime.Now;

                await Task.Run(() => 
                {
                    _simulationThread.Join();
                });

                simulationResults.Add(new()
                {
                    SimulationId = simulationTurn,
                    NoAgents = noAgents,
                    MazeHeight = _maze.Height,
                    MazeWidth = _maze.Width,
                    MazeSeed = _maze.Seed,
                    NoTurns = _noTurns,
                });
            }

            return simulationResults;
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
                                g.FillRectangle(Brushes.White, x * cellSize, y * cellSize, cellSize, cellSize);
                            else
                                DrawPathWithWeights(g, maze, x, y, cellSize);
                            break;
                    }
                }
            }
        }

        private void DrawPathWithWeights(Graphics g, Maze maze, int x, int y, int cellSize)
        {
            Point topLeft = new(x * cellSize + cellSize / 4, y * cellSize + cellSize / 4);
            Point topRight = new(x * cellSize + cellSize - cellSize / 4, y * cellSize + cellSize / 4);
            Point bottomLeft = new(x * cellSize + cellSize / 4, y * cellSize + cellSize - cellSize / 4);
            Point bottomRight = new(x * cellSize + cellSize - cellSize / 4, y * cellSize + cellSize - cellSize / 4);
            Point center = new(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2);
            Point centerLeft = new(x * cellSize, y * cellSize + cellSize / 2);
            Point centerRight = new(x * cellSize + cellSize, y * cellSize + cellSize / 2);
            Point centerTop = new(x * cellSize + cellSize / 2, y * cellSize);
            Point centerBottom = new(x * cellSize + cellSize / 2, y * cellSize + cellSize);

            Color topColor = Color.White;
            Color bottomColor = Color.White;
            Color leftColor = Color.White;
            Color rightColor = Color.White;

            int maxColor = 250;
            int minColor = 50;

            if (maze.Cells[x, y].UpWeight != null)
            {
                var w = (float)maze.Cells[x, y].UpWeight!;
                topColor = GetColorFromWeight(w, minColor, maxColor);
            }
            if (maze.Cells[x, y].DownWeight != null)
            {
                var w = (float)maze.Cells[x, y].DownWeight!;
                bottomColor = GetColorFromWeight(w, minColor, maxColor);
            }
            if (maze.Cells[x, y].LeftWeight != null)
            {
                var w = (float)maze.Cells[x, y].LeftWeight!;
                leftColor = GetColorFromWeight(w, minColor, maxColor);
            }
            if (maze.Cells[x, y].RightWeight != null)
            {
                var w = (float)maze.Cells[x, y].RightWeight!;
                rightColor = GetColorFromWeight(w, minColor, maxColor);
            }

            g.FillPolygon(new SolidBrush(topColor), new Point[] { topLeft, centerTop, topRight }); // top triangle
            g.FillPolygon(new SolidBrush(bottomColor), new Point[] { bottomLeft, centerBottom, bottomRight }); // bottom triangle
            g.FillPolygon(new SolidBrush(leftColor), new Point[] { topLeft, centerLeft, bottomLeft }); // left triangle
            g.FillPolygon(new SolidBrush(rightColor), new Point[] { topRight, centerRight, bottomRight }); // right triangle
            g.FillRectangle(Brushes.White, x * cellSize + cellSize / 4, y * cellSize + cellSize / 4,
                cellSize / 2 + 1, cellSize / 2 + 1); // fill half center
        }

        private void DrawPathWithWeightsOld(Graphics g, Maze maze, int x, int y, int cellSize)
        {
            Color c = GetPathColor(maze, x, y);
            g.FillRectangle(new SolidBrush(c), x * cellSize, y * cellSize, cellSize, cellSize);
        }

        private Color GetColorFromWeight(float weight, int minColor, int maxColor)
        {
            if (weight < 0.5)
            {
                int colorValue = minColor + (int)(maxColor * weight);
                return Color.FromArgb(255, colorValue, colorValue);
            }
            else if (weight > 0.5)
            {
                int colorValue = (int)(maxColor * (weight - 0.5));
                return Color.FromArgb(colorValue, 255, colorValue);
            }
            return Color.White;
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
            int agentSize = (int)((minXY / maxMazeXY) * 0.7);

            foreach (var agent in _agents)
            {
                g.FillEllipse(Brushes.Blue, agent.X * cellSize + agentSize / 4, agent.Y * cellSize + agentSize / 4, agentSize, agentSize);
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
            int agentSize = (int)((minXY / maxMazeXY) * 0.7);

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

                    g.FillEllipse(Brushes.Blue, (int)((agent.X + dx) * cellSize + agentSize / 4), agent.Y * cellSize, agentSize, agentSize);

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

                    g.FillEllipse(Brushes.Blue, agent.X * cellSize, (int)((agent.Y + dy) * cellSize + agentSize / 4), agentSize, agentSize);

                    Graphics pbg = pictureBox.CreateGraphics();
                    pbg.DrawImage(final, 0, 0);
                }
            }
        }
    }
}
