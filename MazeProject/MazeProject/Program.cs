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
            var maze = MazeGenerator.GenerateMaze(52, 40, out int width, out int height);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(maze[j, i] == 0 ? "  " : "██");
                }
                Console.WriteLine();
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

        }
    }
}