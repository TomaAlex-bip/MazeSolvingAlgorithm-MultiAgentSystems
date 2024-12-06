namespace MazeProject.Data
{
    public class SimulationResults
    {
        public int SimulationId { get; set; }
        public int NoTurns { get; set; }
        public int MazeSeed { get; set; }
        public int NoAgents { get; set; }
        public int MazeWidth { get; set; }
        public int MazeHeight { get; set; }

        public override string ToString()
        {
            return $"[{SimulationId}] Turns[{NoTurns}] \t Agents[{NoAgents}] \t Maze[W:{MazeWidth} H:{MazeHeight} - {MazeSeed}]";
        }
    }
}
