using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    class GenerateUseActionCardMoves
    {
        GenerateMovesProperties movesProp;
        GenerateExchangeMoves exchange;
        CommonFeatures common;

        public GenerateUseActionCardMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
            common = new CommonFeatures();
        }

        public GenerateUseActionCardMoves(GenerateMovesProperties movesProp)
        {
            this.movesProp = movesProp;
            exchange = new GenerateExchangeMoves(movesProp);
            common = new CommonFeatures(movesProp);
        }


        public List<Move> Generate(GameDesc gmDesc)
        {
            List<Move> possibleMoves = new List<Move>();
            List<Move> toAdd = new List<Move>();

            if (!gmDesc.wasUseActionCard)
            {
                foreach (var curAct in gmDesc.ActualPlayerDesc.ActionCardsDesc.ActionCards)
                {
                    if (curAct.Quantity > 0)
                    {
                        switch (curAct.ActionCardType)
                        {
                            case GameDesc.actionCards.coupon: GenerateUseCouponActionCard(gmDesc, possibleMoves); break;
                            case GameDesc.actionCards.knight: GenerateUseKnightActionCard(gmDesc, possibleMoves); break;
                            case GameDesc.actionCards.materialsFromPlayers: GenerateUseMatFromPlActionCard(gmDesc, possibleMoves); break;
                            case GameDesc.actionCards.twoMaterials: GenerateUseTwoMatActionCard(gmDesc, possibleMoves); break;
                            case GameDesc.actionCards.twoRoad: GenerateUseTwoRoadActionCard(gmDesc, possibleMoves); break;
                        }

                        
                    }
                }

            }
            return possibleMoves;
        }

        private void GenerateUseTwoRoadActionCard(GameDesc gmDesc, List<Move> possibleMove)
        {
            //List<TwoRoadMove> possibleTwoRoadMoves = new List<TwoRoadMove>();
            if (gmDesc.ActualPlayerDesc.RoadRemaining > 1)
            {
                List<EdgeDesc> possibleEdges, possibleEdgesSec;
                double fitnessMove = 0;
                EdgeDesc firstRoad, secondRoad;

                possibleEdges = common.GeneratePossibleEdgesToBuildRoad(gmDesc);
                foreach (var curEg in possibleEdges)
                {
                    firstRoad = curEg;
                    fitnessMove = common.RateRoad(firstRoad, gmDesc);
                    gmDesc.ActualPlayerDesc.RoadDesc.Add(firstRoad);
                    possibleEdgesSec = common.GeneratePossibleEdgesToBuildRoad(gmDesc);
                    foreach (var curEgSec in possibleEdgesSec)
                    {
                        if (curEgSec != curEg)
                        {
                            secondRoad = curEgSec;
                            fitnessMove += common.RateRoad(secondRoad, gmDesc);
                            TwoRoadMove mvDesc = new TwoRoadMove(firstRoad, secondRoad);
                            fitnessMove = movesProp.weightUseTwoRoadGeneral;
                            mvDesc.fitnessMove = fitnessMove;
                            possibleMove.Add(mvDesc);
                        }
                    }
                    gmDesc.ActualPlayerDesc.RoadDesc.Remove(firstRoad);
                }
            }
        }

        private void GenerateUseCouponActionCard(GameDesc gmDesc, List<Move> possibleMove)
        {
            //List<CouponMove> possibleCouponMoves = new List<CouponMove>();
            CouponMove mvDesc = new CouponMove();
            mvDesc.fitnessMove = RateUseActionCard(mvDesc, gmDesc);
            possibleMove.Add(mvDesc);
        }

        private void GenerateUseKnightActionCard(GameDesc gmDesc, List<Move> possibleMove)
        {
            //List<KnightMove> possibleMoves = new List<KnightMove>();

            foreach (var curFc in gmDesc.GameBorderDesc.facesDesc)
            {
                foreach (var curMv in ComputeMoveKnightFaceFitness(curFc, gmDesc))
                {
                    possibleMove.Add(curMv);
                }
            }
        }

        private List<KnightMove> ComputeMoveKnightFaceFitness(FaceDesc curFc, GameDesc gmDesc)
        {
            double[] prob = gmDesc.GameBorderDesc.probabilities;
            int countOfMyBuilding = 0;
            int countOfOtherBuilding = 0;
            List<GameDesc.color> colors = new List<GameDesc.color>();
            List<KnightMove> result = new List<KnightMove>();

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
                    KnightMove mvDesc = new KnightMove(curFc, col);
                    mvDesc.fitnessMove = ((movesProp.weightThiefMoveBase + countOfOtherBuilding * movesProp.weightThiefMoveOtherBuilding) / (countOfMyBuilding * movesProp.weightThiefMoveMyBuilding + 1)) * prob[curFc.ProbabilityNumber - 2] * movesProp.weightThiefMoveProb;
                    result.Add(mvDesc);
                }
            }
            else
            {
                KnightMove mvDesc = new KnightMove(curFc);
                mvDesc.fitnessMove = movesProp.weightThiefMoveBase;
                result.Add(mvDesc);
            }

            return result;
        }

        private void GenerateUseMatFromPlActionCard(GameDesc gmDesc, List<Move> possibleMove)
        {
            //List<MaterialFromPlayersMove> possibleMatFromPlMoves = new List<MaterialFromPlayersMove>();

            foreach (GameDesc.materials curMat in Enum.GetValues(typeof(GameDesc.materials)))
            {
                if (curMat != GameDesc.materials.desert && curMat != GameDesc.materials.noMaterial)
                {
                    MaterialFromPlayersMove mvDesc = new MaterialFromPlayersMove(curMat);
                    mvDesc.fitnessMove = RateMateFromPl(curMat, gmDesc);
                    possibleMove.Add(mvDesc);
                }
            }
        }

        private double RateMateFromPl(GameDesc.materials curMat, GameDesc gmDesc)
        {
            return movesProp.weightUseMatFromPlGeneral / (gmDesc.ActualPlayerDesc.MaterialsDesc.GetQuantity(curMat) + 1);
        }

        private void GenerateUseTwoMatActionCard(GameDesc gmDesc, List<Move> possibleMove)
        {
            //List<TwoMaterialsMove> possibleMatFromPlMoves = new List<TwoMaterialsMove>();
            TwoMaterialsMove mvDesc;

            if (!gmDesc.wasBuildSomething)
            {
                if (exchange.CountOfMissingMaterials(gmDesc.materialForRoadDesc, gmDesc.ActualPlayerDesc.MaterialsDesc) <= 2)
                {
                    mvDesc = MakeMoveTwoMatActionCard(gmDesc.materialForRoadDesc, gmDesc);
                    mvDesc.fitnessMove = movesProp.weightUseTwoMatRoadGeneral;
                    possibleMove.Add(mvDesc);
                }
                if (exchange.CountOfMissingMaterials(gmDesc.materialForVillageDesc, gmDesc.ActualPlayerDesc.MaterialsDesc) <= 2)
                {
                    mvDesc = MakeMoveTwoMatActionCard(gmDesc.materialForVillageDesc, gmDesc);
                    mvDesc.fitnessMove = movesProp.weightUseTwoMatVillageGeneral;
                    possibleMove.Add(mvDesc);
                }
                if (exchange.CountOfMissingMaterials(gmDesc.materialForTownDesc, gmDesc.ActualPlayerDesc.MaterialsDesc) <= 2)
                {
                    mvDesc = MakeMoveTwoMatActionCard(gmDesc.materialForTownDesc, gmDesc);
                    mvDesc.fitnessMove = movesProp.weightUseTwoMatTownGeneral;
                    possibleMove.Add(mvDesc);
                }
                if (exchange.CountOfMissingMaterials(gmDesc.materialForActionCardDesc, gmDesc.ActualPlayerDesc.MaterialsDesc) <= 2)
                {
                    mvDesc = MakeMoveTwoMatActionCard(gmDesc.materialForActionCardDesc, gmDesc);
                    mvDesc.fitnessMove = movesProp.weightUseTwoMatActGeneral;
                    possibleMove.Add(mvDesc);
                }
            }
        }

        private TwoMaterialsMove MakeMoveTwoMatActionCard(MaterialCollectionDesc whatIWant, GameDesc gmDesc)
        {
            GameDesc.materials firstMat = GameDesc.materials.noMaterial;
            GameDesc.materials secondMat = GameDesc.materials.noMaterial;

            foreach (var curMat in whatIWant.Materials)
            {
                int myQuant = gmDesc.ActualPlayerDesc.MaterialsDesc.GetQuantity(curMat.MaterialType);
                int whatIWantQuant = curMat.Quantity;
                if (whatIWantQuant > myQuant)
                {
                    if (whatIWantQuant - myQuant == 2)
                    {
                        firstMat = curMat.MaterialType;
                        secondMat = curMat.MaterialType;
                        break;
                    }
                    else
                    {
                        if (firstMat == GameDesc.materials.noMaterial)
                        {
                            firstMat = curMat.MaterialType;
                        }
                        else
                        {
                            secondMat = curMat.MaterialType;
                            break;
                        }
                    }
                }
            }

            #warning lepsi algoritmus na vybrani druhe suroviny
            if (firstMat == GameDesc.materials.noMaterial)
            {
                firstMat = GameDesc.materials.brick;
            }
            if (secondMat == GameDesc.materials.noMaterial)
            {
                secondMat = GameDesc.materials.brick;
            }

            TwoMaterialsMove mvDesc = new TwoMaterialsMove(firstMat, secondMat);
            return mvDesc;
        }

        private double RateUseActionCard(Move mvDesc, GameDesc gmDesc)
        {
            return movesProp.weightUseActionCardGeneral;
        }
    }
}
