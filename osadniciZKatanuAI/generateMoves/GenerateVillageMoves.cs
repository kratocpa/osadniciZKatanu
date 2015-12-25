using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    class GenerateVillageMoves
    {
        GenerateMovesProperties movesProp;
        GenerateExchangeMoves exchange;

        public GenerateVillageMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
        }

        public GenerateVillageMoves(GenerateMovesProperties movesProp)
        {
            this.movesProp = movesProp;
            exchange = new GenerateExchangeMoves(movesProp);
        }

        public List<BuildVillageMove> Generate(GameDesc gmDesc)
        {
            List<BuildVillageMove> possibleVillageMoves = new List<BuildVillageMove>();

            if (gmDesc.ActualPlayerDesc.MaterialsDesc.IsPossibleDelete(gmDesc.materialForVillageDesc) && gmDesc.ActualPlayerDesc.VillageRemaining>0)
            {
                var possibleVertices = GeneratePossibleVerticesToBuildVillage(gmDesc);
                foreach (var curVx in possibleVertices)
                {
                    BuildVillageMove mvDesc = new BuildVillageMove(curVx);
                    mvDesc.fitnessMove = RateVillage(curVx, gmDesc);
                    possibleVillageMoves.Add(mvDesc);
                }
            }
            else if (gmDesc.ActualPlayerDesc.VillageRemaining > 0)
            {
                BuildVillageMove mvDesc = (BuildVillageMove)exchange.Generate(gmDesc.materialForVillageDesc, gmDesc, GenerateExchangeMoves.typeMove.buildVillage);
                if (mvDesc != null)
                {
                    var possibleVertices = GeneratePossibleVerticesToBuildVillage(gmDesc);
                    foreach (var curVx in possibleVertices)
                    {
                        mvDesc = (BuildVillageMove)exchange.Generate(gmDesc.materialForVillageDesc, gmDesc, GenerateExchangeMoves.typeMove.buildVillage);
                        mvDesc.fitnessMove = RateVillage(curVx, gmDesc);
                        mvDesc.BuildVillage(curVx);
                        possibleVillageMoves.Add(mvDesc);
                    }
                }
            }

            return possibleVillageMoves;
        }

        private int RateVillage(VertexDesc curVx, GameDesc gmDesc)
        {
            double fitness;
            fitness = movesProp.weightVillageGeneral;

            double[] prob = gmDesc.GameBorderDesc.probabilities;

            foreach (var curFc in curVx.FaceNeighborsDesc)
            {
                fitness = fitness + movesProp.weightVillageGoodNumbers * prob[curFc.ProbabilityNumber - 2];
            }

            if (curVx.Port)
            {
                if (curVx.PortMaterial == GameDesc.materials.noMaterial)
                {
                    fitness = fitness + movesProp.weightVillagePortThreeOne;
                }
                else
                {
                    fitness = fitness + movesProp.weightVillagePortTwoOne;
                }
            }

            return (int)fitness;
        }

        private List<VertexDesc> GeneratePossibleVerticesToBuildVillage(GameDesc gmDesc)
        {
            List<VertexDesc> possibleVertices = new List<VertexDesc>();
            foreach (EdgeDesc curEg in gmDesc.ActualPlayerDesc.RoadDesc)
            {
                foreach (VertexDesc curVx in curEg.VertexNeighborsDesc)
                {
                    if (curVx.IsFreePlaceForVillage() && curVx.IsHereAdjectedRoadWithColor(gmDesc.ActualPlayerDesc.Color) && gmDesc.ActualPlayerDesc.VillageRemaining>0)                        
                    {
                        possibleVertices.Add(curVx);
                    }
                }
            }
            return possibleVertices;
        }

    }
}
