using GMap.NET;
using System.Collections.Generic;

namespace OpenDataApplication.Core.Route
{
    public sealed class AStarNode
    {
        public object Id { get; set; }
        public Vect2 Position { get; private set; }
        public int Heuristic { get; private set; }

        public int FValue { get { return GValue + Heuristic; } }
        public bool HasParent { get { return parent != null; } }
        public AStarNode Parent { get { return parent; } }

        public bool Blocked { get; set; }
        public int GValue { get; set; }

        public List<AStarNode> Adjason { get; set; }

        private AStarNode parent;

        public AStarNode(PointLatLng position)
        {
            Position = new Vect2(position.Lat, position.Lng);
            Adjason = new List<AStarNode>();
        }

        public void SetHeuristic(Vect2 endPoint)
        {
            Heuristic = (int)Vect2.Dist(Position, endPoint);
        }

        public void SetParent(AStarNode parent, int moveCost)
        {
            GValue += parent.GValue + moveCost;
            this.parent = parent;
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}