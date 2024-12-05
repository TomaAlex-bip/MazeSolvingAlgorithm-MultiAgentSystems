using ActressMas;
using MazeProject.Data;
using MazeProject.Utils;

namespace MazeProject.Agents
{
    public class MazeAgent : Agent
    {
        public delegate void HandleFoundExit(MazeAgent agent);
        public delegate void HandleAgentMove(MoveData moveData);
        public event HandleFoundExit? OnFoundExitEvent;
        public event HandleAgentMove? OnAgentMoveEvent;

        public int X { get; private set; }
        public int Y { get; private set; }
        public int OldX { get; private set; }
        public int OldY { get; private set; }

        private readonly Maze _maze;
        private readonly Random _random;

        public MazeAgent(Maze maze, string name)
        {
            X = maze.StartX;
            Y = maze.StartY;
            OldX = X;
            OldY = Y;
            Name = name;
            _maze = new(maze);
            _random = new Random();
        }

        public override void Setup()
        {

        }

        public override void Act(ActressMas.Message message)
        {
            if (message.ContentObj is MoveData moveData)
            {
                _maze.UpdateMazeWeight(moveData.X, moveData.Y, moveData.OldX, moveData.OldY, moveData.WeightChange);
            }
        }

        public void Move()
        {
            MoveRandomly();
        }

        private void MoveRandomly()
        {
            VerifyIfFinishIsReachable();

            bool hasMoved = false;
            while (!hasMoved)
            {
                int d = _random.Next(4);
                switch (d)
                {
                    case 0: // up
                        hasMoved = MoveUp();
                        break;
                    
                    case 1: // right
                        hasMoved = MoveRight();
                        break;
                    
                    case 2: // down
                        hasMoved = MoveDown();
                        break;
                    
                    case 3: // left
                        hasMoved = MoveLeft();
                        break;
                }
            }

            MoveData moveData = new(X, Y, OldX, OldY, -0.1f);
            Broadcast(moveData);
            OnAgentMoveEvent?.Invoke(moveData);
        }

        private void VerifyIfFinishIsReachable()
        {
            if (X < 1 || Y < 1 || X >= _maze.Width - 1 || Y >= _maze.Height - 1)
                return;

            if (_maze.Cells[X + 1, Y].CellType == MazeCell.Exit)
            {
                OnFoundExitEvent?.Invoke(this);
            }
            if (_maze.Cells[X - 1, Y].CellType == MazeCell.Exit)
            {
                OnFoundExitEvent?.Invoke(this);
            }
            if (_maze.Cells[X, Y + 1].CellType == MazeCell.Exit)
            {
                OnFoundExitEvent?.Invoke(this);
            }
            if (_maze.Cells[X, Y - 1].CellType == MazeCell.Exit)
            {
                OnFoundExitEvent?.Invoke(this);
            }
        }

        private bool MoveUp()
        {
            if (Y - 1 < 0)
                return false;

            if (_maze.Cells[X, Y - 1].CellType != (int)MazeCell.Path)
                return false;

            OldX = X;
            OldY = Y;
            Y--;
            return true;
        }

        private bool MoveDown()
        {
            if (Y + 1 >= _maze.Cells.GetLength(0))
                return false;
            if (_maze.Cells[X, Y + 1].CellType != (int)MazeCell.Path)
                return false;

            OldX = X;
            OldY = Y;
            Y++;
            return true;
        }

        private bool MoveRight()
        {
            if (X + 1 >= _maze.Cells.GetLength(1))
                return false;
            if (_maze.Cells[X + 1, Y].CellType != (int)MazeCell.Path)
                return false;

            OldX = X;
            OldY = Y;
            X++;
            return true;
        }

        private bool MoveLeft()
        {
            if (X - 1 < 0)
                return false;
            if (_maze.Cells[X - 1, Y].CellType != (int)MazeCell.Path)
                return false;

            OldX = X;
            OldY = Y;
            X--;
            return true;
        }

        
    }
}
