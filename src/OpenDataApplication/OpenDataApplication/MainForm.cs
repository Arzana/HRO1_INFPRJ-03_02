namespace OpenDataApplication
{
    using Core;
    using System;
    using System.Drawing;
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
                else if (cur.Description.ToUpper().Contains("FERRY")) markerStyle = GMarkerGoogleType.lightblue;
                else
                {
                    Log.Info(nameof(stops), $"Unhandled stattion type: {cur.Description}");
                    markerStyle = GMarkerGoogleType.black_small;
                }

                Log.Debug(nameof(stops), $"Adding stop {cur.Name} at {cur.Position}");
                overlay.Markers.Add(new GMarkerGoogle(cur.Position, markerStyle));
            }

            map.Overlays.Add(overlay);
        }

        private List<PointLatLng> points = new List<PointLatLng>();
        private List<PointLatLng> Gpoints = new List<PointLatLng>();

        private GMapOverlay InitializePolygonLayer(PointLatLng point, GMapOverlay PolyOverlay)
        {           
            overlay.Polygons.Clear();
            map.Overlays.Remove(PolyOverlay);
            points.Clear();
            Gpoints.Clear();

            List<Station> stations = CSVReader.GetStationsFromFile("..\\..\\..\\..\\Third-Party\\Data\\stations-nl-2015-08.csv");
            List<Stop> stops = CSVReader.GetStopsFromFile("..\\..\\..\\..\\Third-Party\\Data\\RET-haltebestand.csv");
            PointLatLng Stopclosest = new PointLatLng(0,0);
            double StopdifCoor = 1000000;
            PointLatLng Stationclosest = new PointLatLng(0, 0);
            double StationdifCoor = 1000000;
            PointLatLng Drawer = new PointLatLng(0, 0);

            

            for (int i = 0; i < stops.Count; i++)
            {
                Stop cur = stops[i];
                double difLat = Math.Abs(cur.Position.Lat - point.Lat);
                double difLng = Math.Abs(cur.Position.Lng - point.Lng);
                double InnerdifCoor = difLng + difLat;
                if(InnerdifCoor < StopdifCoor)
                {
                    StopdifCoor = InnerdifCoor;
                    InnerdifCoor = 0;
                    Stopclosest = new PointLatLng(cur.Position.Lat, cur.Position.Lng);
                }
            }

            for (int i = 0; i < stations.Count; i++)
            {
                Station cur = stations[i];
                double difLat = Math.Abs(cur.Position.Lat - point.Lat);
                double difLng = Math.Abs(cur.Position.Lng - point.Lng);
                double InnerdifCoor = difLng + difLat;
                if (InnerdifCoor < StationdifCoor)
                {
                    StationdifCoor = InnerdifCoor;
                    InnerdifCoor = 0;
                    Stationclosest = new PointLatLng(cur.Position.Lat, cur.Position.Lng);
                }
            }

            if (StationdifCoor > StopdifCoor)
            {
                Drawer = Stopclosest;
            }
            else if (StationdifCoor < StopdifCoor)
            {
                Drawer = Stationclosest;
            }

            double radius = 0.01;
            double Gradius = 0.005;
            double seg = Math.PI * 2 / (radius * 10000);

            for (int i = 0; i < 100; i++)
            {
                double theta = seg * i;
                double a = Drawer.Lat + Math.Cos(theta) * radius;
                double b = Drawer.Lng + Math.Sin(theta) * (radius * 1.7);

                PointLatLng drawpoint = new PointLatLng(a, b);

                points.Add(drawpoint);
            }

            for (int i = 0; i < 100; i++)
            {
                double theta = seg * i;
                double a = Drawer.Lat + Math.Cos(theta) * Gradius;
                double b = Drawer.Lng + Math.Sin(theta) * (Gradius * 1.7);

                PointLatLng Gdrawpoint = new PointLatLng(a, b);

                Gpoints.Add(Gdrawpoint);
            }

            GMapPolygon polygon = new GMapPolygon(points, "Region");
            polygon.Fill = new SolidBrush(Color.FromArgb(30, Color.Red));
            polygon.Stroke = new Pen(Color.FromArgb(50, Color.Red));
            overlay.Polygons.Add(polygon);

            GMapPolygon Gpolygon = new GMapPolygon(Gpoints, "Region");
            Gpolygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Green));
            Gpolygon.Stroke = new Pen(Color.FromArgb(50, Color.Green));
            overlay.Polygons.Add(Gpolygon);

            map.Overlays.Add(overlay);

            return overlay;

        }

        private GMapOverlay overlay = new GMapOverlay("Polygon");

        private void MouseClicker(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                PointLatLng quickpoint = new PointLatLng(-100.0, -100.0);
                overlay = InitializePolygonLayer(quickpoint, overlay);
                overlay.Polygons.Clear();
                map.Overlays.Remove(overlay);
                double lat = map.FromLocalToLatLng(e.X, e.Y).Lat;
                double lang = map.FromLocalToLatLng(e.X, e.Y).Lng;
                PointLatLng clickpoint = new PointLatLng(lat, lang);
                overlay = InitializePolygonLayer(clickpoint, overlay);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            map.BoundsOfMap = map.GetRectOfAllMarkers("Stations");
            map.Zoom = 12;
        }
    }
}