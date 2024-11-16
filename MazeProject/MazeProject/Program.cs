using MazeProject.Utils;

namespace MazeProject
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var maze = MazeGenerator.GenerateMaze(15, 15, out int width, out int height, 0);
            //for (int j = 0; j < height; j++)
            //{
            //    for (int i = 0; i < width; i++)
            //    {
            //        if (maze[i, j] == (int)MazeCell.Path)
            //            Console.Write("  ");
            //        if (maze[i, j] == (int)MazeCell.Wall)
            //            Console.Write("██");
            //        if (maze[i, j] == (int)MazeCell.Exit)
            //            Console.Write("XX");
            //        if (maze[i, j] == (int)MazeCell.Start)
            //            Console.Write("OO");

            //    }
            //    Console.WriteLine();
            //}

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

        }
    }
}