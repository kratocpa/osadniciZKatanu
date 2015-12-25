using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace osadniciZKatanu
{
    public class SetGameBorder
    {
        private const int MAX_VERTEX_DISTANCE = 70;

        public SetGameBorder()
        {
        }

        public GameBorder GenerateGameBorder(bool randomMaterials, GameProperties gmProp)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<Edge> edges = new List<Edge>();
            List<Face> faces = new List<Face>();
            foreach (var curVx in gmProp.Vertices) { vertices.Add((Vertex)curVx.Clone()); }
            foreach (var curEg in gmProp.Edges) { edges.Add((Edge)curEg.Clone()); }
            foreach (var curFc in gmProp.Faces) { faces.Add((Face)curFc.Clone()); }

            foreach (Vertex currentVertex in vertices)
            {
                currentVertex.AddEdgeNeighbors(FindNeighboringEdgesToVertex(currentVertex, edges));
                currentVertex.AddFaceNeighbors(FindNeighboringFacesToVertex(currentVertex, faces));
                currentVertex.AddVertexNeighbors(FindNeighboringVerticesToVertex(currentVertex, vertices));
            }

            foreach (Face currentFace in faces)
            {
                currentFace.AddVertexNeighbors(FindNeighboringVerticesToFace(currentFace, vertices));
            }

            foreach (Edge current_edge in edges)
            {
                current_edge.AddEdgeNeighbors(FindNeighboringEdgeToEdge(current_edge, edges));
                current_edge.AddVertexNeighbors(FindNeighboringVerticestoEdge(current_edge, vertices));
            }

            if (randomMaterials)
            {
                MixedMaterials(faces);
            }

            GameBorder res = new GameBorder(vertices, edges, faces);
            res.Synchronize();

            return res;
        }

        void MixedMaterials(List<Face> faces)
        {
            Random rand = new Random();
            int fs, sc;
            GameDesc.materials fsMat, scMat;
            for (int i = 0; i < 100; i++)
            {
                fs = rand.Next(0, faces.Count - 1);
                sc = rand.Next(0, faces.Count - 1);
                bool succesFs = false, succesSc = false;
                if (fs != sc && fs != 9 && sc != 9)
                {
                    fsMat = faces[fs].Material;
                    scMat = faces[sc].Material;
                    faces[fs].SetMaterial(scMat);
                    faces[sc].SetMaterial(fsMat);

                    succesFs = IsThreeSameMaterialsInNeighbour(faces[fs]);
                    succesSc = IsThreeSameMaterialsInNeighbour(faces[sc]);


                    if (!succesFs || !succesSc)
                    {
                        faces[fs].SetMaterial(fsMat);
                        faces[sc].SetMaterial(scMat);
                    }
                }
            }
        }

        private bool IsThreeSameMaterialsInNeighbour(Face searchedFace)
        {
            foreach (var curVert in searchedFace.VerticesNeighbors)
            {
                bool succes = false;
                //pro každý vrchol sousedící se stěnou musím zkontrolovat, jestli nesousedí se třemi stejnými stěnami
                GameDesc.materials lastMat = curVert.FaceNeighbors[0].Material;
                if (curVert.FaceNeighbors.Count == 3)
                {
                    foreach (var curFace in curVert.FaceNeighbors)
                    {
                        if (curFace.Material != lastMat) { succes = true; }
                    }

                    if (!succes)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        List<Vertex> FindNeighboringVerticesToVertex(Vertex searchedVertex, List<Vertex> vertices)
        {
            List<Vertex> listOfNeighboringVertices = new List<Vertex>();
            foreach (Vertex currentVertex in vertices)
            {
                if (NeighboringVertices(currentVertex, searchedVertex)) { listOfNeighboringVertices.Add(currentVertex); }
            }
            return listOfNeighboringVertices;
        }

        List<Edge> FindNeighboringEdgesToVertex(Vertex searchedVertex, List<Edge> edges)
        {
            List<Edge> listOfNeighboringEdges = new List<Edge>();
            foreach (Edge currentEdge in edges)
            {
                if (NeighboringEdgeAndVertex(searchedVertex, currentEdge)) { listOfNeighboringEdges.Add(currentEdge); }
            }
            return listOfNeighboringEdges;
        }

        List<Face> FindNeighboringFacesToVertex(Vertex searchedVertex, List<Face> faces)
        {
            List<Face> listOfNeighboringFaces = new List<Face>();
            foreach (Face currentFace in faces)
            {
                if (NeighboringFaceAndVertex(searchedVertex, currentFace)) { listOfNeighboringFaces.Add(currentFace); }
            }
            return listOfNeighboringFaces;
        }

        List<Vertex> FindNeighboringVerticesToFace(Face searchedFace, List<Vertex> vertices)
        {
            List<Vertex> listOfNeighboringVertices = new List<Vertex>();
            foreach (Vertex currentVertex in vertices)
            {
                if (NeighboringFaceAndVertex(currentVertex, searchedFace)) { listOfNeighboringVertices.Add(currentVertex); }
            }
            return listOfNeighboringVertices;
        }

        List<Edge> FindNeighboringEdgeToEdge(Edge searchedEdge, List<Edge> edges)
        {
            List<Edge> listOfNeighboringEdges = new List<Edge>();
            foreach (Edge currentEdge in edges)
            {
                if (NeighboringEdgeAndEdge(searchedEdge, currentEdge)) { listOfNeighboringEdges.Add(currentEdge); }
            }
            return listOfNeighboringEdges;
        }

        List<Vertex> FindNeighboringVerticestoEdge(Edge searchedEdge, List<Vertex> vertices)
        {
            List<Vertex> listOfNeighboringVertices = new List<Vertex>();
            foreach (Vertex currentVertex in vertices)
            {
                if (NeighboringEdgeAndVertex(currentVertex, searchedEdge)) { listOfNeighboringVertices.Add(currentVertex); }
            }
            return listOfNeighboringVertices;
        }

        bool NeighboringFaceAndVertex(Vertex vertex, Face face)
        {
            int maxDistance = 80;
            return GameBorderDesc.IsAproximatly(vertex.Coordinate, face.Coordinate, maxDistance);
        }

        bool NeighboringEdgeAndVertex(Vertex vertex, Edge edge)
        {
            if (GameBorderDesc.SamePoints(vertex.Coordinate, edge.Coordinate.Item1) ||
                GameBorderDesc.SamePoints(vertex.Coordinate, edge.Coordinate.Item2)) { return true; }
            return false;
        }

        bool NeighboringEdgeAndEdge(Edge firstEdge, Edge secondEdge)
        {
            if ((GameBorderDesc.SamePoints(firstEdge.Coordinate.Item1, secondEdge.Coordinate.Item1) ||
                GameBorderDesc.SamePoints(firstEdge.Coordinate.Item1, secondEdge.Coordinate.Item2) ||
                GameBorderDesc.SamePoints(firstEdge.Coordinate.Item2, secondEdge.Coordinate.Item1) ||
                GameBorderDesc.SamePoints(firstEdge.Coordinate.Item2, secondEdge.Coordinate.Item2)) &&
                !GameBorderDesc.SameLine(firstEdge.Coordinate, secondEdge.Coordinate)) { return true; }

            return false;
        }

        public static bool NeighboringVertices(Vertex firstVertices, Vertex secondVertices)
        {
            int maxDistance = 80;
            return GameBorderDesc.IsAproximatly(firstVertices.Coordinate, secondVertices.Coordinate, maxDistance) &&
                !GameBorderDesc.SamePoints(firstVertices.Coordinate, secondVertices.Coordinate);
        }
    }
}
