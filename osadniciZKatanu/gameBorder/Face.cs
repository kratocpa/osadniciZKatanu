using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class Face : ICloneable
    {
        public Coord Coordinate { get; protected set; } // souřadnice středu stěny
        public Game.materials Material { get; protected set; } // surovina na stěně
        public int ProbabilityNumber { get; protected set; } // číslo stěny (pokud padne toto číslo na kostkách, tak stěna produkuje surovinu)
        public bool Thief { get; set; } // true - je na stěně zloděj, false - není
        public int ID { get; set; } // ID stěny

        public List<Vertex> VerticesNeighbors { get; private set; } // seznam vrcholů které sousedí se stěnou (vždy jich je právě 6)

        public void SetMaterial(Game.materials changedMaterial) { Material = changedMaterial; }

        public Face(Coord faceCoordinate, Game.materials faceMaterial, int probabilityNumber)
        {
            Coordinate = faceCoordinate;
            Material = faceMaterial;
            ProbabilityNumber = probabilityNumber;
            if (Material == Game.materials.desert) { Thief = true; }
            else Thief = false;

            VerticesNeighbors = new List<Vertex>();
        }

        public void AddVertexNeighbors(List<Vertex> addedVertex)
        {
            VerticesNeighbors.Clear();
            foreach (Vertex curVx in addedVertex)
            {
                VerticesNeighbors.Add(curVx);
            }
        }

        public object Clone()
        {
            Face newFc = new Face(Coordinate, Material, ProbabilityNumber);

            newFc.Coordinate = Coordinate;
            newFc.Material = Material;
            newFc.ProbabilityNumber = ProbabilityNumber;
            newFc.Thief = Thief;
            newFc.ID = ID;

            return newFc;
        }
    }
}
