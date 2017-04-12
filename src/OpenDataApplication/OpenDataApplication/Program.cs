namespace OpenDataApplication
{
    using Core;
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
                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new MainForm());
                string Stationname = "ut";
                string url = "https://webservices.ns.nl/ns-api-avt?station="+Stationname;
                var x = HttpHeader.webRequest(url);
                string y = HttpHeader.XmLcon(x);
                
            }
        }
    }
}