using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class Vertex : VertexDesc, ICloneable
    {
        public List<Vertex> VerticesNeighbors { get; private set; }
        public List<Edge> EdgeNeighbors { get; private set; }
        public List<Face> FaceNeighbors { get; private set; }

        public void addPort(Game.materials portMaterial_)
        {
            PortMaterial = portMaterial_;
            Port = true;
        }

        public void SetVillage(Game.color playerColor_) { Color = playerColor_; Village = true; }

        public void SetTown(Game.color playerColor_) { Color = playerColor_; Town = true; Village = false; }

        public Vertex(Coord vertexCoordinate_)
            : base(vertexCoordinate_)
        {
            VerticesNeighbors = new List<Vertex>();
            EdgeNeighbors = new List<Edge>();
            FaceNeighbors = new List<Face>();
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

        public void AddEdgeNeighbors(List<Edge> addedEdge)
        {
            EdgeNeighbors.Clear();
            foreach (Edge curEg in addedEdge)
            {
                EdgeNeighbors.Add(curEg);
                base.EdgeNeighborsDesc.Add(curEg);
            }
        }

        public void AddFaceNeighbors(List<Face> addedFace)
        {
            FaceNeighbors.Clear();
            foreach (Face curFc in addedFace)
            {
                FaceNeighbors.Add(curFc);
                base.FaceNeighborsDesc.Add(curFc);
            }
        }

        public override object Clone()
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
