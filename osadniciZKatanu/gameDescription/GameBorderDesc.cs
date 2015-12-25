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

        public List<VertexDesc> verticesDesc;
        public List<EdgeDesc> edgesDesc;
        public List<FaceDesc> facesDesc;

        public GameBorderDesc()
        {
            verticesDesc = new List<VertexDesc>();
            edgesDesc = new List<EdgeDesc>();
            facesDesc = new List<FaceDesc>();
            countNumbersFit = new double[] { 1, 2, 3, 4, 5, 6, 5, 4, 3, 2, 1 };
            allPossibilities = 36;

            ComputeProbabilities();
        }

        public VertexDesc FindVerticesByCoordinateDesc(Coord coordinate)
        {
            foreach (VertexDesc currentVertexStruct in verticesDesc)
            {
                if (SamePoints(currentVertexStruct.Coordinate, coordinate))
                {
                    return currentVertexStruct;
                }
            }
            return new VertexDesc(new Coord(0, 0));
        }

        public FaceDesc FindFaceByCoordinateDesc(Coord coordinate)
        {
            foreach (FaceDesc currentFaceStruct in facesDesc)
            {
                if (SamePoints(currentFaceStruct.Coordinate, coordinate))
                {
                    return currentFaceStruct;
                }
            }

            return new FaceDesc(new Coord(-1,-1), GameDesc.materials.noMaterial, -1);
        }

        public EdgeDesc FindEdgeByCoordinateDesc(Coord coordinate)
        {
            foreach (EdgeDesc currentEdgeStruct in edgesDesc)
            {
                if (SamePoints(currentEdgeStruct.CentreCoordinate, coordinate))
                {
                    return currentEdgeStruct;
                }
            }
            return new EdgeDesc(Tuple.Create(new Coord(-1,-1), new Coord(-1,-1)));
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

        public static bool SameLine(EdgeDesc firstEg, EdgeDesc secEg)
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

        public static bool IsAproximatly(VertexDesc first, VertexDesc second, int min)
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
