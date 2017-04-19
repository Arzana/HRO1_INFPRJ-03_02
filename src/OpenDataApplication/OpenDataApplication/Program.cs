namespace OpenDataApplication
{
    using DeJong.Utilities.Logging;
    using System;
    using System.Windows.Forms;

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            if (System.Diagnostics.Debugger.IsAttached) throw new InvalidOperationException("Do not run in an IDE!");
#if DEBUG
            bool release = false;
#else
            bool release = true;
#endif
            using (ConsoleLogger cl = new ConsoleLogger(suppressConsoleResize: release) { AutoUpdate = true })
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}