﻿using System;
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

        public List<Edge> GeneratePossibleEdgesToBuildRoad(GameProperties gmProp, PlayerProperties plProp)
        {
            List<Edge> possibleEdges = new List<Edge>();
            foreach (Edge curEg in plProp.Road)
            {
                foreach (Edge cEg in curEg.EdgeNeighborsDesc)
                {
                    if (!cEg.Road && cEg.IsHereAdjacentRoadWithColor(plProp.Color) &&
                        plProp.RoadRemaining > 0 )
                    {
                        possibleEdges.Add(cEg);
                    }
                }
            }
            return possibleEdges;
        }

        public double RateRoad(GameProperties gmProp, PlayerProperties plProp, Edge curEg)
        {
            double fitness;
            fitness = movesProp.weightRoadGeneral;
            int newLength = LengthOfNewPath(gmProp, plProp, curEg);
            if (newLength > plProp.LongestWayLength)
            {
                if (newLength > gmProp.LongestRoad)
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

        public bool CanIBuildVillageOnEdge(Edge curEg)
        {
            bool succes = false;
            foreach (VertexDesc curVx in curEg.VertexNeighborsDesc)
            {
                succes = succes || (!curVx.Building && !curVx.IsHereBuildingInNeighbour());
            }
            return succes;
        }

        public int LengthOfNewPath(GameProperties gmProp, PlayerProperties plProp, Edge newEg)
        {
            List<Edge> newRoadList = new List<Edge>();

            foreach (var curEg in plProp.Road)
            {
                newRoadList.Add(curEg);
            }
            newRoadList.Add(newEg);

            return Math.Max(
                Player.FindFurthermostVertex(0, newEg.VertexNeighbors[0], newRoadList),
                Player.FindFurthermostVertex(0, newEg.VertexNeighbors[1], newRoadList)
            );
        }

    }
}
