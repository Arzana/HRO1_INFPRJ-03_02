namespace OpenDataApplication
{
    using Core;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using GMap.NET.WindowsForms;
    using GMap.NET.MapProviders;
    using GMap.NET;
    using GMap.NET.WindowsForms.Markers;
    using Mentula.Utilities.Logging;

    public sealed partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeBaseMap();
            InitializeStationLayer();
        }

        private void InitializeBaseMap()
        {
            map.ShowCenter = false;
            map.MapProvider = BingMapProvider.Instance;
            map.Manager.Mode = AccessMode.ServerAndCache;
            map.SetPositionByKeywords("Rotterdam, Netherlands");
        }

        private void InitializeStationLayer()
        {
            GMapOverlay overlay = new GMapOverlay("Stations");
            List<Station> stations = CSVReader.GetStationsFromFile("..\\..\\..\\..\\Third-Party\\Data\\stations-nl-2015-08.csv");

            for (int i = 0; i < stations.Count; i++)
            {
                Station cur = stations[i];

                Log.Debug(nameof(stations), $"Adding station {cur.FriendlyName} at {cur.Position}");
                overlay.Markers.Add(new GMarkerGoogle(cur.Position, GMarkerGoogleType.arrow));
            }

            map.Overlays.Add(overlay);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            map.BoundsOfMap = map.GetRectOfAllMarkers("Stations");
            map.Zoom = 12;
        }
    }
}