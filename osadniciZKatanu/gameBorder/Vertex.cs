using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class Vertex : ICloneable
    {
        public Coord Coordinate { get; protected set; } // souřadnice vrcholu
        public bool Village { get; protected set; } // true - je zde vesnice, false - není
        public bool Town { get; protected set; } // true - je zde město, false - není
        public bool Building { get { return Village || Town; } } // true - je zde město nebo vesnice, false - není
        public bool Port { get; protected set; } // true - je zde port, false - není
        public Game.materials PortMaterial { get; protected set; } // pokud je hodnota noMaterial, pak je zde univerzální port
        public Game.color Color { get; protected set; } // barva vesnice nebo města které je zde postavené (jinak noColor)
        public int ID { get; set; }

        public List<Vertex> VerticesNeighbors { get; private set; }
        public List<Edge> EdgeNeighbors { get; private set; }
        public List<Face> FaceNeighbors { get; private set; }

        public Vertex(Coord vertexCoordinate)
        {
            Village = false;
            Town = false;
            Color = Game.color.noColor;
            Coordinate = vertexCoordinate;

            Port = false;
            PortMaterial = Game.materials.noMaterial;

            VerticesNeighbors = new List<Vertex>();
            EdgeNeighbors = new List<Edge>();
            FaceNeighbors = new List<Face>();
        }

        public void addPort(Game.materials portMaterial_)
        {
            PortMaterial = portMaterial_;
            Port = true;
        }

        public void SetVillage(Game.color playerColor_) { Color = playerColor_; Village = true; }

        public void SetTown(Game.color playerColor_) { Color = playerColor_; Town = true; Village = false; }

        public void AddVertexNeighbors(List<Vertex> addedVertex)
        {
            VerticesNeighbors.Clear();
            foreach (Vertex curVx in addedVertex)
            {
                VerticesNeighbors.Add(curVx);
            }
        }

        public void AddEdgeNeighbors(List<Edge> addedEdge)
        {
            EdgeNeighbors.Clear();
            foreach (Edge curEg in addedEdge)
            {
                EdgeNeighbors.Add(curEg);
            }
        }

        public void AddFaceNeighbors(List<Face> addedFace)
        {
            FaceNeighbors.Clear();
            foreach (Face curFc in addedFace)
            {
                FaceNeighbors.Add(curFc);
            }
        }

        public bool IsHereVillage(Game.color playerColor)
        {
            return Village && Color == playerColor;
        }

        public bool IsHereTown(Game.color playerColor)
        {
            return Town && Color == playerColor;
        }

        public bool IsHereBuildingInNeighbour()
        {
            bool succes = false;
            foreach (Vertex curVx in VerticesNeighbors)
            {
                succes = succes || curVx.Building;
            }
            return succes;
        }

        public bool IsHereAdjectedRoadWithColor(Game.color roadColor)
        {
            bool succes = false;
            foreach (Edge curEg in EdgeNeighbors)
            {
                succes = succes || curEg.Color == roadColor;
            }
            return succes;
        }

        public bool IsFreePlaceForVillage()
        {
            return !Building && !IsHereBuildingInNeighbour();
        }

        public object Clone()
        {
            Vertex newVx = new Vertex(Coordinate);

            newVx.Village = Village;
            newVx.Town = Town;
            newVx.Port = Port;
            newVx.PortMaterial = PortMaterial;
            newVx.Color = Color;
            newVx.ID = ID;

            return newVx;
        }
    }
}
