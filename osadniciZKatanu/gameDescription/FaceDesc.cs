using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class FaceDesc : ICloneable
    {
        public Coord Coordinate { get; protected set; } // souřadnice středu stěny
        public Game.materials Material { get; protected set; } // surovina na stěně
        public int ProbabilityNumber { get; protected set; } // číslo stěny (pokud padne toto číslo na kostkách, tak stěna produkuje surovinu)
        public bool Thief { get; set; } // true - je na stěně zloděj, false - není
        public List<VertexDesc> VerticesNeighborsDesc { get; protected set; } // seznam vrcholů které sousedí se stěnou
        public int ID { get; set; }

        public FaceDesc(Coord faceCoordinate, Game.materials faceMaterial, int probabilityNumber)
        {
            Coordinate = faceCoordinate;
            Material = faceMaterial;
            ProbabilityNumber = probabilityNumber;
            VerticesNeighborsDesc = new List<VertexDesc>();
            if (Material == Game.materials.desert) { Thief = true; }
            else Thief = false;
        }

        public virtual object Clone()
        {
            FaceDesc newFc = new FaceDesc(Coordinate, Material, ProbabilityNumber);

            newFc.Coordinate = Coordinate;
            newFc.Material = Material;
            newFc.ProbabilityNumber = ProbabilityNumber;
            newFc.Thief = Thief;
            newFc.ID = ID;

            return newFc;
        }
    }
}
