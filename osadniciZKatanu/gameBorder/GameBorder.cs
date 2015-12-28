using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class GameBorder
    {
        private const int MAX_DISTANCE_BEETWEN_TWO_SAME_VERTICES = 10;
        private const int DISTANCE_BEETWEN_NEIGHBORING_FACE_AND_VERTEX = 80;
        private const int DISTANCE_BEETWEN_NEIGHBORING_TWO_VERTICES = 80;

        //na pozici "i" je počet možností, kterými muže padnout číslo "i+2" (0 ani 1 nemůže padnout) 
        private double[] countNumbersFit;

        //počet všech kombinací vzniklých na hracích kostkách
        private double allPossibilities;

        //na pozici "i" je pravděpodobnost, že padne součet na kostkách "i+2" (0 ani 1 nemůže padnout)
        public double[] probabilities;

        public Vertex noVertex = new Vertex(new Coord(-1, -1));
        public Edge noEdge = new Edge(Tuple.Create(new Coord(-1, -1), new Coord(-1, -1)));
        public Face noFace = new Face(new Coord(-1, -1), Game.materials.noMaterial, 0);

        public List<Vertex> Vertices;
        public List<Edge> Edges;
        public List<Face> Faces;

        public GameBorder(List<Vertex> vertices, List<Edge> edges, List<Face> faces)
        {
            Vertices = vertices;
            Edges = edges;
            Faces = faces;

            countNumbersFit = new double[] { 1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1 };
            allPossibilities = 36;

            ComputeProbabilities();
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

        public static bool SamePoints(Coord firstPoint, Coord secondPoint)
        {
            return IsAproximatly(firstPoint, secondPoint, MAX_DISTANCE_BEETWEN_TWO_SAME_VERTICES);
        }

        public static bool SameLine(Tuple<Coord, Coord> firstLine, Tuple<Coord, Coord> secondLine)
        {
            return (SamePoints(firstLine.Item1, secondLine.Item1) && SamePoints(firstLine.Item2, secondLine.Item2)) ||
                (SamePoints(firstLine.Item1, secondLine.Item2) && SamePoints(firstLine.Item2, secondLine.Item1));
        }

        public static bool SameLine(Edge firstEg, Edge secEg)
        {
            return SameLine(firstEg.Coordinate, secEg.Coordinate);
        }

        public static bool IsAproximatly(Coord first, Coord second, int min)
        {
            if (first.X - second.X < min && first.X - second.X > -min &&
                first.Y - second.Y < min && first.Y - second.Y > -min)
            {
                return true;
            }
            return false;
        }

        public static bool IsAproximatly(Vertex first, Vertex second, int min)
        {
            return IsAproximatly(first.Coordinate, second.Coordinate, min);
        }

        //spočítá pravděpodobnosti, s jakýma padají jednotlivá čísla
        private void ComputeProbabilities()
        {
            int numberCount = countNumbersFit.Count();
            probabilities = new double[numberCount];
            for (int i = 0; i < numberCount; i++)
            {
                probabilities[i] = countNumbersFit[i] / allPossibilities;
            }
        }
    }
}
