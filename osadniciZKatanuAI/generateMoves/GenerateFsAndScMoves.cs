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

        public List<FirstPhaseGameMove> Generate(GameProperties gmProp, PlayerProperties plProp)
        {
            List<FirstPhaseGameMove> possibleMoves = new List<FirstPhaseGameMove>();
            double fitness;

            foreach (var curVx in gmProp.GameBorderData.Vertices)
            {
                if (curVx.IsFreePlaceForVillage())
                {
                    fitness = ComputeVertexFitness(gmProp, plProp, curVx);
                    foreach (var curEg in curVx.EdgeNeighbors)
                    {
                        FirstPhaseGameMove mvDesc = new FirstPhaseGameMove(curVx, curEg);
                        fitness += ComputeEdgeFitness(gmProp, plProp, curEg);
                        mvDesc.fitnessMove = fitness;
                        possibleMoves.Add(mvDesc);
                    }
                }
            }

            return possibleMoves;
        }

        private double ComputeEdgeFitness(GameProperties gmProp, PlayerProperties plProp, Edge edge)
        {
            return movesProp.weightEdgeGeneral;
        }

        private double ComputeVertexFitness(GameProperties gmProp, PlayerProperties plProp, Vertex vertex)
        {
            List<Game.materials> matWhatIHave = WhatIHave(gmProp, plProp);

            double[] prob = gmProp.GameBorderData.probabilities;
            double fitness = 0;

            foreach (var curFc in vertex.FaceNeighbors)
            {
                double matWeigth = ComputeWeightMaterial(curFc.Material);
                fitness = fitness + movesProp.weightGoodNumbers * matWeigth * prob[curFc.ProbabilityNumber - 2];
                if (!matWhatIHave.Contains(curFc.Material) && curFc.Material != Game.materials.desert)
                {
                    fitness += movesProp.weightMissingMaterial;
                }
            }

            if (vertex.Port)
            {
                if (vertex.PortMaterial == Game.materials.noMaterial)
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

        private List<Game.materials> WhatIHave(GameProperties gmProp, PlayerProperties plProp)
        {
            List<Game.materials> matWhatIHave = new List<Game.materials>();

            foreach (var curVx in plProp.Village)
            {
                foreach (var curFc in curVx.FaceNeighbors)
                {
                    if (!matWhatIHave.Contains(curFc.Material))
                    {
                        matWhatIHave.Add(curFc.Material);
                    }
                }
            }

            return matWhatIHave;
        }

        private double ComputeWeightMaterial(Game.materials curMat)
        {
            double fitness;
            switch (curMat)
            {
                case Game.materials.brick: fitness = movesProp.weightBrick; break;
                case Game.materials.grain: fitness = movesProp.weightGrain; break;
                case Game.materials.sheep: fitness = movesProp.weightSheep; break;
                case Game.materials.stone: fitness = movesProp.weightStone; break;
                case Game.materials.wood: fitness = movesProp.weightWood; break;
                default: fitness = 0; break;
            }

            return fitness;
        }
    }
}
