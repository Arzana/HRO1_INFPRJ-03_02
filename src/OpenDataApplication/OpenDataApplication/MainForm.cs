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
            InitializeStopLayer();
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
                GMarkerGoogleType markerStyle;
                switch (cur.Type)
                {
                    case StationType.knooppuntstoptreinstation:
                    case StationType.stoptreinstation:
                        markerStyle = GMarkerGoogleType.green;
                        break;
                    case StationType.sneltreinstation:
                    case StationType.intercitystation:
                    case StationType.knooppuntsneltreinstation:
                    case StationType.knooppuntintercitystation:
                        markerStyle = GMarkerGoogleType.yellow;
                        break;
                    case StationType.megastation:
                        markerStyle = GMarkerGoogleType.orange;
                        break;
                    case StationType.facultatiefstation:
                    case StationType.none:
                    default:
                        markerStyle = GMarkerGoogleType.red;
                        break;
                }

                Log.Debug(nameof(stations), $"Adding station {cur.FriendlyName} at {cur.Position}");
                overlay.Markers.Add(new GMarkerGoogle(cur.Position, markerStyle));
            }

            map.Overlays.Add(overlay);
        }

        private void InitializeStopLayer()
        {
            GMapOverlay overlay = new GMapOverlay("Stops");
            List<Stop> stops = CSVReader.GetStopsFromFile("..\\..\\..\\..\\Third-Party\\Data\\RET-haltebestand.csv");

            for (int i = 0; i < stops.Count; i++)
            {
                Stop cur = stops[i];
                GMarkerGoogleType markerStyle;
                if (cur.Description.ToUpper().Contains("TRAM")) markerStyle = GMarkerGoogleType.blue;
                else if (cur.Description.ToUpper().Contains("BUS")) markerStyle = GMarkerGoogleType.purple;
                else if (cur.Description.ToUpper().Contains("METRO")) markerStyle = GMarkerGoogleType.pink;
                else markerStyle = GMarkerGoogleType.black_small;

                Log.Debug(nameof(stops), $"Adding stop {cur.Name} at {cur.Position}");
                overlay.Markers.Add(new GMarkerGoogle(cur.Position, markerStyle));
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