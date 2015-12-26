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

        public List<BuildVillageMove> Generate(GameProperties gmProp, PlayerProperties plProp)
        {
            List<BuildVillageMove> possibleVillageMoves = new List<BuildVillageMove>();

            if (plProp.Materials.IsPossibleDelete(gmProp.MaterialsForVillage) && plProp.VillageRemaining>0)
            {
                var possibleVertices = GeneratePossibleVerticesToBuildVillage(gmProp, plProp);
                foreach (var curVx in possibleVertices)
                {
                    BuildVillageMove mvDesc = new BuildVillageMove(curVx);
                    mvDesc.fitnessMove = RateVillage(gmProp, plProp, curVx);
                    possibleVillageMoves.Add(mvDesc);
                }
            }
            else if (plProp.VillageRemaining > 0)
            {
                BuildVillageMove mvDesc = (BuildVillageMove)exchange.Generate(gmProp, plProp, gmProp.MaterialsForVillage, GenerateExchangeMoves.typeMove.buildVillage);
                if (mvDesc != null)
                {
                    var possibleVertices = GeneratePossibleVerticesToBuildVillage(gmProp, plProp);
                    foreach (var curVx in possibleVertices)
                    {
                        mvDesc = (BuildVillageMove)exchange.Generate(gmProp, plProp, gmProp.MaterialsForVillage, GenerateExchangeMoves.typeMove.buildVillage);
                        mvDesc.fitnessMove = RateVillage(gmProp, plProp, curVx);
                        mvDesc.BuildVillage(curVx);
                        possibleVillageMoves.Add(mvDesc);
                    }
                }
            }

            return possibleVillageMoves;
        }

        private int RateVillage(GameProperties gmProp, PlayerProperties plProp, VertexDesc curVx)
        {
            double fitness;
            fitness = movesProp.weightVillageGeneral;

            double[] prob = gmProp.GameBorderData.probabilities;

            foreach (var curFc in curVx.FaceNeighborsDesc)
            {
                fitness = fitness + movesProp.weightVillageGoodNumbers * prob[curFc.ProbabilityNumber - 2];
            }

            if (curVx.Port)
            {
                if (curVx.PortMaterial == Game.materials.noMaterial)
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

        private List<VertexDesc> GeneratePossibleVerticesToBuildVillage(GameProperties gmDesc, PlayerProperties plProp)
        {
            List<VertexDesc> possibleVertices = new List<VertexDesc>();
            foreach (EdgeDesc curEg in plProp.Road)
            {
                foreach (VertexDesc curVx in curEg.VertexNeighborsDesc)
                {
                    if (curVx.IsFreePlaceForVillage() && curVx.IsHereAdjectedRoadWithColor(plProp.Color) && plProp.VillageRemaining>0)                        
                    {
                        possibleVertices.Add(curVx);
                    }
                }
            }
            return possibleVertices;
        }

    }
}
