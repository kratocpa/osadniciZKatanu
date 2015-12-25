using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    class CommonFeatures
    {
        GenerateMovesProperties movesProp;
        
        public CommonFeatures()
        {
            movesProp = new GenerateMovesProperties();
        }

        public CommonFeatures(GenerateMovesProperties movesProp)
        {
            this.movesProp = movesProp;
        }

        public List<EdgeDesc> GeneratePossibleEdgesToBuildRoad(GameDesc gmDesc)
        {
            List<EdgeDesc> possibleEdges = new List<EdgeDesc>();
            foreach (EdgeDesc curEg in gmDesc.ActualPlayerDesc.RoadDesc)
            {
                foreach (EdgeDesc cEg in curEg.EdgeNeighborsDesc)
                {
                    if (!cEg.Road && cEg.IsHereAdjacentRoadWithColor(gmDesc.ActualPlayerDesc.Color) &&
                        gmDesc.ActualPlayerDesc.RoadRemaining > 0 )
                    {
                        possibleEdges.Add(cEg);
                    }
                }
            }
            return possibleEdges;
        }

        public double RateRoad(EdgeDesc curEg, GameDesc gmDesc)
        {
            double fitness;
            fitness = movesProp.weightRoadGeneral;
            int newLength = LengthOfNewPath(curEg, gmDesc);
            if (newLength > gmDesc.ActualPlayerDesc.LongestWayLength)
            {
                if (newLength > gmDesc.LongestRoad)
                {
                    fitness += movesProp.weightLongestRoad;
                }
                else
                {
                    fitness += movesProp.weightExtensionRoad;
                }
            }
            if (CanIBuildVillageOnEdge(curEg))
            {
                fitness += movesProp.weightSpaceForVillage;
            }
            return fitness;
        }

        public bool CanIBuildVillageOnEdge(EdgeDesc curEg)
        {
            bool succes = false;
            foreach (VertexDesc curVx in curEg.VertexNeighborsDesc)
            {
                succes = succes || (!curVx.Building && !curVx.IsHereBuildingInNeighbour());
            }
            return succes;
        }

        public int LengthOfNewPath(EdgeDesc newEg, GameDesc gmDesc)
        {
            List<EdgeDesc> newRoadList = new List<EdgeDesc>();

            foreach (var curEg in gmDesc.ActualPlayerDesc.RoadDesc)
            {
                newRoadList.Add(curEg);
            }
            newRoadList.Add(newEg);

            return Math.Max(
                PlayerDesc.FindFurthermostVertexDesc(0, newEg.VertexNeighborsDesc[0], newRoadList).Item2,
                PlayerDesc.FindFurthermostVertexDesc(0, newEg.VertexNeighborsDesc[1], newRoadList).Item2
            );
        }

    }
}
