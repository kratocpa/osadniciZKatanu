using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class Edge : ICloneable
    {
        public Tuple<Coord, Coord> Coordinate { get; protected set; } // souřadnice cesty
        public Coord CentreCoordinate { get; protected set; } // souřadnice středu cesty
        public bool Road { get; protected set; } // true - je zde cesta, false - není
        public Game.color Color { get; protected set; } // barva cesty, pokud zde nějaká je, jinak noColor
        public int ID { get; set; } // ID hrany

        public List<Edge> EdgeNeighbors { get; private set; } // seznam hran, které sousedí s touto hranou
        public List<Vertex> VertexNeighbors { get; private set; } // seznam vrcholů které sousedí s touto hranou (vždy právě 2)

        public void SetRoad(Game.color playerColor_) { Color = playerColor_; Road = true; }

        public Edge(Tuple<Coord, Coord> edgeCoordinate)
        {
            Coordinate = edgeCoordinate;
            CentreCoordinate = new Coord(Coordinate.Item2.X + (Coordinate.Item1.X - Coordinate.Item2.X) / 2,
                                         Coordinate.Item2.Y + (Coordinate.Item1.Y - Coordinate.Item2.Y) / 2);
            Road = false;
            Color = Game.color.noColor;

            EdgeNeighbors = new List<Edge>();
            VertexNeighbors = new List<Vertex>();
        }

        public void AddEdgeNeighbors(List<Edge> addedEdge)
        {
            EdgeNeighbors.Clear();
            foreach (Edge curEg in addedEdge)
            {
                EdgeNeighbors.Add(curEg);
            }
        }

        public void AddVertexNeighbors(List<Vertex> addedVertices)
        {
            VertexNeighbors.Clear();
            foreach (Vertex curVx in addedVertices)
            {
                VertexNeighbors.Add(curVx);
            }
        }

        public bool IsHereRoadWithColor(Game.color playerColor)
        {
            return Road && playerColor == Color;
        }

        public bool IsHereAdjacentRoadWithColor(Game.color playerColor)
        {
            bool succes = false;
            foreach (var curEg in EdgeNeighbors)
            {
                succes = succes || curEg.Color == playerColor;
            }
            return succes;
        }

        public bool IsHereAdjectedVillageWithColor(Game.color playerColor)
        {
            bool succes = false;
            foreach (var curVx in VertexNeighbors)
            {
                succes = succes || curVx.Color == playerColor;
            }
            return succes;
        }

        public Boolean IsEqualTo(Edge scEg)
        {
            return (Coordinate.Item1.X == scEg.Coordinate.Item1.X &&
                Coordinate.Item1.Y == scEg.Coordinate.Item1.Y &&
                Coordinate.Item2.X == scEg.Coordinate.Item2.X &&
                Coordinate.Item2.Y == scEg.Coordinate.Item2.Y);
        }

        public object Clone()
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
