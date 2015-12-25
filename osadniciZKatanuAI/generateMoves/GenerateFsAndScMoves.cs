using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    public class GenerateFsAndScMoves
    {

        GenerateMovesProperties movesProp;

        public GenerateFsAndScMoves()
        {
            movesProp = new GenerateMovesProperties();
        }

        public GenerateFsAndScMoves(GenerateMovesProperties movesProp)
        {
            this.movesProp = movesProp;
        }

        public List<FirstPhaseGameMove> Generate(GameDesc gmDesc)
        {
            List<FirstPhaseGameMove> possibleMoves = new List<FirstPhaseGameMove>();
            double fitness;

            foreach (var curVx in gmDesc.GameBorderDesc.verticesDesc)
            {
                if (curVx.IsFreePlaceForVillage())
                {
                    fitness = ComputeVertexFitness(curVx, gmDesc);
                    foreach (var curEg in curVx.EdgeNeighborsDesc)
                    {
                        FirstPhaseGameMove mvDesc = new FirstPhaseGameMove(curVx, curEg);
                        fitness += ComputeEdgeFitness(curEg, gmDesc);
                        mvDesc.fitnessMove = fitness;
                        possibleMoves.Add(mvDesc);
                    }
                }
            }

            return possibleMoves;
        }

        private double ComputeEdgeFitness(EdgeDesc edge, GameDesc gmDesc)
        {
            return movesProp.weightEdgeGeneral;
        }

        private double ComputeVertexFitness(VertexDesc vertex, GameDesc gmDesc)
        {
            List<GameDesc.materials> matWhatIHave = WhatIHave(gmDesc);

            double[] prob = gmDesc.GameBorderDesc.probabilities;
            double fitness = 0;

            foreach (var curFc in vertex.FaceNeighborsDesc)
            {
                double matWeigth = ComputeWeightMaterial(curFc.Material);
                fitness = fitness + movesProp.weightGoodNumbers * matWeigth * prob[curFc.ProbabilityNumber - 2];
                if (!matWhatIHave.Contains(curFc.Material) && curFc.Material != GameDesc.materials.desert)
                {
                    fitness += movesProp.weightMissingMaterial;
                }
            }

            if (vertex.Port)
            {
                if (vertex.PortMaterial == GameDesc.materials.noMaterial)
                {
                    fitness = fitness + movesProp.weightPortThreeOne;
                }
                else
                {
                    fitness = fitness + movesProp.weightPortTwoOne;
                }
            }

            return fitness;
        }

        private List<GameDesc.materials> WhatIHave(GameDesc gmDesc)
        {
            List<GameDesc.materials> matWhatIHave = new List<GameDesc.materials>();

            foreach (var curVx in gmDesc.ActualPlayerDesc.VillageDesc)
            {
                foreach (var curFc in curVx.FaceNeighborsDesc)
                {
                    if (!matWhatIHave.Contains(curFc.Material))
                    {
                        matWhatIHave.Add(curFc.Material);
                    }
                }
            }

            return matWhatIHave;
        }

        private double ComputeWeightMaterial(GameDesc.materials curMat)
        {
            double fitness;
            switch (curMat)
            {
                case GameDesc.materials.brick: fitness = movesProp.weightBrick; break;
                case GameDesc.materials.grain: fitness = movesProp.weightGrain; break;
                case GameDesc.materials.sheep: fitness = movesProp.weightSheep; break;
                case GameDesc.materials.stone: fitness = movesProp.weightStone; break;
                case GameDesc.materials.wood: fitness = movesProp.weightWood; break;
                default: fitness = 0; break;
            }

            return fitness;
        }
    }
}
