using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace osadniciZKatanu
{
    public class Game
    {
        public List<Player> Players { get; private set; } // seznam hráčů
        public Player ActualPlayer { get; private set; } // hráč na tahu
        public Player FirstPlayer { get { if (Players != null) { return Players.First(); } else return null; } } // první hráč v pořadí
        public Player LastPlayer { get { if (Players != null) { return Players.Last(); } else return null; } } // poslední hráč v pořadí

        public GameProperties GmProp; // informace o hře
        
        public bool EndGame { get { return ActualPlayer.PlProp.Points >= GmProp.MinPointsToWin; } }
        private Random rand = new Random();

        public enum color { red, blue, yellow, white, noColor };
        public enum materials { brick, grain, sheep, stone, wood, desert, noMaterial };
        public enum actionCards { coupon, knight, twoRoad, twoMaterials, materialsFromPlayers, noActionCard };
        public enum state { start, firstPhaseOfGame, game, endGame };
        public enum building { road, village, town };

        public Game(List<Player> players, GameProperties gmProp)
        {
            GmProp = gmProp;
            Players = players;
            ActualPlayer = FirstPlayer;
        }

        #region game statue function

        public bool EndOfFirstPhaseOfTheGame()
        {
            bool end = true;
            foreach (Player currentPlayer in Players)
            {
                end = end && currentPlayer.PlProp.FirstVillageCreated && currentPlayer.PlProp.FirstPathCreated &&
                    currentPlayer.PlProp.SecondVillageCreated && currentPlayer.PlProp.SecondPathCreated;
            }
            return end;
        }

        public void NextState()
        {
            if (GmProp.CurrentState != state.endGame)
            {
                GmProp.CurrentState++;
            }
            GmProp.wasBuildSomething = false;
            GmProp.wasUseActionCard = false;
        }

        public void NextPlayer()
        {
            switch (GmProp.CurrentState)
            {
                case state.firstPhaseOfGame: NextFirstPhaseOfTheGame();
                    break;
                case state.game: Next();
                    break;
            }

            GmProp.wasBuildSomething = false;
            GmProp.wasUseActionCard = false;
        }

        private void NextFirstPhaseOfTheGame()
        {
            GmProp.wasBuildSomething = false;
            int cur = Players.FindIndex(x => x == ActualPlayer);

            bool back_way = LastPlayer.PlProp.SecondPathCreated && LastPlayer.PlProp.SecondVillageCreated;

            if (back_way)
            {
                ActualPlayer = Players.ElementAt(cur - 1);
            }
            else
            {
                if (ActualPlayer != LastPlayer)
                {
                    ActualPlayer = Players.ElementAt(cur + 1);
                }
            }
        }

        private void Next()
        {
            int cur = Players.FindIndex(x => x == ActualPlayer);
            if (ActualPlayer == LastPlayer) { ActualPlayer = FirstPlayer; GmProp.Round++; }
            else { ActualPlayer = Players.ElementAt(cur + 1); }
        }

        public void Previous()
        {
            int cur = Players.FindIndex(x => x == ActualPlayer);
            if (ActualPlayer == FirstPlayer) { ActualPlayer = LastPlayer; }
            else { ActualPlayer = Players.ElementAt(cur - 1); }

            ActualPlayer.CurrentAddedMaterials.NullAllMaterials();
        }

        #endregion

        public void GetMaterials(int num)
        {
            List<Face> searchFc = GmProp.GameBorderData.Faces.FindAll(x => x.ProbabilityNumber == num && !x.Thief);

            foreach (Face curFc in searchFc)
            {
                List<Vertex> fcNeigh = curFc.VerticesNeighbors;

                foreach (Vertex curVx in fcNeigh)
                {
                    if (curVx.Village)
                    {
                        Players.Find(x => x.PlProp.Color == curVx.Color).PlProp.Materials.RaiseQuantity(curFc.Material, GmProp.VillageProduction);
                    }
                    else if (curVx.Town)
                    {
                        Players.Find(x => x.PlProp.Color == curVx.Color).PlProp.Materials.RaiseQuantity(curFc.Material, GmProp.TownProduction);
                    }
                }
            }
        }

        public void GetMaterials()
        {
            foreach (Face curFc in GmProp.GameBorderData.Faces)
            {
                List<Vertex> verticesAround = curFc.VerticesNeighbors;
                if (curFc.Material != materials.desert && curFc.Material != materials.noMaterial )
                {
                    foreach (Vertex curVx in verticesAround)
                    {
                        if (curVx.Village)
                        {
                            Players.Find(x => x.PlProp.Color == curVx.Color).PlProp.Materials.RaiseQuantity(curFc.Material, GmProp.VillageProduction);
                        }
                    }
                }
            }

        }

        public void RollTheDice()
        {
            GmProp.FirstDice = rand.Next(1, 6);
            GmProp.SecondDice = rand.Next(1, 6);
            if (GmProp.FirstDice + GmProp.SecondDice == 7)
            {
                GmProp.NeedToMoveThief = true;
            }
        }

        #region move function
        public string MakeMove(Move mvDesc)
        {
            try
            {
                if (GmProp.NeedToMoveThief && !(mvDesc is ThiefMove)) { throw new IncorectMoveException("Need to move thief"); }
                MakeExchangeMove(mvDesc);

                if (mvDesc is FirstPhaseGameMove)
                {
                    MakeFirstPhaseGameMove((FirstPhaseGameMove)mvDesc);
                }
                else if (mvDesc is ThiefMove)
                {
                    MakeThiefMove((ThiefMove)mvDesc);
                }
                else if (mvDesc is BuildRoadMove)
                {
                    MakeBuildRoadMove((BuildRoadMove)mvDesc);
                }
                else if (mvDesc is BuildVillageMove)
                {
                    MakeBuildVillageMove((BuildVillageMove)mvDesc);
                }
                else if (mvDesc is BuildTownMove)
                {
                    MakeBuildTownMove((BuildTownMove)mvDesc);
                }
                else if (mvDesc is BuyActionCardMove)
                {
                    MakeBuyActionCardMove();
                }
                else if (mvDesc is CouponMove)
                {
                    MakeCouponMove();
                }
                else if (mvDesc is KnightMove)
                {
                    MakeKnightMove((KnightMove)mvDesc);
                }
                else if (mvDesc is MaterialFromPlayersMove)
                {
                    MakeMatFromPlayersMove((MaterialFromPlayersMove)mvDesc);
                }
                else if (mvDesc is TwoMaterialsMove)
                {
                    MakeTwoMatMove((TwoMaterialsMove)mvDesc);
                }
                else if (mvDesc is TwoRoadMove)
                {   
                    MakeTwoRoadMove((TwoRoadMove)mvDesc);
                }

                if (ActualPlayer.PlProp.Points >= GmProp.MinPointsToWin) { GmProp.CurrentState = state.endGame; }
                return mvDesc.MoveDescription(GmProp.CurLang);
            }
            catch(Exception ex)
            {
                //TODO: přidat ošetření výjimek
                Console.WriteLine(ex.Message);
                return null;
            }      
        }

        private void MakeExchangeMove(Move mvDesc)
        {
            if (!(mvDesc is FirstPhaseGameMove) && !(mvDesc is ThiefMove))
            {
                foreach (var mat in mvDesc.ChangedMaterials)
                {
                    ChangeMaterial(mat.MatFrom, mat.MatTo);
                }
            }
        }

        private void MakeFirstPhaseGameMove(FirstPhaseGameMove mvDesc)
        {
            if (GmProp.wasBuildSomething) { throw new TooMuchActionsException("this move was some done already"); }

            if (ActualPlayer.PlProp.FirstPathCreated && ActualPlayer.PlProp.FirstVillageCreated &&
                ActualPlayer.PlProp.SecondPathCreated && ActualPlayer.PlProp.SecondPathCreated)
            {
                throw new IncorectMoveException("First and second roads and villages was created already");
            }

            BuildVillage(mvDesc.VillageCoord.Coordinate, true);
            BuildRoad(mvDesc.RoadCoord.CentreCoordinate);

            if (!ActualPlayer.PlProp.FirstPathCreated && !ActualPlayer.PlProp.FirstVillageCreated &&
                !ActualPlayer.PlProp.SecondPathCreated && !ActualPlayer.PlProp.SecondPathCreated)
            {
                ActualPlayer.PlProp.FirstPathCreated = true;
                ActualPlayer.PlProp.FirstVillageCreated = true;
            }
            else if (ActualPlayer.PlProp.FirstPathCreated && ActualPlayer.PlProp.FirstVillageCreated &&
                !ActualPlayer.PlProp.SecondPathCreated && !ActualPlayer.PlProp.SecondPathCreated)
            {
                ActualPlayer.PlProp.SecondPathCreated = true;
                ActualPlayer.PlProp.SecondVillageCreated = true;
            }

            GmProp.wasBuildSomething = true;
        }

        private void MakeThiefMove(ThiefMove mvDesc)
        {
            Face newThiefFace = GmProp.GameBorderData.FindFaceByCoordinate(mvDesc.ThiefCoord.Coordinate);
            if (newThiefFace == GmProp.GameBorderData.noFace) { throw new WrongCoordinateException("Wrong thief coordinate"); }

            newThiefFace.Thief = true;
            GmProp.ThiefFace.Thief = false;
            GmProp.ThiefFace = newThiefFace;
            DropHalfMaterialsFromPlayers();

            if (mvDesc.RobbedPlayer != color.noColor)
            {
                if (newThiefFace.VerticesNeighbors.Find(x => x.Color == mvDesc.RobbedPlayer) == null)
                {
                    throw new WrongPlayerToRobbedException("Wrong Player to roobed exception");
                }
                if (mvDesc.RobbedPlayer == ActualPlayer.PlProp.Color) 
                {
                    throw new WrongPlayerToRobbedException("Wrong Player to roobed exception");
                }
                Player robbedPl = Players.Find(x => x.PlProp.Color == mvDesc.RobbedPlayer);
                materials delMat = robbedPl.PlProp.Materials.PickRandomMaterial();
                if (delMat != materials.noMaterial)
                {
                    ActualPlayer.PlProp.Materials.AddMaterial(delMat);
                    robbedPl.PlProp.Materials.DeleteMaterial(delMat);
                }
            }
            GmProp.NeedToMoveThief = false;
        }

        private void MakeBuildRoadMove(BuildRoadMove mvDesc)
        {
            if (GmProp.wasBuildSomething) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.PlProp.Materials.DeleteMaterials(GmProp.MaterialsForRoad);
            BuildRoad(mvDesc.BuildingCoord.CentreCoordinate);
            GmProp.wasBuildSomething = true;   
        }

        private void MakeBuildVillageMove(BuildVillageMove mvDesc)
        {
            if (GmProp.wasBuildSomething) { throw new TooMuchActionsException("this kind of move was done already"); }
            ActualPlayer.PlProp.Materials.DeleteMaterials(GmProp.MaterialsForVillage);
            BuildVillage(mvDesc.BuildingCoord.Coordinate, false);
            GmProp.wasBuildSomething = true;
        }

        private void MakeBuildTownMove(BuildTownMove mvDesc)
        {
            if (GmProp.wasBuildSomething) { throw new TooMuchActionsException("this kind of move was done already"); }
            ActualPlayer.PlProp.Materials.DeleteMaterials(GmProp.MaterialsForTown);
            BuildTown(mvDesc.BuildingCoord.Coordinate);
            GmProp.wasBuildSomething = true;
        }

        private void MakeBuyActionCardMove()
        {
            if (GmProp.RemainingActionCards.GetSumAllActionCard() <= 0) { throw new NoActionCardException("No action card in packed"); }
            if (GmProp.wasBuildSomething) { throw new TooMuchActionsException("this move was some done already"); }

            ActualPlayer.PlProp.Materials.DeleteMaterials(GmProp.MaterialsForActionCard);

            Game.actionCards pickAct = GmProp.RemainingActionCards.PickRandomActionCard();
            GmProp.RemainingActionCards.DeleteActionCard(pickAct);
            ActualPlayer.PlProp.ActionCards.AddActionCard(pickAct);

            GmProp.wasBuildSomething = true;
        }

        private void MakeCouponMove()
        {
            if (GmProp.wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.PlProp.ActionCards.DeleteActionCard(actionCards.coupon);
            ActualPlayer.PlProp.Points++;
            GmProp.wasUseActionCard = true;
        }

        private void MakeKnightMove(KnightMove mvDesc)
        {
            if (GmProp.wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.PlProp.ActionCards.DeleteActionCard(actionCards.knight);
            ActualPlayer.PlProp.Knights++;
            if (ActualPlayer.PlProp.Knights > GmProp.MaxKnights)
            {
                Player prevLargestArmy = Players.Find(x => x.PlProp.LargestArmy == true);
                if (prevLargestArmy != null)
                {
                    prevLargestArmy.PlProp.LargestArmy = false;
                    prevLargestArmy.PlProp.Points -= GmProp.LargestArmyProduction;
                }
                ActualPlayer.PlProp.LargestArmy = true;
                ActualPlayer.PlProp.Points += GmProp.LargestArmyProduction;
                GmProp.MaxKnights = ActualPlayer.PlProp.Knights;
            }
            ThiefMove thiefMove = new ThiefMove(mvDesc.ThiefCoord, mvDesc.RobbedPlayer);
            MakeThiefMove(thiefMove);
            GmProp.wasUseActionCard = true;
        }

        private void MakeMatFromPlayersMove(MaterialFromPlayersMove mvDesc)
        {
            if (GmProp.wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.PlProp.ActionCards.DeleteActionCard(actionCards.materialsFromPlayers);
            int matNum = 0;
            foreach (Player curPl in Players)
            {
                if (curPl != ActualPlayer)
                {
                    matNum += curPl.PlProp.Materials.GetQuantity(mvDesc.PickedMaterial);
                    curPl.PlProp.Materials.SetQuantity(mvDesc.PickedMaterial, 0);
                }
            }            
            ActualPlayer.PlProp.Materials.RaiseQuantity(mvDesc.PickedMaterial, matNum);
            GmProp.wasUseActionCard = true;
        }

        private void MakeTwoMatMove(TwoMaterialsMove mvDesc)
        {
            if (GmProp.wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.PlProp.ActionCards.DeleteActionCard(actionCards.twoMaterials);
            ActualPlayer.PlProp.Materials.AddMaterial(mvDesc.FirstMaterial);
            ActualPlayer.PlProp.Materials.AddMaterial(mvDesc.SecondMaterial);
            GmProp.wasUseActionCard = true;
        }

        private void MakeTwoRoadMove(TwoRoadMove mvDesc)
        {
            if (GmProp.wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.PlProp.ActionCards.DeleteActionCard(actionCards.twoRoad);
            BuildRoad(mvDesc.FirstRoad.CentreCoordinate);
            BuildRoad(mvDesc.SecondRoad.CentreCoordinate);
            GmProp.wasUseActionCard = true;
        }

        private void ChangeMaterial(materials from, materials to)
        {
            if (ActualPlayer.PlProp.PortForMaterial.Contains(from))
            {
                ActualPlayer.PlProp.Materials.DecreaseQuantity(from, GmProp.SpecialPortRate);
            }
            else if (ActualPlayer.PlProp.UniversalPort)
            {
                ActualPlayer.PlProp.Materials.DecreaseQuantity(from, GmProp.UniversalPortRate);
            }
            else
            {
                ActualPlayer.PlProp.Materials.DecreaseQuantity(from, GmProp.NoPortRate);
            }

            ActualPlayer.PlProp.Materials.AddMaterial(to);
        }

        private void DropHalfMaterialsFromPlayers()
        {
            foreach (Player curPlayer in Players)
            {
                if (curPlayer.PlProp.Materials.GetSumAllMaterial() > 7)
                {
                    int sum = curPlayer.PlProp.Materials.GetSumAllMaterial();
                    int curSum = 0;
                    while (curSum < sum / 2)
                    {
                        materials delMat = curPlayer.PlProp.Materials.PickRandomMaterial();
                        curPlayer.PlProp.Materials.DeleteMaterial(delMat);
                        curSum++;
                    }
                }
            }
        }

        private void BuildRoad(Coord now)
        {
            if (ActualPlayer.PlProp.RoadRemaining <= 0) { throw new NoRoadLeftException("Player has no roads to build"); }
            Edge addRoad = GmProp.GameBorderData.FindEdgeByCoordinate(now);
            if (addRoad == GmProp.GameBorderData.noEdge) { throw new WrongCoordinateException("Wrong building coordinate"); }
            if (addRoad.Road) { throw new BuildingCollisionException("On this place is some building"); }
            if (!addRoad.IsHereAdjacentRoadWithColor(ActualPlayer.PlProp.Color) &&
                !addRoad.IsHereAdjectedVillageWithColor(ActualPlayer.PlProp.Color)) { throw new WrongLocationForBuildingException("Wrong location for build road"); }

            ActualPlayer.AddRoad(addRoad);

            if (ActualPlayer.PlProp.LongestWayLength > GmProp.LongestRoad)
            {
                Player prevLongestRoad = Players.Find(x => x.PlProp.LongestWay);
                if (prevLongestRoad != null) 
                {
                    prevLongestRoad.PlProp.Points -= GmProp.LongestRoadProduction;
                    prevLongestRoad.PlProp.LongestWay = false;
                }
                ActualPlayer.PlProp.Points += GmProp.LongestRoadProduction;
                ActualPlayer.PlProp.LongestWay = true;
                GmProp.LongestRoad = ActualPlayer.PlProp.LongestWayLength;
            }
        }

        private void BuildVillage(Coord now, bool firstOrSecondVillage)
        {
            if (ActualPlayer.PlProp.VillageRemaining <= 0) { throw new NoVillageLeftException("Player has no village to build"); }
            Vertex addVillage = GmProp.GameBorderData.FindVerticesByCoordinate(now);
            if (addVillage == GmProp.GameBorderData.noVertex) { throw new WrongCoordinateException("Wrong building coordinate"); }
            if (addVillage.Building) { throw new BuildingCollisionException("On this place is some building"); }
            if (addVillage.IsHereBuildingInNeighbour()) { throw new WrongLocationForBuildingException("Wrong location for build road"); }
            if (!firstOrSecondVillage && !addVillage.IsHereAdjectedRoadWithColor(ActualPlayer.PlProp.Color)) { throw new WrongLocationForBuildingException("Wrong location for build road"); }

            ActualPlayer.AddVillage(addVillage);
            ActualPlayer.PlProp.Points += GmProp.VillageProduction;

            if (addVillage.Port)
            {
                if (addVillage.PortMaterial == materials.noMaterial)
                {
                    ActualPlayer.PlProp.UniversalPort = true;
                }
                else
                {
                    ActualPlayer.AddPort(addVillage.PortMaterial);
                }
            }
        }

        private void BuildTown(Coord now)
        {
            if (ActualPlayer.PlProp.TownRemaining <= 0) { throw new NoTownLeftException("Player has no town to build"); }
            Vertex addTown = GmProp.GameBorderData.FindVerticesByCoordinate(now);
            if (addTown == GmProp.GameBorderData.noVertex) { throw new WrongCoordinateException("Wrong building coordinate"); }
            if (!addTown.Village || !(addTown.Color==ActualPlayer.PlProp.Color)) { throw new WrongLocationForBuildingException("Actual player has no village on this location"); }
            
            ActualPlayer.AddTown(addTown);
            ActualPlayer.PlProp.Points -= GmProp.VillageProduction;
            ActualPlayer.PlProp.Points += GmProp.TownProduction;
        }
        #endregion


        public static materials RecogniseMaterials(string strMaterials)
        {
            //TODO: přidat aby to nebylo case sensitive, tedy aby při vstupu např. GraIN vrátila funkce materials.grain
            materials portMaterials;
            switch (strMaterials)
            {
                case "brick": portMaterials = materials.brick; break;
                case "grain": portMaterials = materials.grain; break;
                case "desert": portMaterials = materials.desert; break;
                case "noMaterial": portMaterials = materials.noMaterial; break;
                case "sheep": portMaterials = materials.sheep; break;
                case "stone": portMaterials = materials.stone; break;
                case "wood": portMaterials = materials.wood; break;
                default: portMaterials = materials.noMaterial; break;
            }

            return portMaterials;
        }

        public static actionCards RecogniseActionCard(string strActionCard)
        {
            //TODO: přidat aby to nebylo case sensitive
            actionCards act;
            switch (strActionCard)
            {
                case "coupon": act = actionCards.coupon; break;
                case "knight": act = actionCards.knight; break;
                case "materialsFromPlayers": act = actionCards.materialsFromPlayers; break;
                case "noActionCard": act = actionCards.noActionCard; break;
                case "twoMaterials": act = actionCards.twoMaterials; break;
                case "twoRoad": act = actionCards.twoRoad; break;
                default: act = actionCards.noActionCard; break;
            }

            return act;
        }
    }
}
