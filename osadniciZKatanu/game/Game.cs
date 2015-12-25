using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace osadniciZKatanu
{
    public class Game : GameDesc
    {
        public List<Player> Players { get; private set; } // seznam hráčů
        public Player ActualPlayer { get; private set; } // hráč na tahu
        public Player FirstPlayer { get; private set; } // první hráč v pořadí
        public Player LastPlayer { get; private set; } // poslední hráč v pořadí

        public Face ThiefFace { get; private set; } // stěna, na který je zloděj

        public ActionCardCollection RemainingActionCards { get; private set; } // seznam zbývajících akčních karet
        public MaterialCollection materialForVillage; // suroviny potřebné na stavbu vesnice
        public MaterialCollection materialForTown; // suroviny potřebné na stavbu města
        public MaterialCollection materialForActionCard; // suroviny potřebné na koupi akční karty
        public MaterialCollection materialForRoad; // suroviny potřebné na stavbu cesty
        
        public GameBorder GameBorderData; // popis hrací desky
        public ILanguage CurLang { get; private set; } // jazyk hry
        public bool EndGame { get { return ActualPlayer.Points >= MinPointsToWin; } }

        private Random rand = new Random();

        public Game(List<Player> players, bool isRandomGameBorder, ILanguage curLang, GameProperties gmProp)
            : base(gmProp)
        {
            RemainingActionCards = (ActionCardCollection)gmProp.RemainingActionCards.Clone();
            materialForRoad = (MaterialCollection)gmProp.MaterialsForRoad.Clone();
            materialForVillage = (MaterialCollection)gmProp.MaterialsForVillage.Clone();
            materialForTown = (MaterialCollection)gmProp.MaterialsForTown.Clone();
            materialForActionCard = (MaterialCollection)gmProp.MaterialsForActionCard.Clone();

            SetGameBorder st = new SetGameBorder();
            GameBorderData = st.GenerateGameBorder(isRandomGameBorder, gmProp);

            CurLang = curLang;
            Players = players;

            ThiefFace = GameBorderData.Faces.Find(x => x.Material == materials.desert);
            ThiefFace.Thief = true;

            FirstPlayer = Players.First();
            LastPlayer = Players.Last();
            ActualPlayer = FirstPlayer;

            wasBuildSomething = false;
            wasUseActionCard = false;
        }

        public void Sychronize()
        {
            PlayersDesc.Clear();
            foreach (Player curPl in Players)
            {
                curPl.Synchronize();
                PlayersDesc.Add(curPl);
            }

            RemainingActionCardsDesc = RemainingActionCards;
            ActualPlayerDesc = ActualPlayer;
            ThiefFaceDesc = ThiefFace;
            GameBorderDesc = GameBorderData;
        }

        #region game statue function

        public bool EndOfFirstPhaseOfTheGame()
        {
            bool end = true;
            foreach (Player currentPlayer in Players)
            {
                end = end && currentPlayer.FirstVillageCreated && currentPlayer.FirstPathCreated &&
                    currentPlayer.SecondVillageCreated && currentPlayer.SecondPathCreated;
            }
            return end;
        }

        public void NextState()
        {
            if (CurrentState != state.endGame)
            {
                CurrentState++;
            }
            wasBuildSomething = false;
            wasUseActionCard = false;
        }

        public void NextPlayer()
        {
            switch (CurrentState)
            {
                case state.firstPhaseOfGame: NextFirstPhaseOfTheGame();
                    break;
                case state.game: Next();
                    break;
            }

            wasBuildSomething = false;
            wasUseActionCard = false;
        }

        private void NextFirstPhaseOfTheGame()
        {
            wasBuildSomething = false;
            int cur = Players.FindIndex(x => x == ActualPlayer);

            bool back_way = LastPlayer.SecondPathCreated && LastPlayer.SecondVillageCreated;

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
            if (ActualPlayer == LastPlayer) { ActualPlayer = FirstPlayer; Round++; }
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
            List<Face> searchFc = GameBorderData.Faces.FindAll(x => x.ProbabilityNumber == num && !x.Thief);

            foreach (Face curFc in searchFc)
            {
                List<Vertex> fcNeigh = curFc.VerticesNeighbors;

                foreach (Vertex curVx in fcNeigh)
                {
                    if (curVx.Village)
                    {
                        Players.Find(x => x.Color == curVx.Color).Materials.RaiseQuantity(curFc.Material, VillageProduction);
                    }
                    else if (curVx.Town)
                    {
                        Players.Find(x => x.Color == curVx.Color).Materials.RaiseQuantity(curFc.Material, TownProduction);
                    }
                }
            }
        }

        public void GetMaterials()
        {
            foreach (Face curFc in GameBorderData.Faces)
            {
                List<Vertex> verticesAround = curFc.VerticesNeighbors;
                if (curFc.Material != materials.desert && curFc.Material != materials.noMaterial )
                {
                    foreach (Vertex curVx in verticesAround)
                    {
                        if (curVx.Village)
                        {
                            Players.Find(x => x.Color == curVx.Color).Materials.RaiseQuantity(curFc.Material, VillageProduction);
                        }
                    }
                }
            }

        }

        public void RollTheDice()
        {
            FirstDice = rand.Next(1, 6);
            SecondDice = rand.Next(1, 6);
            if (FirstDice + SecondDice == 7)
            {
                NeedToMoveThief = true;
            }
        }

        #region move function
        public string MakeMove(Move mvDesc)
        {
            try
            {
                if (NeedToMoveThief && !(mvDesc is ThiefMove)) { throw new IncorectMoveException("Need to move thief"); }
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

                if (ActualPlayer.Points >= MinPointsToWin) { CurrentState = state.endGame; }
                return mvDesc.MoveDescription(CurLang);
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
            if (wasBuildSomething) { throw new TooMuchActionsException("this move was some done already"); }

            if (ActualPlayer.FirstPathCreated && ActualPlayer.FirstVillageCreated &&
                ActualPlayer.SecondPathCreated && ActualPlayer.SecondPathCreated)
            {
                throw new IncorectMoveException("First and second roads and villages was created already");
            }

            BuildVillage(mvDesc.VillageCoord.Coordinate, true);
            BuildRoad(mvDesc.RoadCoord.CentreCoordinate);

            if (!ActualPlayer.FirstPathCreated && !ActualPlayer.FirstVillageCreated &&
                !ActualPlayer.SecondPathCreated && !ActualPlayer.SecondPathCreated)
            {
                ActualPlayer.FirstPathCreated = true;
                ActualPlayer.FirstVillageCreated = true;
            }
            else if (ActualPlayer.FirstPathCreated && ActualPlayer.FirstVillageCreated &&
                !ActualPlayer.SecondPathCreated && !ActualPlayer.SecondPathCreated)
            {
                ActualPlayer.SecondPathCreated = true;
                ActualPlayer.SecondVillageCreated = true;
            }

            wasBuildSomething = true;
        }

        private void MakeThiefMove(ThiefMove mvDesc)
        {
            Face newThiefFace = GameBorderData.FindFaceByCoordinate(mvDesc.ThiefCoord.Coordinate);
            if (newThiefFace == GameBorderData.noFace) { throw new WrongCoordinateException("Wrong thief coordinate"); }

            newThiefFace.Thief = true;
            ThiefFace.Thief = false;
            ThiefFace = newThiefFace;
            DropHalfMaterialsFromPlayers();

            if (mvDesc.RobbedPlayer != color.noColor)
            {
                if (newThiefFace.VerticesNeighbors.Find(x => x.Color == mvDesc.RobbedPlayer) == null)
                {
                    throw new WrongPlayerToRobbedException("Wrong Player to roobed exception");
                }
                if (mvDesc.RobbedPlayer == ActualPlayer.Color) 
                {
                    throw new WrongPlayerToRobbedException("Wrong Player to roobed exception");
                }
                Player robbedPl = Players.Find(x => x.Color == mvDesc.RobbedPlayer);
                materials delMat = robbedPl.Materials.PickRandomMaterial();
                if (delMat != materials.noMaterial)
                {
                    ActualPlayer.Materials.AddMaterial(delMat);
                    robbedPl.Materials.DeleteMaterial(delMat);
                }
            }
            NeedToMoveThief = false;
        }

        private void MakeBuildRoadMove(BuildRoadMove mvDesc)
        {
            if (wasBuildSomething) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.Materials.DeleteMaterials(materialForRoad);
            BuildRoad(mvDesc.BuildingCoord.CentreCoordinate);
            wasBuildSomething = true;   
        }

        private void MakeBuildVillageMove(BuildVillageMove mvDesc)
        {
            if (wasBuildSomething) { throw new TooMuchActionsException("this kind of move was done already"); }
            ActualPlayer.Materials.DeleteMaterials(materialForVillage);
            BuildVillage(mvDesc.BuildingCoord.Coordinate, false);
            wasBuildSomething = true;
        }

        private void MakeBuildTownMove(BuildTownMove mvDesc)
        {
            if (wasBuildSomething) { throw new TooMuchActionsException("this kind of move was done already"); }
            ActualPlayer.Materials.DeleteMaterials(materialForTown);
            BuildTown(mvDesc.BuildingCoord.Coordinate);
            wasBuildSomething = true;
        }

        private void MakeBuyActionCardMove()
        {
            if (RemainingActionCards.GetSumAllActionCard() <= 0) { throw new NoActionCardException("No action card in packed"); }
            if (wasBuildSomething) { throw new TooMuchActionsException("this move was some done already"); }

            ActualPlayer.Materials.DeleteMaterials(materialForActionCard);

            GameDesc.actionCards pickAct = RemainingActionCards.PickRandomActionCard();
            RemainingActionCards.DeleteActionCard(pickAct);
            ActualPlayer.ActionCards.AddActionCard(pickAct);

            wasBuildSomething = true;
        }

        private void MakeCouponMove()
        {
            if (wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.ActionCards.DeleteActionCard(actionCards.coupon);
            ActualPlayer.Points++;
            wasUseActionCard = true;
        }

        private void MakeKnightMove(KnightMove mvDesc)
        {
            if (wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.ActionCards.DeleteActionCard(actionCards.knight);
            ActualPlayer.Knights++;
            if (ActualPlayer.Knights > MaxKnights)
            {
                Player prevLargestArmy = Players.Find(x => x.LargestArmy == true);
                if (prevLargestArmy != null)
                {
                    prevLargestArmy.LargestArmy = false;
                    prevLargestArmy.Points -= LargestArmyProduction;
                }
                ActualPlayer.LargestArmy = true;
                ActualPlayer.Points += LargestArmyProduction;
                MaxKnights = ActualPlayer.Knights;
            }
            ThiefMove thiefMove = new ThiefMove(mvDesc.ThiefCoord, mvDesc.RobbedPlayer);
            MakeThiefMove(thiefMove);
            wasUseActionCard = true;
        }

        private void MakeMatFromPlayersMove(MaterialFromPlayersMove mvDesc)
        {
            if (wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.ActionCards.DeleteActionCard(actionCards.materialsFromPlayers);
            int matNum = 0;
            foreach (Player curPl in Players)
            {
                if (curPl != ActualPlayer)
                {
                    matNum += curPl.Materials.GetQuantity(mvDesc.PickedMaterial);
                    curPl.Materials.SetQuantity(mvDesc.PickedMaterial, 0);
                }
            }            
            ActualPlayer.Materials.RaiseQuantity(mvDesc.PickedMaterial, matNum);
            wasUseActionCard = true;
        }

        private void MakeTwoMatMove(TwoMaterialsMove mvDesc)
        {
            if (wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.ActionCards.DeleteActionCard(actionCards.twoMaterials);
            ActualPlayer.Materials.AddMaterial(mvDesc.FirstMaterial);
            ActualPlayer.Materials.AddMaterial(mvDesc.SecondMaterial);
            wasUseActionCard = true;
        }

        private void MakeTwoRoadMove(TwoRoadMove mvDesc)
        {
            if (wasUseActionCard) { throw new TooMuchActionsException("this kind of move was done already"); }

            ActualPlayer.ActionCards.DeleteActionCard(actionCards.twoRoad);
            BuildRoad(mvDesc.FirstRoad.CentreCoordinate);
            BuildRoad(mvDesc.SecondRoad.CentreCoordinate);
            wasUseActionCard = true;
        }

        private void ChangeMaterial(materials from, materials to)
        {
            if (ActualPlayer.PortForMaterial.Contains(from))
            {
                ActualPlayer.Materials.DecreaseQuantity(from, SpecialPortRate);
            }
            else if (ActualPlayer.UniversalPort)
            {
                ActualPlayer.Materials.DecreaseQuantity(from, UniversalPortRate);
            }
            else
            {
                ActualPlayer.Materials.DecreaseQuantity(from, NoPortRate);
            }

            ActualPlayer.Materials.AddMaterial(to);
        }

        private void DropHalfMaterialsFromPlayers()
        {
            foreach (Player curPlayer in Players)
            {
                if (curPlayer.Materials.GetSumAllMaterial() > 7)
                {
                    int sum = curPlayer.Materials.GetSumAllMaterial();
                    int curSum = 0;
                    while (curSum < sum / 2)
                    {
                        materials delMat = curPlayer.Materials.PickRandomMaterial();
                        curPlayer.Materials.DeleteMaterial(delMat);
                        curSum++;
                    }
                }
            }
        }

        private void BuildRoad(Coord now)
        {
            if (ActualPlayer.RoadRemaining <= 0) { throw new NoRoadLeftException("Player has no roads to build"); }
            Edge addRoad = GameBorderData.FindEdgeByCoordinate(now);
            if (addRoad == GameBorderData.noEdge) { throw new WrongCoordinateException("Wrong building coordinate"); }
            if (addRoad.Road) { throw new BuildingCollisionException("On this place is some building"); }
            if (!addRoad.IsHereAdjacentRoadWithColor(ActualPlayer.Color) &&
                !addRoad.IsHereAdjectedVillageWithColor(ActualPlayer.Color)) { throw new WrongLocationForBuildingException("Wrong location for build road"); }

            ActualPlayer.AddRoad(addRoad);

            if (ActualPlayer.LongestWayLength > LongestRoad)
            {
                Player prevLongestRoad = Players.Find(x => x.LongestWay);
                if (prevLongestRoad != null) 
                {
                    prevLongestRoad.Points -= LongestRoadProduction;
                    prevLongestRoad.LongestWay = false;
                }
                ActualPlayer.Points += LongestRoadProduction;
                ActualPlayer.LongestWay = true;
                LongestRoad = ActualPlayer.LongestWayLength;
            }
        }

        private void BuildVillage(Coord now, bool firstOrSecondVillage)
        {
            if (ActualPlayer.VillageRemaining <= 0) { throw new NoVillageLeftException("Player has no village to build"); }
            Vertex addVillage = GameBorderData.FindVerticesByCoordinate(now);
            if (addVillage == GameBorderData.noVertex) { throw new WrongCoordinateException("Wrong building coordinate"); }
            if (addVillage.Building) { throw new BuildingCollisionException("On this place is some building"); }
            if (addVillage.IsHereBuildingInNeighbour()) { throw new WrongLocationForBuildingException("Wrong location for build road"); }
            if (!firstOrSecondVillage && !addVillage.IsHereAdjectedRoadWithColor(ActualPlayer.Color)) { throw new WrongLocationForBuildingException("Wrong location for build road"); }

            ActualPlayer.AddVillage(addVillage);
            ActualPlayer.Points += VillageProduction;

            if (addVillage.Port)
            {
                if (addVillage.PortMaterial == materials.noMaterial)
                {
                    ActualPlayer.UniversalPort = true;
                }
                else
                {
                    ActualPlayer.AddPort(addVillage.PortMaterial);
                }
            }
        }

        private void BuildTown(Coord now)
        {
            if (ActualPlayer.TownRemaining <= 0) { throw new NoTownLeftException("Player has no town to build"); }
            Vertex addTown = GameBorderData.FindVerticesByCoordinate(now);
            if (addTown == GameBorderData.noVertex) { throw new WrongCoordinateException("Wrong building coordinate"); }
            if (!addTown.Village || !(addTown.Color==ActualPlayer.Color)) { throw new WrongLocationForBuildingException("Actual player has no village on this location"); }
            
            ActualPlayer.AddTown(addTown);
            ActualPlayer.Points -= VillageProduction;
            ActualPlayer.Points += TownProduction;
        }
        #endregion

    }
}
