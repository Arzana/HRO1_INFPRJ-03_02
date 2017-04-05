namespace OpenDataApplication
{
    using Core;
    using Mentula.Utilities.Logging;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            ConsoleLogger cl = new ConsoleLogger(suppressConsoleResize: true) { AutoUpdate = true };
            List<Station> test = CSVReader.GetStationsFromFile("..\\..\\..\\..\\Third-Party\\Data\\stations-nl-2015-08.csv");
            Log.Info(nameof(Program), $"File contains {test.Count} readable entries");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}