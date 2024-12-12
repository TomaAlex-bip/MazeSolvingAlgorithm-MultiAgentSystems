namespace MazeProject.Utils
{
    public class Cell
    {
        public MazeCell CellType { get; private set; }
        public float? UpWeight { get; set; }
        public float? DownWeight { get; set; }
        public float? RightWeight { get; set; }
        public float? LeftWeight { get; set; }

        public Cell(MazeCell mazeCell)
        {
            CellType = mazeCell;
        }

        public Cell(Cell cell)
        {
            CellType = cell.CellType;
            UpWeight = cell.UpWeight;
            DownWeight = cell.DownWeight;
            RightWeight = cell.RightWeight;
            LeftWeight = cell.LeftWeight;
        }

        // am ales ca ponderea maxima sa fie 0.5 doar din motiv ca exista o idee sa se marcheze si celulele care duc catre iesire
        // acele celule ar fi fost marcate cu 1, de asta am ales o valoare neutra (intre 0 si 1)
        internal void ResetWeights()
        {
            if (UpWeight != null)
                UpWeight = 0.5f;
            if (DownWeight != null)
                DownWeight = 0.5f;
            if (LeftWeight != null)
                LeftWeight = 0.5f;
            if (RightWeight != null)
                RightWeight = 0.5f;
        }
    }
}
