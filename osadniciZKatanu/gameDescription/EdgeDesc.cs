using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class EdgeDesc : ICloneable
    {
        public Tuple<Coord, Coord> Coordinate { get; protected set; } // souřadnice cesty
        public Coord CentreCoordinate { get; protected set; } // souřadnice středu cesty
        public List<EdgeDesc> EdgeNeighborsDesc { get; protected set; } // seznam sousedících cest
        public List<VertexDesc> VertexNeighborsDesc { get; protected set; } // seznam sousedících vrcholů (vždy jsou dva)
        public bool Road { get; protected set; } // true - je zde cesta, false - není
        public Game.color Color { get; protected set; } // barva cesty, pokud zde nějaká je, jinak noColor
        public int ID { get; set; }

        public EdgeDesc(Tuple<Coord, Coord> edgeCoordinate)
        {
            Coordinate = edgeCoordinate;
            CentreCoordinate = new Coord(Coordinate.Item2.X + (Coordinate.Item1.X - Coordinate.Item2.X) / 2,
                                         Coordinate.Item2.Y + (Coordinate.Item1.Y - Coordinate.Item2.Y) / 2);
            Road = false;
            Color = Game.color.noColor;
            EdgeNeighborsDesc = new List<EdgeDesc>();
            VertexNeighborsDesc = new List<VertexDesc>();
        }

        public bool IsHereRoadWithColor(Game.color playerColor)
        {
            return Road && playerColor == Color;
        }

        public bool IsHereAdjacentRoadWithColor(Game.color playerColor)
        {
            bool succes = false;
            foreach (var curEg in EdgeNeighborsDesc)
            {
                succes = succes || curEg.Color == playerColor;
            }
            return succes;
        }

        public bool IsHereAdjectedVillageWithColor(Game.color playerColor)
        {
            bool succes = false;
            foreach (var curVx in VertexNeighborsDesc)
            {
                succes = succes || curVx.Color == playerColor;
            }
            return succes;
        }

        public Boolean IsEqualTo(EdgeDesc scEg)
        {
            return (Coordinate.Item1.X == scEg.Coordinate.Item1.X &&
                Coordinate.Item1.Y == scEg.Coordinate.Item1.Y &&
                Coordinate.Item2.X == scEg.Coordinate.Item2.X &&
                Coordinate.Item2.Y == scEg.Coordinate.Item2.Y);
        }

        public virtual object Clone()
        {
            EdgeDesc newEg = new EdgeDesc(Coordinate);

            newEg.Coordinate = Coordinate;
            newEg.CentreCoordinate = CentreCoordinate;
            newEg.Road = Road;
            newEg.Color = Color;
            newEg.ID = ID;

            return newEg;
        }
    }
}
