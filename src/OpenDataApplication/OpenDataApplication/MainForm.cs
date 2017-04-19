namespace OpenDataApplication
{
    using Core;
    using Core.DataTypes;
    using GMap.NET;
    using GMap.NET.MapProviders;
    using GMap.NET.WindowsForms;
    using Markers;
    using DeJong.Utilities.Logging;
    using System;
    using System.Drawing;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Linq;
    using Core.Route;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Diagnostics;

    public sealed partial class MainForm : Form
    {
        private AStarMap map_all;
        private List<string> stationsCodesRotterdam;
        List<Station> stations;
        List<Stop> stops;

        public MainForm()
        {
            stationsCodesRotterdam = new List<string>();

            InitializeComponent();
            InitializeBaseMap();

            InitializeStationLayer();
            InitializeStopLayer();
            InitializeAStartMaps();
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
            stations = CSVReader.GetStationsFromFile($"stations-nl-2015-08.csv");

            Log.Info(nameof(stations), $"Starting adding {stations.Count} station markers");
            for (int i = 0; i < stations.Count; i++)
            {
                Station cur = stations[i];
                if (cur.FullName.ToUpper().Contains("ROTTERDAM") && (!comboBox1.Items.Contains(cur)))
                {
                    Log.Debug(nameof(stations), $"Adding station {cur.FriendlyName}");
                    overlay.Markers.Add(new NSMarker(cur.Position));
                    comboBox1.Items.Add(cur);
                    comboBox2.Items.Add(cur);
                    stationsCodesRotterdam.Add(cur.Code);
                }
            }
            Log.Info(nameof(stations), $"Finished adding station markers");

            map.Overlays.Add(overlay);
        }

        private void InitializeStopLayer()
        {
            GMapOverlay Busoverlay = new GMapOverlay("BusStops");
            GMapOverlay Tramoverlay = new GMapOverlay("TramStops");
            GMapOverlay Metrooverlay = new GMapOverlay("MetroStops");
            stops = CSVReader.GetStopsFromFile($"RET-haltebestand.csv");
            List<Stop> busstops = new List<Stop>();
            List<Stop> tramstops = new List<Stop>();
            List<Stop> metrostops = new List<Stop>();


            for (int i = 0; i < stops.Count; i++)
            {
                if (stops[i].Type == StopType.Bus)
                {
                    busstops.Add(stops[i]);
                }
                else if (stops[i].Type == StopType.Tram)
                {
                    tramstops.Add(stops[i]);
                }
                else if (stops[i].Type == StopType.Metro)
                {
                    metrostops.Add(stops[i]);
                }
            }

            Log.Info(nameof(stops), $"Started adding {stops.Count} stop markers");
            for (int i = 0; i < busstops.Count; i++)
            {
                Stop cur = busstops[i];
                if (!comboBox1.Items.Contains(cur))
                {
                    Log.Debug(nameof(stops), $"Adding stop {cur.Name}");
                    Busoverlay.Markers.Add(new RETMarker(cur));
                    comboBox1.Items.Add(cur);
                    comboBox2.Items.Add(cur);
                }
            }

            for (int i = 0; i < tramstops.Count; i++)
            {
                Stop cur = tramstops[i];
                if (!comboBox1.Items.Contains(cur))
                {
                    Log.Debug(nameof(stops), $"Adding stop {cur.Name}");
                    Tramoverlay.Markers.Add(new RETMarker(cur));
                    comboBox1.Items.Add(cur);
                    comboBox2.Items.Add(cur);
                }
            }

            for (int i = 0; i < metrostops.Count; i++)
            {
                Stop cur = metrostops[i];
                if (!comboBox1.Items.Contains(cur))
                {
                    Log.Debug(nameof(stops), $"Adding stop {cur.Name}");
                    Metrooverlay.Markers.Add(new RETMarker(cur));
                    comboBox1.Items.Add(cur);
                    comboBox2.Items.Add(cur);
                }
            }
            Log.Info(nameof(stops), $"Finished adding stop markers");

            map.Overlays.Add(Busoverlay);
            map.Overlays.Add(Tramoverlay);
            map.Overlays.Add(Metrooverlay);
        }

        private void InitializeAStartMaps()
        {
            map_all = new AStarMap();
            Log.Verbose(nameof(AStar), "Started generating map");
            Stopwatch sw = Stopwatch.StartNew();

            for (int i = 0; i < stationsCodesRotterdam.Count; i++)
            {
                Station cur = stations.Find(s => s.Code == stationsCodesRotterdam[i]);
                AStarNode node = new AStarNode(cur.Position) { Id = cur };
                map_all.Nodes.Add(node);
            }

            for (int i = 0; i < stops.Count; i++)
            {
                Stop cur = stops[i];
                AStarNode node = new AStarNode(cur.Position) { Id = cur };
                map_all.Nodes.Add(node);
            }

            List<RETRoute> retRoutes = PASReader.ReadRoutesFromFile("RET.PAS");
            List<RetStop> retStops = CSVReader.GetRetStopsFromFile("RET.HLT");
            for (int i = 0; i < retRoutes.Count; i++)
            {
                RETRoute cur = retRoutes[i];
                Log.Debug(nameof(AStar), $"Adding route: {cur}");
                for (int j = 0; j < cur.Stops.Count; j++)
                {
                    AStarNode curNode;
                    if (!TryGetNode(retStops, cur, j, out curNode)) continue;

                    if (j > 0)
                    {
                        AStarNode prevNode;
                        if (!TryGetNode(retStops, cur, j - 1, out prevNode)) continue;
                        if (!curNode.Adjason.Contains(prevNode)) curNode.Adjason.Add(prevNode);
                    }

                    if (j < cur.Stops.Count - 1)
                    {
                        AStarNode nextNode;
                        if (!TryGetNode(retStops, cur, j + 1, out nextNode)) continue;
                        if (!curNode.Adjason.Contains(nextNode)) curNode.Adjason.Add(nextNode);
                    }
                }
            }

            map_all.Nodes.RemoveAll(n => n.Adjason.Count == 0);
            sw.Stop();
            Log.Verbose(nameof(AStar), $"Finished generating map, took: {sw.Elapsed}");
        }

        private bool TryGetNode(List<RetStop> retStops, RETRoute cur, int j, out AStarNode result)
        {
            try
            {
                RetStop curRetStop = retStops.Find(rs => rs.Code == cur.Stops[j].Id);
                Stop nodeId = stops.Find(s => s.Name == curRetStop.Name);
                result = map_all.Nodes.Find(n => n.Id == nodeId);
                return result != null;
            }
            catch
            {
                result = null;
                return false;
            }
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
            PointLatLng Stopclosest = new PointLatLng(0, 0);
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
                if (InnerdifCoor < StopdifCoor)
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
            map.BoundsOfMap = map.GetRectOfAllMarkers("BusStops");
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
            map.Overlays.First(o => o.Id == "MetroStops").IsVisibile = Metro.Checked;
        }

        private void Tram_CheckedChanged(object sender, EventArgs e)
        {
            //Tram
            map.Overlays.First(o => o.Id == "TramStops").IsVisibile = Tram.Checked;
        }

        private void Bus_CheckedChanged(object sender, EventArgs e)
        {
            //Bus
            map.Overlays.First(o => o.Id == "BusStops").IsVisibile = Bus.Checked;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Help_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stop start = comboBox1.SelectedItem as Stop;
            Stop end = comboBox2.SelectedItem as Stop;

            label3.Text = string.Empty;
            if (map_all.Nodes.Find(n => n.Position == new Vect2(start.Position)) == null) label3.Text = "Start not in routes";
            if (map_all.Nodes.Find(n => n.Position == new Vect2(end.Position)) == null) label3.Text = "End not in routes";
            if (!string.IsNullOrEmpty(label3.Text)) return;

            map_all.Start = new Vect2(start.Position);
            map_all.End = new Vect2(end.Position);

            List<AStarNode> result = AStar.GetRoute(map_all);
            StringBuilder sb = new StringBuilder();
            sb.Append("Route length: ");
            sb.Append(result.Count);
            sb.AppendLine(Environment.NewLine);
            for (int i = 0; i < result.Count; i++)
            {
                sb.Append(i);
                sb.Append(": ");
                sb.Append(result[i].Id);
                sb.Append(Environment.NewLine);
            }
            label3.Text = sb.ToString();
        }
    }
}