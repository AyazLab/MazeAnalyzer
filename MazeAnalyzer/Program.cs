using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MazeLib;

namespace MazeAnalyzer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                SplashScreen.ShowSplashScreen<Splash>();
                SplashScreen.SetStatus("Initializing...");

                System.Threading.Thread.Sleep(1500);
            }
            catch
            {
            }
            if (args.Length == 0)
                Application.Run(new Main());
            else
                Application.Run(new Main(args[0]));  
        }
    }
}
