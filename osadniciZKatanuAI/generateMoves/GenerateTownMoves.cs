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

        GameProperties gmProp;
        PlayerProperties plProp;

        public GenerateTownMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
        }

        public GenerateTownMoves(GenerateMovesProperties movesProp, GameProperties gmProp, PlayerProperties plProp)
        {
            this.movesProp = movesProp;
            this.gmProp = gmProp;
            this.plProp = plProp;
            exchange = new GenerateExchangeMoves(movesProp, gmProp, plProp);
        }

        public List<BuildTownMove> Generate()
        {
            List<BuildTownMove> possibleTownMoves = new List<BuildTownMove>();

            if (plProp.Materials.IsPossibleDelete(gmProp.MaterialsForTown) && plProp.TownRemaining>0)
            {
                var possibleVertices = GeneratePossibleVerticesToBuildTown();
                foreach (var curVx in possibleVertices)
                {
                    BuildTownMove mvDesc = new BuildTownMove(curVx);
                    mvDesc.fitnessMove = RateTown(curVx);
                    possibleTownMoves.Add(mvDesc);
                }
            }
            else if (plProp.TownRemaining > 0)
            {
                BuildTownMove mvDesc = (BuildTownMove)exchange.Generate(gmProp.MaterialsForTown, GenerateExchangeMoves.typeMove.buildTown);
                if (mvDesc != null)
                {
                    var possibleVertices = GeneratePossibleVerticesToBuildTown();
                    foreach (var curVx in possibleVertices)
                    {
                        mvDesc = (BuildTownMove)exchange.Generate(gmProp.MaterialsForTown, GenerateExchangeMoves.typeMove.buildTown);
                        mvDesc.fitnessMove = RateTown(curVx);
                        mvDesc.BuildTown(curVx);
                        possibleTownMoves.Add(mvDesc);
                    }
                }
            }

            return possibleTownMoves;
        }

        private List<Vertex> GeneratePossibleVerticesToBuildTown()
        {
            List<Vertex> possibleVertices = new List<Vertex>();
            foreach (Vertex curVx in plProp.Village)
            {
                if (curVx.IsHereVillage(plProp.Color) && plProp.TownRemaining > 0)
                {
                    possibleVertices.Add(curVx);
                }
            }
            return possibleVertices;
        }

        private int RateTown(Vertex curVx)
        {
            double fitness;
            fitness = movesProp.weightTownGeneral;

            double[] prob = gmProp.GameBorderData.probabilities;

            foreach (var curFc in curVx.FaceNeighbors)
            {
                fitness = fitness + movesProp.weightTownGoodNumbers * prob[curFc.ProbabilityNumber - 2];
            }

            return (int)fitness;
        }

    }
}
