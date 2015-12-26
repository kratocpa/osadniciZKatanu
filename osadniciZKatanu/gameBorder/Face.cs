using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class Face : FaceDesc
    {
        public List<Vertex> VerticesNeighbors { get; private set; }

        public void SetMaterial(Game.materials changedMaterial) { Material = changedMaterial; }

        public Face(Coord faceCoordinate_, Game.materials faceMaterial_, int probabilityNumber_)
            : base(faceCoordinate_, faceMaterial_, probabilityNumber_)
        {
            VerticesNeighbors = new List<Vertex>();
        }

        public void AddVertexNeighbors(List<Vertex> addedVertex)
        {
            VerticesNeighbors.Clear();
            foreach (Vertex curVx in addedVertex)
            {
                VerticesNeighbors.Add(curVx);
                base.VerticesNeighborsDesc.Add(curVx);
            }
        }

        public override object Clone()
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
