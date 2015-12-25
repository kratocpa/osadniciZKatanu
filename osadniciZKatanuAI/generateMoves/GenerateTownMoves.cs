using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    class GenerateTownMoves
    {
        GenerateMovesProperties movesProp;
        GenerateExchangeMoves exchange;

        public GenerateTownMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
        }

        public GenerateTownMoves(GenerateMovesProperties movesProp)
        {
            this.movesProp = movesProp;
            exchange = new GenerateExchangeMoves(movesProp);
        }

        public List<BuildTownMove> Generate(GameDesc gmDesc)
        {
            List<BuildTownMove> possibleTownMoves = new List<BuildTownMove>();

            if (gmDesc.ActualPlayerDesc.MaterialsDesc.IsPossibleDelete(gmDesc.materialForTownDesc) && gmDesc.ActualPlayerDesc.TownRemaining>0)
            {
                var possibleVertices = GeneratePossibleVerticesToBuildTown(gmDesc);
                foreach (var curVx in possibleVertices)
                {
                    BuildTownMove mvDesc = new BuildTownMove(curVx);
                    mvDesc.fitnessMove = RateTown(curVx, gmDesc);
                    possibleTownMoves.Add(mvDesc);
                }
            }
            else if (gmDesc.ActualPlayerDesc.TownRemaining > 0)
            {
                BuildTownMove mvDesc = (BuildTownMove)exchange.Generate(gmDesc.materialForTownDesc, gmDesc, GenerateExchangeMoves.typeMove.buildTown);
                if (mvDesc != null)
                {
                    var possibleVertices = GeneratePossibleVerticesToBuildTown(gmDesc);
                    foreach (var curVx in possibleVertices)
                    {
                        mvDesc = (BuildTownMove)exchange.Generate(gmDesc.materialForTownDesc, gmDesc, GenerateExchangeMoves.typeMove.buildTown);
                        mvDesc.fitnessMove = RateTown(curVx, gmDesc);
                        mvDesc.BuildTown(curVx);
                        possibleTownMoves.Add(mvDesc);
                    }
                }
            }

            return possibleTownMoves;
        }

        private List<VertexDesc> GeneratePossibleVerticesToBuildTown(GameDesc gmDesc)
        {
            List<VertexDesc> possibleVertices = new List<VertexDesc>();
            foreach (VertexDesc curVx in gmDesc.ActualPlayerDesc.VillageDesc)
            {
                if (curVx.IsHereVillage(gmDesc.ActualPlayerDesc.Color) && gmDesc.ActualPlayerDesc.TownRemaining > 0)
                {
                    possibleVertices.Add(curVx);
                }
            }
            return possibleVertices;
        }

        private int RateTown(VertexDesc curVx, GameDesc gmDesc)
        {
            double fitness;
            fitness = movesProp.weightTownGeneral;

            double[] prob = gmDesc.GameBorderDesc.probabilities;

            foreach (var curFc in curVx.FaceNeighborsDesc)
            {
                fitness = fitness + movesProp.weightTownGoodNumbers * prob[curFc.ProbabilityNumber - 2];
            }

            return (int)fitness;
        }

    }
}
