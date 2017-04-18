namespace OpenDataApplication.Core.Route
{
    public sealed class AStarMap
    {
        public AStarNode[] Nodes { get; private set; }
        public Vect2 Start { get; set; }
        public Vect2 End { get { return end; } set { end = value; SetHeuristic(); } }

        private int w, h;
        private Vect2 end;

        public AStarNode this[float x, float y]
        {
            get
            {
                return Nodes[(int)(y * w + x)];
            }
            set
            {
                Nodes[(int)(y * w + x)] = value;
            }
        }

        public AStarMap(int width, int height)
        {
            w = width;
            h = height;
            EmptyMap();
        }

        public void EmptyMap()
        {
            Nodes = new AStarNode[w * h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Nodes[y * w + x] = new AStarNode(new Vect2(x, y));
                }
            }

            Start = Vect2.InvOne;
            end = Vect2.InvOne;
        }

        public AStarNode GetStartNode()
        {
            return this[Start.X, Start.Y];
        }

        public AStarNode GetEndNode()
        {
            return this[end.X, end.Y];
        }

        private void SetHeuristic()
        {
            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].SetHeuristic(end);
            }
        }
    }
}