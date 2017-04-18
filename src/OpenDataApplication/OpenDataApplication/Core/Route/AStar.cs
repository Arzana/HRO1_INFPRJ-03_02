﻿using System.Collections.Generic;

namespace OpenDataApplication.Core.Route
{
    public static class AStar
    {
        public static List<AStarNode> GetRoute(AStarMap map)
        {
            List<AStarNode> open = new List<AStarNode>(map.Nodes.Length);
            List<AStarNode> closed = new List<AStarNode>(map.Nodes.Length);

            AStarNode cur = map.GetStartNode();
            while (true)
            {
                if (open.Contains(cur)) open.Remove(cur);
                closed.Add(cur);

                if (cur.Adjason.Contains(map.GetEndNode())) break;

                cur.Adjason.RemoveAll(n => closed.Contains(n) || n.Blocked);
                for (int i = 0; i < cur.Adjason.Count; i++)
                {
                    AStarNode neighbour = cur.Adjason[i];
                    int moveCost = (int)Vect2.Dist(cur.Position, neighbour.Position);
                    if (!neighbour.HasParent) cur.SetParent(cur, moveCost);
                    if (cur.GValue + moveCost < neighbour.GValue) cur.SetParent(cur, moveCost);
                    if (!open.Contains(neighbour)) open.Add(neighbour);
                }

                if (open.Count == 0) return new List<AStarNode>();

                AStarNode min = open[0];
                for (int i = 0; i < open.Count; i++)
                {
                    if (min != open[i] && open[i].FValue < min.FValue) min = open[i];
                }

                cur = min;
            }

            AStarNode end = map.GetEndNode();
            end.SetParent(cur, (int)Vect2.Dist(cur.Position, end.Position));

            List<AStarNode> result = new List<AStarNode>();
            cur = end;

            while (cur.HasParent)
            {
                result.Add(cur);
                cur = cur.Parent;
            }

            result.RemoveAt(0);
            return result;
        }
    }
}