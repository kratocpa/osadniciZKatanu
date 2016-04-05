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

        GameProperties gmProp;
        PlayerProperties plProp;

        public GenerateUseActionCardMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
            common = new CommonFeatures();
        }

        public GenerateUseActionCardMoves(GenerateMovesProperties movesProp, GameProperties gmProp, PlayerProperties plProp)
        {
            this.movesProp = movesProp;
            this.gmProp = gmProp;
            this.plProp = plProp;
            exchange = new GenerateExchangeMoves(movesProp, gmProp, plProp);
            common = new CommonFeatures(movesProp, gmProp, plProp);
        }


        public List<Move> Generate()
        {
            List<Move> possibleMoves = new List<Move>();
            List<Move> toAdd = new List<Move>();

            if (!gmProp.wasUseActionCard)
            {
                foreach (var curAct in plProp.ActionCards.ActionCards)
                {
                    if (curAct.Quantity > 0)
                    {
                        switch (curAct.ActionCardType)
                        {
                            case Game.actionCards.coupon: GenerateUseCouponActionCard(possibleMoves); break;
                            case Game.actionCards.knight: GenerateUseKnightActionCard(possibleMoves); break;
                            case Game.actionCards.materialsFromPlayers: GenerateUseMatFromPlActionCard(possibleMoves); break;
                            case Game.actionCards.twoMaterials: GenerateUseTwoMatActionCard(possibleMoves); break;
                            case Game.actionCards.twoRoad: GenerateUseTwoRoadActionCard(possibleMoves); break;
                        }

                        
                    }
                }

            }
            return possibleMoves;
        }

        private void GenerateUseTwoRoadActionCard(List<Move> possibleMove)
        {
            //List<TwoRoadMove> possibleTwoRoadMoves = new List<TwoRoadMove>();
            if (plProp.RoadRemaining > 1)
            {
                List<Edge> possibleEdges, possibleEdgesSec;
                double fitnessMove = 0;
                double fitnessFirstPart = 0;
                Edge firstRoad, secondRoad;

                possibleEdges = common.GeneratePossibleEdgesToBuildRoad();
                foreach (var curEg in possibleEdges)
                {
                    firstRoad = curEg;
                    fitnessMove = common.RateRoad(firstRoad);
                    fitnessFirstPart = fitnessMove;
                    plProp.Road.Add(firstRoad);
                    possibleEdgesSec = common.GeneratePossibleEdgesToBuildRoad();
                    foreach (var curEgSec in possibleEdgesSec)
                    {
                        if (curEgSec != curEg)
                        {
                            secondRoad = curEgSec;
                            fitnessMove += common.RateRoad(secondRoad);
                            TwoRoadMove mvDesc = new TwoRoadMove(firstRoad, secondRoad);
                            fitnessMove *= movesProp.weightUseTwoRoadGeneral;
                            mvDesc.fitnessMove = fitnessMove;
                            possibleMove.Add(mvDesc);
                            fitnessMove = fitnessFirstPart;
                        }
                    }
                    plProp.Road.Remove(firstRoad);
                }
            }
        }

        private void GenerateUseCouponActionCard(List<Move> possibleMove)
        {
            //List<CouponMove> possibleCouponMoves = new List<CouponMove>();
            CouponMove mvDesc = new CouponMove();
            mvDesc.fitnessMove = RateUseActionCard();
            possibleMove.Add(mvDesc);
        }

        private void GenerateUseKnightActionCard(List<Move> possibleMove)
        {
            //List<KnightMove> possibleMoves = new List<KnightMove>();

            foreach (var curFc in gmProp.GameBorderData.Faces)
            {
                foreach (var curMv in ComputeMoveKnightFaceFitness(curFc))
                {
                    possibleMove.Add(curMv);
                }
            }
        }

        private List<KnightMove> ComputeMoveKnightFaceFitness(Face curFc)
        {
            double[] prob = gmProp.GameBorderData.probabilities;
            int countOfMyBuilding = 0;
            int countOfOtherBuilding = 0;
            List<Game.color> colors = new List<Game.color>();
            List<KnightMove> result = new List<KnightMove>();

            foreach (var curVx in curFc.VerticesNeighbors)
            {
                if (curVx.Building)
                {
                    if (curVx.Color == plProp.Color)
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

        private void GenerateUseMatFromPlActionCard(List<Move> possibleMove)
        {
            //List<MaterialFromPlayersMove> possibleMatFromPlMoves = new List<MaterialFromPlayersMove>();

            foreach (Game.materials curMat in Enum.GetValues(typeof(Game.materials)))
            {
                if (curMat != Game.materials.desert && curMat != Game.materials.noMaterial)
                {
                    MaterialFromPlayersMove mvDesc = new MaterialFromPlayersMove(curMat);
                    mvDesc.fitnessMove = RateMateFromPl(curMat);
                    possibleMove.Add(mvDesc);
                }
            }
        }

        private double RateMateFromPl(Game.materials curMat)
        {
            return movesProp.weightUseMatFromPlGeneral / (plProp.Materials.GetQuantity(curMat) + 1);
        }

        private void GenerateUseTwoMatActionCard(List<Move> possibleMove)
        {
            //List<TwoMaterialsMove> possibleMatFromPlMoves = new List<TwoMaterialsMove>();
            TwoMaterialsMove mvDesc;

            if (!gmProp.wasBuildSomething)
            {
                if (exchange.CountOfMissingMaterials(gmProp.MaterialsForRoad, plProp.Materials) <= 2)
                {
                    mvDesc = MakeMoveTwoMatActionCard(gmProp.MaterialsForRoad);
                    mvDesc.fitnessMove = movesProp.weightUseTwoMatRoadGeneral;
                    possibleMove.Add(mvDesc);
                }
                if (exchange.CountOfMissingMaterials(gmProp.MaterialsForVillage, plProp.Materials) <= 2)
                {
                    mvDesc = MakeMoveTwoMatActionCard(gmProp.MaterialsForVillage);
                    mvDesc.fitnessMove = movesProp.weightUseTwoMatVillageGeneral;
                    possibleMove.Add(mvDesc);
                }
                if (exchange.CountOfMissingMaterials(gmProp.MaterialsForTown, plProp.Materials) <= 2)
                {
                    mvDesc = MakeMoveTwoMatActionCard(gmProp.MaterialsForTown);
                    mvDesc.fitnessMove = movesProp.weightUseTwoMatTownGeneral;
                    possibleMove.Add(mvDesc);
                }
                if (exchange.CountOfMissingMaterials(gmProp.MaterialsForActionCard, plProp.Materials) <= 2)
                {
                    mvDesc = MakeMoveTwoMatActionCard(gmProp.MaterialsForActionCard);
                    mvDesc.fitnessMove = movesProp.weightUseTwoMatActGeneral;
                    possibleMove.Add(mvDesc);
                }
            }
        }

        private TwoMaterialsMove MakeMoveTwoMatActionCard(MaterialCollection whatIWant)
        {
            Game.materials firstMat = Game.materials.noMaterial;
            Game.materials secondMat = Game.materials.noMaterial;

            foreach (var curMat in whatIWant.Materials)
            {
                int myQuant = plProp.Materials.GetQuantity(curMat.MaterialType);
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
                        if (firstMat == Game.materials.noMaterial)
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
            if (firstMat == Game.materials.noMaterial)
            {
                firstMat = Game.materials.brick;
            }
            if (secondMat == Game.materials.noMaterial)
            {
                secondMat = Game.materials.brick;
            }

            TwoMaterialsMove mvDesc = new TwoMaterialsMove(firstMat, secondMat);
            return mvDesc;
        }

        private double RateUseActionCard()
        {
            return movesProp.weightUseActionCardGeneral;
        }
    }
}
