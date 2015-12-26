using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class VertexDesc : ICloneable
    {
        public Coord Coordinate { get; protected set; } // souřadnice vrcholu
        public bool Village { get; protected set; } // true - je zde vesnice, false - není
        public bool Town { get; protected set; } // true - je zde město, false - není
        public bool Building { get { return Village || Town; } } // true - je zde město nebo vesnice, false - není
        public bool Port { get; protected set; } // true - je zde port, false - není
        public Game.materials PortMaterial { get; protected set; } // pokud je hodnota noMaterial, pak je zde univerzální port
        public Game.color Color { get; protected set; } // barva vesnice nebo města které je zde postavené (jinak noColor)
        public List<VertexDesc> VerticesNeighborsDesc { get; protected set; } // seznam vrcholů, který přimo sousedí s tímto
        public List<EdgeDesc> EdgeNeighborsDesc { get; protected set; } // seznam hran, které sousední s tímto vrcholem
        public List<FaceDesc> FaceNeighborsDesc { get; protected set; } // seznam stěn, které sousedí s vrcholem
        public int ID { get; set; }

        public VertexDesc(Coord vertexCoordinate_)
        {
            Village = false;
            Town = false;
            Color = Game.color.noColor;
            Coordinate = vertexCoordinate_;
            VerticesNeighborsDesc = new List<VertexDesc>();
            EdgeNeighborsDesc = new List<EdgeDesc>();
            FaceNeighborsDesc = new List<FaceDesc>();

            Port = false;
            PortMaterial = Game.materials.noMaterial;
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
            foreach (VertexDesc curVx in VerticesNeighborsDesc)
            {
                succes = succes || curVx.Building;
            }
            return succes;
        }

        public bool IsHereAdjectedRoadWithColor(Game.color roadColor)
        {
            bool succes = false;
            foreach (EdgeDesc curEg in EdgeNeighborsDesc)
            {
                succes = succes || curEg.Color == roadColor;
            }
            return succes;
        }

        public bool IsFreePlaceForVillage()
        {
            return !Building && !IsHereBuildingInNeighbour();
        }


        public virtual object Clone()
        {
            VertexDesc newVx = new VertexDesc(Coordinate);

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
