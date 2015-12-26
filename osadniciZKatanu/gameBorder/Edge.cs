using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class Edge : EdgeDesc
    {
        public List<Edge> EdgeNeighbors { get; private set; }
        public List<Vertex> VertexNeighbors { get; private set; }

        public void SetRoad(Game.color playerColor_) { Color = playerColor_; Road = true; }

        public Edge(Tuple<Coord, Coord> edgeCoordinate_)
            : base(edgeCoordinate_)
        {
            EdgeNeighbors = new List<Edge>();
            VertexNeighbors = new List<Vertex>();
        }

        public void AddEdgeNeighbors(List<Edge> addedEdge)
        {
            EdgeNeighbors.Clear();
            foreach (Edge curEg in addedEdge)
            {
                EdgeNeighbors.Add(curEg);
                base.EdgeNeighborsDesc.Add(curEg);
            }
        }

        public void AddVertexNeighbors(List<Vertex> addedVertices)
        {
            VertexNeighbors.Clear();
            foreach (Vertex curVx in addedVertices)
            {
                VertexNeighbors.Add(curVx);
                base.VertexNeighborsDesc.Add(curVx);
            }
        }

        public override object Clone()
        {
            Edge newEg = new Edge(Coordinate);

            newEg.Coordinate = Coordinate;
            newEg.CentreCoordinate = CentreCoordinate;
            newEg.Road = Road;
            newEg.Color = Color;
            newEg.ID = ID;

            return newEg;
        }
    }
}
