using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    class GenerateThiefMoves
    {
        GenerateMovesProperties movesProp;
        GenerateExchangeMoves exchange;

        public GenerateThiefMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
        }

        public GenerateThiefMoves(GenerateMovesProperties movesProp)
        {
            this.movesProp = movesProp;
            exchange = new GenerateExchangeMoves(movesProp);
        }

        public List<ThiefMove> Generate(GameDesc gmDesc)
        {
            List<ThiefMove> possibleMoves = new List<ThiefMove>();

            foreach (var curFc in gmDesc.GameBorderDesc.facesDesc)
            {
                foreach (var curMv in ComputeMoveThiefFaceFitness(curFc, gmDesc))
                {
                    possibleMoves.Add(curMv);
                }
            }

            return possibleMoves;
        }

        private List<ThiefMove> ComputeMoveThiefFaceFitness(FaceDesc curFc, GameDesc gmDesc)
        {
            double[] prob = gmDesc.GameBorderDesc.probabilities;
            int countOfMyBuilding = 0;
            int countOfOtherBuilding = 0;
            List<GameDesc.color> colors = new List<GameDesc.color>();
            List<ThiefMove> result = new List<ThiefMove>();

            foreach (var curVx in curFc.VerticesNeighborsDesc)
            {
                if (curVx.Building)
                {
                    if (curVx.Color == gmDesc.ActualPlayerDesc.Color)
                    {
                        countOfMyBuilding++;
                    }
                    else
                    {
                        countOfOtherBuilding++;
                        colors.Add(curVx.Color);
                    }
                }
            }

            if (colors.Count > 0)
            {
                foreach (var col in colors)
                {
                    ThiefMove mvDesc = new ThiefMove(curFc, col);
                    mvDesc.fitnessMove = ((movesProp.weightThiefMoveBase + countOfOtherBuilding * movesProp.weightThiefMoveOtherBuilding) / (countOfMyBuilding * movesProp.weightThiefMoveMyBuilding + 1)) * prob[curFc.ProbabilityNumber - 2] * movesProp.weightThiefMoveProb;
                    result.Add(mvDesc);
                }
            }
            else
            {
                ThiefMove mvDesc = new ThiefMove(curFc);
                mvDesc.fitnessMove = movesProp.weightThiefMoveBase;
                result.Add(mvDesc);
            }

            return result;
        }
    }
}
