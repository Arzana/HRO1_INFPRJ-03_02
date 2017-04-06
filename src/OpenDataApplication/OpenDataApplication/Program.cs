namespace OpenDataApplication
{
    using Mentula.Utilities.Logging;
    using System;
    using System.Windows.Forms;

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (ConsoleLogger cl = new ConsoleLogger { AutoUpdate = true })
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}