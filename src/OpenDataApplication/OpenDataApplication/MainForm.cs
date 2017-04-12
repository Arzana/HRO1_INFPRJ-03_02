namespace OpenDataApplication
{
    using Core;
    using Core.DataTypes;
    using GMap.NET;
    using GMap.NET.MapProviders;
    using GMap.NET.WindowsForms;
    using Markers;
    using Mentula.Utilities.Logging;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public sealed partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeBaseMap();

            InitializeStationLayer();   // TODO: run on background thread.
            InitializeStopLayer();      // TODO: run on background thread.
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
            List<Station> stations = CSVReader.GetStationsFromFile($"stations-nl-2015-08.csv");

            Log.Info(nameof(stations), $"Starting adding {stations.Count} station markers");
            for (int i = 0; i < stations.Count; i++)
            {
                Station cur = stations[i];
                Log.Debug(nameof(stations), $"Adding station {cur.FriendlyName}");
                overlay.Markers.Add(new NSMarker(cur.Position));
            }
            Log.Info(nameof(stations), $"Finished adding station markers");

            map.Overlays.Add(overlay);
        }

        private void InitializeStopLayer()
        {
            GMapOverlay overlay = new GMapOverlay("Stops");
            List<Stop> stops = CSVReader.GetStopsFromFile($"RET-haltebestand.csv");

            Log.Info(nameof(stops), $"Started adding {stops.Count} stop markers");
            for (int i = 0; i < stops.Count; i++)
            {
                Stop cur = stops[i];
                Log.Debug(nameof(stops), $"Adding stop {cur.Name}");
                overlay.Markers.Add(new RETMarker(cur));
            }
            Log.Info(nameof(stops), $"Finished adding stop markers");

            map.Overlays.Add(overlay);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            map.BoundsOfMap = map.GetRectOfAllMarkers("Stations");
            map.Zoom = 12;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}