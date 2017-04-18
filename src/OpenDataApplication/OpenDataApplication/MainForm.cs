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
    using System.Drawing;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Linq;

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
                if (cur.FullName.ToUpper().Contains("ROTTERDAM") && ( !comboBox1.Items.Contains(cur)))
                {
                    Log.Debug(nameof(stations), $"Adding station {cur.FriendlyName}");
                    overlay.Markers.Add(new NSMarker(cur.Position));
                    comboBox1.Items.Add(cur);
                    comboBox2.Items.Add(cur);
                }
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
                if (!comboBox1.Items.Contains(cur))
                {
                    Log.Debug(nameof(stops), $"Adding stop {cur.Name}");
                    overlay.Markers.Add(new RETMarker(cur));
                    comboBox1.Items.Add(cur);
                    comboBox2.Items.Add(cur);
                }
            }
            Log.Info(nameof(stops), $"Finished adding stop markers");
                        
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

            List<Station> stations = CSVReader.GetStationsFromFile($"stations-nl-2015-08.csv");
            List<Stop> stops = CSVReader.GetStopsFromFile($"RET-haltebestand.csv");
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Station selectedStation = (sender as ComboBox)?.SelectedItem as Station;
            if (selectedStation != null)
            {
                label3.Text = selectedStation.Type.ToString();
                map.Position = selectedStation.Position;
            }
            else
            {
                Stop selectedStop = (sender as ComboBox)?.SelectedItem as Stop;
                if (selectedStop != null)
                {
                    label3.Text = selectedStop.Description;
                    map.Position = selectedStop.Position;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Station selectedStation = (sender as ComboBox)?.SelectedItem as Station;
            if (selectedStation != null)
            {
                label3.Text = selectedStation.Type.ToString();
                map.Position = selectedStation.Position;
            }
            else
            {
                Stop selectedStop = (sender as ComboBox)?.SelectedItem as Stop;
                if (selectedStop != null)
                {
                    label3.Text = selectedStop.Description;
                    map.Position = selectedStop.Position;
                }
            }
        }

        private void Trein_CheckedChanged(object sender, EventArgs e)
        {
            //Trein
            map.Overlays.First(o => o.Id == "Stations").IsVisibile = Trein.Checked;
        }

        private void Metro_CheckedChanged(object sender, EventArgs e)
        {
            //Metro

        }

        private void Tram_CheckedChanged(object sender, EventArgs e)
        {
            //Tram

        }

        private void Bus_CheckedChanged(object sender, EventArgs e)
        {
            //Bus

        }
    }
}