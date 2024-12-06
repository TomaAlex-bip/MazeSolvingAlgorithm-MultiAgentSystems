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
        public MoveDirection? ComingDirection
        {
            get
            {
                if (X > OldX)
                    return MoveDirection.Left;
                if (X < OldX)
                    return MoveDirection.Right;
                if (Y > OldY)
                    return MoveDirection.Up;
                if (Y < OldY)
                    return MoveDirection.Down;
                return MoveDirection.None;
            }
        }

        private readonly Maze _maze;
        private readonly Random _random;
        private bool _isInDeadEnd = false;
        private bool _moveRandom = false;

        public MazeAgent(Maze maze, string name, bool moveRandom)
        {
            X = maze.StartX;
            Y = maze.StartY;
            OldX = X;
            OldY = Y;
            Name = name;
            _maze = new(maze);
            _random = new Random();
            _moveRandom = moveRandom;
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

        public void MakeTurn()
        {
            var status = VerifyIfFinishIsReachable();
            if (status)
                return;

            if (_moveRandom)
            {
                MoveRandomly();
                return;
            }

            Move();

            if (_isInDeadEnd)
            {
                MoveData moveDataDeadEnd = new(OldX, OldY, X, Y, -1f);
                Broadcast(moveDataDeadEnd, true);
                OnAgentMoveEvent?.Invoke(moveDataDeadEnd);
            }
            MoveData moveData = new(X, Y, OldX, OldY, -0.1f);
            Broadcast(moveData, true);
            OnAgentMoveEvent?.Invoke(moveData);
        }

        private void Move()
        {
            List<Tuple<MoveDirection, float>> moveDirections = new();
            float maxWeight = 0f;
            if (CanMoveUp())
            {
                var w = (float)_maze.Cells[X, Y].UpWeight!;
                moveDirections.Add(new(MoveDirection.Up, w));
                if (w > maxWeight)
                    maxWeight = w;
            }
            if (CanMoveDown())
            {
                var w = (float)_maze.Cells[X, Y].DownWeight!;
                moveDirections.Add(new(MoveDirection.Down, w));
                if (w > maxWeight)
                    maxWeight = w;
            }
            if (CanMoveLeft())
            {
                var w = (float)_maze.Cells[X, Y].LeftWeight!;
                moveDirections.Add(new(MoveDirection.Left, w));
                if (w > maxWeight)
                    maxWeight = w;
            }
            if (CanMoveRight())
            {
                var w = (float)_maze.Cells[X, Y].RightWeight!;
                moveDirections.Add(new(MoveDirection.Right, w));
                if (w > maxWeight)
                    maxWeight = w;
            }

            var validMoveDirections = moveDirections
                .Where(x => x.Item2 >= maxWeight)
                .Where(x => x.Item1 != ComingDirection)
                .ToList();

            var validMoveDirections2 = moveDirections
                .Where(x => x.Item1 != ComingDirection)
                .ToList();

            if (validMoveDirections2.Count > 1)
            {
                _isInDeadEnd = false;
            }

            if (validMoveDirections.Count == 0)
            {
                validMoveDirections.Add(new((MoveDirection)ComingDirection!, 0));
                _isInDeadEnd = true;
            }

            var direction = validMoveDirections[_random.Next(validMoveDirections.Count)];
            
            switch (direction.Item1)
            {
                case MoveDirection.Up:
                    MoveUp();
                    break;
                case MoveDirection.Down:
                    MoveDown();
                    break;
                case MoveDirection.Left:
                    MoveLeft();
                    break;
                case MoveDirection.Right:
                    MoveRight();
                    break;
                default:
                    MoveRandomly();
                    break;
            }
        }

        private void MoveRandomly()
        {
            var status = VerifyIfFinishIsReachable();
            if (status)
                return;

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
        }

        private bool VerifyIfFinishIsReachable()
        {
            if (X < 1 || Y < 1 || X >= _maze.Width - 1 || Y >= _maze.Height - 1)
                return false;

            if (_maze.Cells[X + 1, Y].CellType == MazeCell.Exit)
            {
                OnFoundExitEvent?.Invoke(this);
                return true;
            }
            if (_maze.Cells[X - 1, Y].CellType == MazeCell.Exit)
            {
                OnFoundExitEvent?.Invoke(this);
                return true;
            }
            if (_maze.Cells[X, Y + 1].CellType == MazeCell.Exit)
            {
                OnFoundExitEvent?.Invoke(this);
                return true;
            }
            if (_maze.Cells[X, Y - 1].CellType == MazeCell.Exit)
            {
                OnFoundExitEvent?.Invoke(this);
                return true;
            }

            return false;
        }

        private bool MoveUp()
        {
            if (!CanMoveUp())
                return false;

            OldX = X;
            OldY = Y;
            Y--;
            return true;
        }

        private bool MoveDown()
        {
            if (!CanMoveDown())
                return false;

            OldX = X;
            OldY = Y;
            Y++;
            return true;
        }

        private bool MoveRight()
        {
            if (!CanMoveRight())
                return false;

            OldX = X;
            OldY = Y;
            X++;
            return true;
        }

        private bool MoveLeft()
        {
            if (!CanMoveLeft())
                return false;

            OldX = X;
            OldY = Y;
            X--;
            return true;
        }

        private bool CanMoveUp()
        {
            if (Y - 1 < 0)
                return false;
            if (_maze.Cells[X, Y - 1].CellType != (int)MazeCell.Path)
                return false;
            return true;
        }

        private bool CanMoveDown()
        {
            if (Y + 1 >= _maze.Cells.GetLength(1))
                return false;
            if (_maze.Cells[X, Y + 1].CellType != (int)MazeCell.Path)
                return false;
            return true;
        }

        private bool CanMoveRight()
        {
            if (X + 1 >= _maze.Cells.GetLength(0))
                return false;
            if (_maze.Cells[X + 1, Y].CellType != (int)MazeCell.Path)
                return false;
            return true;
        }

        private bool CanMoveLeft()
        {
            if (X - 1 < 0)
                return false;
            if (_maze.Cells[X - 1, Y].CellType != (int)MazeCell.Path)
                return false;
            return true;
        }

        public enum MoveDirection
        {
            Up, Down, Left, Right, None
        }
    }
}
