using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class GameBorderDesc
    {
        protected const int MAX_DISTANCE_BEETWEN_TWO_SAME_VERTICES = 10;
        protected const int DISTANCE_BEETWEN_NEIGHBORING_FACE_AND_VERTEX = 80;
        protected const int DISTANCE_BEETWEN_NEIGHBORING_TWO_VERTICES = 80;

        //na pozici "i" je počet možností, kterými muže padnout číslo "i+2" (0 ani 1 nemůže padnout) 
        private double[] countNumbersFit;

        //počet všech kombinací vzniklých na hracích kostkách
        private double allPossibilities;

        //na pozici "i" je pravděpodobnost, že padne součet na kostkách "i+2" (0 ani 1 nemůže padnout)
        public double[] probabilities;

        public List<Vertex> verticesDesc;
        public List<Edge> edgesDesc;
        public List<Face> facesDesc;

        public GameBorderDesc()
        {
            verticesDesc = new List<Vertex>();
            edgesDesc = new List<Edge>();
            facesDesc = new List<Face>();
            countNumbersFit = new double[] { 1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1 };
            allPossibilities = 36;

            ComputeProbabilities();
        }

        public Vertex FindVerticesByCoordinateDesc(Coord coordinate)
        {
            foreach (Vertex currentVertexStruct in verticesDesc)
            {
                if (SamePoints(currentVertexStruct.Coordinate, coordinate))
                {
                    return currentVertexStruct;
                }
            }
            return new Vertex(new Coord(0, 0));
        }

        public Face FindFaceByCoordinateDesc(Coord coordinate)
        {
            foreach (Face currentFaceStruct in facesDesc)
            {
                if (SamePoints(currentFaceStruct.Coordinate, coordinate))
                {
                    return currentFaceStruct;
                }
            }

            return new Face(new Coord(-1,-1), Game.materials.noMaterial, -1);
        }

        public Edge FindEdgeByCoordinateDesc(Coord coordinate)
        {
            foreach (Edge currentEdgeStruct in edgesDesc)
            {
                if (SamePoints(currentEdgeStruct.CentreCoordinate, coordinate))
                {
                    return currentEdgeStruct;
                }
            }
            return new Edge(Tuple.Create(new Coord(-1,-1), new Coord(-1,-1)));
        }

        public static bool SamePoints(Coord firstPoint, Coord secondPoint)
        {
            return IsAproximatly(firstPoint, secondPoint, MAX_DISTANCE_BEETWEN_TWO_SAME_VERTICES);
        }

        public static bool SameLine(Tuple<Coord, Coord> firstLine, Tuple<Coord, Coord> secondLine)
        {
            return (GameBorderDesc.SamePoints(firstLine.Item1, secondLine.Item1) && GameBorderDesc.SamePoints(firstLine.Item2, secondLine.Item2)) ||
                (GameBorderDesc.SamePoints(firstLine.Item1, secondLine.Item2) && GameBorderDesc.SamePoints(firstLine.Item2, secondLine.Item1));
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
