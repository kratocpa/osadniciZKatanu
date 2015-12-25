using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class GameBorder : GameBorderDesc
    {
        public Vertex noVertex = new Vertex(new Coord(-1, -1));
        public Edge noEdge = new Edge(Tuple.Create(new Coord(-1, -1), new Coord(-1, -1)));
        public Face noFace = new Face(new Coord(-1, -1), GameDesc.materials.noMaterial, 0);

        public List<Vertex> Vertices;
        public List<Edge> Edges;
        public List<Face> Faces;

        public GameBorder(List<Vertex> vertices, List<Edge> edges, List<Face> faces)
        {
            Vertices = vertices;
            Edges = edges;
            Faces = faces;
        }

        public void Synchronize()
        {
            verticesDesc.Clear();
            edgesDesc.Clear();
            facesDesc.Clear();

            foreach (Vertex curVx in Vertices)
            {
                verticesDesc.Add(curVx);
            }

            foreach (Edge curEg in Edges)
            {
                edgesDesc.Add(curEg);
            }

            foreach (Face curFc in Faces)
            {
                facesDesc.Add(curFc);
            }
        }

        public Vertex FindVerticesByCoordinate(Coord coordinate)
        {
            foreach (Vertex currentVertexStruct in Vertices)
            {
                if (SamePoints(currentVertexStruct.Coordinate, coordinate))
                {
                    return currentVertexStruct;
                }
            }
            return noVertex;
        }

        public Face FindFaceByCoordinate(Coord coordinate)
        {
            foreach (Face currentFaceStruct in Faces)
            {
                if (SamePoints(currentFaceStruct.Coordinate, coordinate))
                {
                    return currentFaceStruct;
                }
            }

            return noFace;
        }

        public Edge FindEdgeByCoordinate(Coord coordinate)
        {
            foreach (Edge currentEdgeStruct in Edges)
            {
                if (SamePoints(currentEdgeStruct.CentreCoordinate, coordinate))
                {
                    return currentEdgeStruct;
                }
            }
            return noEdge;
        }

    }
}
