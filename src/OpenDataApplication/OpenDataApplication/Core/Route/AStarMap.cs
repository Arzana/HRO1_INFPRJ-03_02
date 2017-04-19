namespace OpenDataApplication.Core.Route
{
    using System.Collections.Generic;

    public sealed class AStarMap
    {
        public List<AStarNode> Nodes { get; private set; }
        public Vect2 Start { get; set; }
        public Vect2 End { get { return end; } set { end = value; SetHeuristic(); } }

        private Vect2 end;

        public AStarMap()
        {
            EmptyMap();
        }

        public void EmptyMap()
        {
            Nodes = new List<AStarNode>();
            Start = Vect2.InvOne;
            end = Vect2.InvOne;
        }

        public AStarNode GetStartNode()
        {
            return Nodes.Find(n => n.Position == Start);
        }

        public AStarNode GetEndNode()
        {
            return Nodes.Find(n => n.Position == end);
        }

        private void SetHeuristic()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].SetHeuristic(end);
            }
        }
    }
}