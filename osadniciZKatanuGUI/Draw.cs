using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using osadniciZKatanu;

namespace osadniciZKatanuGUI
{
    public class Draw
    {
        Canvas canvasGameBorder;
        ListBox materialListbox;
        ListBox actionCardListbox;
        ListBox fsSelectionListbox;
        Label diceLabel;
        Label informationLabel;
        Label pointsLabel;
        Label actionCardsLabel;
        Label materialsLabel;
        Label actualPlayerLabel;
        Label gameEventLabel;
        Label pathLabel;
        Label villageLabel;
        Label townLabel;
        Label actionCardLabel;
        Label roundLabel;
        Label remainingVillageLabel;
        Label remainingTownLabel;
        Label remainingRoadLabel;
        Label remainingActionCardLabel;
        Label woodCountLabel, brickCountLabel, grainCountLabel, sheepCountLabel, stoneCountLabel;
        Button buyActionCardButton;
        Button useActionCardButton;
        Button changeMaterialButton;
        Button startGameButton;
        Image largestArmyImage;
        Image longestWayImage;
        Image remainingVillageImage;
        Image remainingTownImage;
        Image remainingActionCardImage;
        Line remainingRoadLine;
        List<village> villageList;
        List<town> townList;
        List<road> roadList;
        thief thief_;

        ILanguage curLang;

        class thief
        {
            public Image thiefImage;
            public FaceDesc thiefLocation;
            public static int thiefSize = 70;
        }

        class village
        {
            public Image villageImage;
            public VertexDesc villageLocation;
            public village(Image villageImage_, VertexDesc villageLocation_)
            {
                villageImage = villageImage_;
                villageLocation = villageLocation_;
            }
            public static int villageSize = 25;
        }

        class town
        {
            public Image townImage;
            public VertexDesc townLocation;
            public town(Image townImage_, VertexDesc townLocation_)
            {
                townImage = townImage_;
                townLocation = townLocation_;
            }
            public static int townSize = 25;
        }

        class road
        {
            public Line roadLine;
            public EdgeDesc roadLocation;
            public road(Line roadLine_, EdgeDesc roadLocation_)
            {
                roadLine = roadLine_;
                roadLocation = roadLocation_;
            }
        }

        public Draw(Canvas canvasGameBorder_, 
            Button useActionCardButton_, Button changeMaterialButton_, Button buyActionCardButton_, Button startGameButton_,
            ListBox materialListbox_, ListBox actionCardListbox_, ListBox fsSelectionListbox_,
            Label pointsLabel_, Label actionCardsLabel_, Label materialsLabel_, Label diceLabel_, Label informationLabel_,
            Label pathLabel_, Label townLabel_, Label villageLabel_, Label actionCardLabel_, Label actualPlayerLabel_,
            Label gameEventLabel_, Label roundLabel_, Label remainingVillageLabel_, Label remainingTownLabel_, Label remainingRoadLabel_,
            Label remainingActionCardLabel_, Label woodCountLabel_, Label brickCountLabel_, Label grainCountLabel_, Label sheepCountLabel_, Label stoneCountLabel_,
            Image largestArmyImage_, Image longestWayImage_, Image remainingVillageImage_, Image remainingTownImage_, Image remainingActionCardImage_,
            Line remainingRoadLine_,
            ILanguage curLang_)
        {
            informationLabel = informationLabel_;
            canvasGameBorder = canvasGameBorder_;
            roundLabel = roundLabel_;

            materialListbox = materialListbox_;
            actionCardListbox = actionCardListbox_;
            fsSelectionListbox = fsSelectionListbox_;
            
            useActionCardButton = useActionCardButton_;
            buyActionCardButton = buyActionCardButton_;
            changeMaterialButton = changeMaterialButton_;
            startGameButton = startGameButton_;
            
            pointsLabel = pointsLabel_;
            diceLabel = diceLabel_;
            actualPlayerLabel = actualPlayerLabel_;
            actionCardsLabel = actionCardsLabel_;
            materialsLabel = materialsLabel_;
            gameEventLabel = gameEventLabel_;

            woodCountLabel = woodCountLabel_;
            brickCountLabel = brickCountLabel_;
            grainCountLabel = grainCountLabel_;
            sheepCountLabel = sheepCountLabel_;
            stoneCountLabel = stoneCountLabel_;

            largestArmyImage = largestArmyImage_;
            longestWayImage = longestWayImage_;
            remainingVillageImage = remainingVillageImage_;
            remainingTownImage = remainingTownImage_;
            remainingActionCardImage = remainingActionCardImage_;

            remainingRoadLine = remainingRoadLine_;

            villageList = new List<village>();
            townList = new List<town>();
            roadList = new List<road>();
            thief_ = new thief();
            curLang = curLang_;

            pathLabel = pathLabel_;
            villageLabel = villageLabel_;
            townLabel = townLabel_;
            actionCardLabel = actionCardLabel_;

            remainingVillageLabel = remainingVillageLabel_;
            remainingTownLabel = remainingTownLabel_;
            remainingRoadLabel = remainingRoadLabel_;
            remainingActionCardLabel = remainingActionCardLabel_;

            pathLabel.Content = curLang.MaterialForRoad();
            villageLabel.Content = curLang.MaterialForVillage();
            townLabel.Content = curLang.MaterialForTown();
            actionCardLabel.Content = curLang.MaterialForActionCard();

            useActionCardButton.Content = curLang.UseActionCardButton();
            changeMaterialButton.Content = curLang.ChangeMaterialButton();
            buyActionCardButton.Content = curLang.BuyActionCardButton();
            startGameButton.Content = curLang.StartGameButton();

            actionCardsLabel.Content = curLang.ActionCardLabel();
            materialsLabel.Content = curLang.MaterialLabel();
            actualPlayerLabel.Content = curLang.StartGameLabel();

            fsSelectionListbox.Visibility = Visibility.Hidden;

            largestArmyImage.Visibility = Visibility.Hidden;
            longestWayImage.Visibility = Visibility.Hidden;
         }

    /// <summary>
    /// vykreslí počet cest, vesnic, měst, které ještě hráči zbývají (vlevo dole) a vykreslý počet zbývajících akčních karet (v pravo nahoře)
    /// </summary>
    /// <param name="gm"></param>
        private void DrawRemainingBuildingAndCards(Game gm)
        {
            remainingRoadLine.Visibility = Visibility.Visible;
            remainingTownImage.Visibility = Visibility.Visible;
            remainingVillageImage.Visibility = Visibility.Visible;
            remainingRoadLabel.Visibility = Visibility.Visible;
            remainingVillageLabel.Visibility = Visibility.Visible;
            remainingTownLabel.Visibility = Visibility.Visible;
            remainingActionCardImage.Visibility = Visibility.Visible;
            remainingActionCardLabel.Visibility = Visibility.Visible;

            switch (gm.ActualPlayer.PlProp.Color)
            {
                case Game.color.blue:
                    remainingVillageImage.Source = new BitmapImage(new Uri("pictures/blueVillage.png", UriKind.Relative));
                    remainingTownImage.Source = new BitmapImage(new Uri("pictures/blueTown.png", UriKind.Relative));
                    remainingRoadLine.Stroke = Brushes.Blue;
                    break;
                case Game.color.red:
                    remainingVillageImage.Source = new BitmapImage(new Uri("pictures/redVillage.png", UriKind.Relative));
                    remainingTownImage.Source = new BitmapImage(new Uri("pictures/redTown.png", UriKind.Relative));
                    remainingRoadLine.Stroke = Brushes.Red;
                    break;
                case Game.color.white:
                    remainingVillageImage.Source = new BitmapImage(new Uri("pictures/whiteVillage.png", UriKind.Relative));
                    remainingTownImage.Source = new BitmapImage(new Uri("pictures/whiteTown.png", UriKind.Relative));
                    remainingRoadLine.Stroke = Brushes.White;
                    break;
                case Game.color.yellow:
                    remainingVillageImage.Source = new BitmapImage(new Uri("pictures/yellowVillage.png", UriKind.Relative));
                    remainingTownImage.Source = new BitmapImage(new Uri("pictures/yellowTown.png", UriKind.Relative));
                    remainingRoadLine.Stroke = Brushes.Yellow;
                    break;
            }

            remainingRoadLabel.Content = gm.ActualPlayer.PlProp.RoadRemaining;
            remainingVillageLabel.Content = gm.ActualPlayer.PlProp.VillageRemaining;
            remainingTownLabel.Content = gm.ActualPlayer.PlProp.TownRemaining;
            remainingActionCardLabel.Content = gm.GmProp.RemainingActionCards.GetSumAllActionCard();
        }

        /// <summary>
        /// vykreslí na hrací desce jednotlivé suroviny a zloděje
        /// </summary>
        /// <param name="gameBorderDataStruct_"></param>
        public void DrawGameBorder(GameBorder gameBorder)
        {
            foreach (FaceDesc curFc in gameBorder.Faces)
            {
                DrawMaterials(curFc, gameBorder.probabilities[curFc.ProbabilityNumber-2]);
            }

            DrawThief(gameBorder.Faces.Find(x => x.Thief == true));
        }

        /// <summary>
        /// vykreslí zloděje na pozici zadanou stěnou v parametru
        /// </summary>
        /// <param name="thiefFaceStruct"></param>
        public void DrawThief(FaceDesc thiefFaceStruct)
        {
            Image thiefImg = new Image();
            thiefImg.Source = new BitmapImage(new Uri("pictures/thief.png", UriKind.Relative));
            thiefImg.Height = thief.thiefSize;
            thiefImg.Width = thief.thiefSize;
            canvasGameBorder.Children.Add(thiefImg);
            Canvas.SetTop(thiefImg, thiefFaceStruct.Coordinate.Y - thief.thiefSize / 2);
            Canvas.SetLeft(thiefImg, thiefFaceStruct.Coordinate.X - thief.thiefSize / 2);
            thief_.thiefImage = thiefImg;
            thief_.thiefLocation = thiefFaceStruct;
        }

        public void MoveThief(FaceDesc newFc)
        {
            Canvas.SetTop(thief_.thiefImage, newFc.Coordinate.Y - thief.thiefSize / 2);
            Canvas.SetLeft(thief_.thiefImage, newFc.Coordinate.X - thief.thiefSize / 2);
            thief_.thiefLocation = newFc;
        }

        public void DrawPickPlayer(List<Game.color> players)
        {
            fsSelectionListbox.Items.Clear();
            foreach (var curPl in players)
            {
                fsSelectionListbox.Items.Add(curLang.ColorToString(curPl));
            }
        }

        /// <summary>
        /// Vykreslí surovinu na zadané pole hrací plochy
        /// </summary>
        /// <param name="plottedFace"></param>
        /// <param name="prob">pravděpodobnost padnutí této suroviny na hracích kostkách</param>
        public void DrawMaterials(FaceDesc plottedFace, double prob)
        {
            if (plottedFace.ProbabilityNumber == 7)
            {
                return;
            }
            int faceSize = 80;
            Image materialImg = new Image();
            switch (plottedFace.Material)
            {
                case Game.materials.brick:
                    materialImg.Source = new BitmapImage(new Uri("pictures/brick.png", UriKind.Relative));
                    break;
                case Game.materials.grain:
                    materialImg.Source = new BitmapImage(new Uri("pictures/grain.png", UriKind.Relative));
                    break;
                case Game.materials.sheep:
                    materialImg.Source = new BitmapImage(new Uri("pictures/sheep.png", UriKind.Relative));
                    break;
                case Game.materials.stone:
                    materialImg.Source = new BitmapImage(new Uri("pictures/stone.png", UriKind.Relative));
                    break;
                case Game.materials.wood:
                    materialImg.Source = new BitmapImage(new Uri("pictures/wood.png", UriKind.Relative));
                    break;
            }
            materialImg.Height = faceSize;
            materialImg.Width = faceSize;
            canvasGameBorder.Children.Add(materialImg);
            Canvas.SetTop(materialImg, plottedFace.Coordinate.Y - faceSize/2);
            Canvas.SetLeft(materialImg, plottedFace.Coordinate.X - faceSize/2);

            int numBackgroundSize = 80;
            
            double curNumBackSize = numBackgroundSize * prob+22;

            Ellipse numBackground = new Ellipse();
            numBackground.Height = curNumBackSize;
            numBackground.Width = curNumBackSize;
            numBackground.Fill = Brushes.LightYellow;
            canvasGameBorder.Children.Add(numBackground);
            Canvas.SetTop(numBackground, plottedFace.Coordinate.Y - curNumBackSize / 2);
            Canvas.SetLeft(numBackground, plottedFace.Coordinate.X - curNumBackSize / 2);


            double faceNumSize = 60;
            Label matNum = new Label();
            matNum.Content = plottedFace.ProbabilityNumber;
            matNum.Height = faceNumSize;
            matNum.Width = faceNumSize;
            matNum.FontWeight = FontWeights.Bold;

            //mat_num.Foreground = Brushes.White;
            matNum.HorizontalContentAlignment = HorizontalAlignment.Center;
            matNum.VerticalContentAlignment = VerticalAlignment.Center;
            matNum.VerticalAlignment = VerticalAlignment.Center;
            matNum.HorizontalAlignment = HorizontalAlignment.Center;
            matNum.FontSize = curNumBackSize - 10;
            
            canvasGameBorder.Children.Add(matNum);
            Canvas.SetTop(matNum, plottedFace.Coordinate.Y - faceNumSize / 2);
            Canvas.SetLeft(matNum, plottedFace.Coordinate.X - faceNumSize / 2);
        }

        public void DrawHelpfullID(Game gm)
        {
            foreach (var curVx in gm.GmProp.GameBorderData.Vertices)
            {
                int faceNumSize = 40;
                Label matNum = new Label();
                matNum.Content = curVx.ID;
                matNum.Height = faceNumSize;
                matNum.Width = faceNumSize;
                matNum.FontWeight = FontWeights.Bold;
                matNum.Foreground = Brushes.Navy;

                canvasGameBorder.Children.Add(matNum);
                Canvas.SetTop(matNum, curVx.Coordinate.Y - faceNumSize / 2);
                Canvas.SetLeft(matNum, curVx.Coordinate.X - faceNumSize / 2);
            }

            foreach (var curEg in gm.GmProp.GameBorderData.Edges)
            {
                int faceNumSize = 40;
                Label matNum = new Label();
                matNum.Content = curEg.ID;
                matNum.Height = faceNumSize;
                matNum.Width = faceNumSize;
                matNum.FontWeight = FontWeights.Bold;
                matNum.Foreground = Brushes.Cyan;

                canvasGameBorder.Children.Add(matNum);
                Canvas.SetTop(matNum, curEg.CentreCoordinate.Y - faceNumSize / 2);
                Canvas.SetLeft(matNum, curEg.CentreCoordinate.X - faceNumSize / 2);
            }

            foreach (var curFc in gm.GmProp.GameBorderData.Faces)
            {
                int faceNumSize = 40;
                Label matNum = new Label();
                matNum.Content = curFc.ID;
                matNum.Height = faceNumSize;
                matNum.Width = faceNumSize;
                matNum.FontWeight = FontWeights.Bold;
                matNum.Foreground = Brushes.Red;

                canvasGameBorder.Children.Add(matNum);
                Canvas.SetTop(matNum, curFc.Coordinate.Y - faceNumSize/1.5);
                Canvas.SetLeft(matNum, curFc.Coordinate.X - faceNumSize/1.5);
            }
        }

        /// <summary>
        /// vykreslí vesnici na zadané souřadnici
        /// </summary>
        /// <param name="villageVertexStruct"></param>
        public void DrawVillage(VertexDesc villageVertexStruct)
        {
            DrawVillage(villageVertexStruct, villageVertexStruct.Color);
        }

        public void DrawVillage(VertexDesc villageVertexStruct, Game.color col)
        {
            Image buildingImg = new Image();
            switch (col)
            {
                case Game.color.blue:
                    buildingImg.Source = new BitmapImage(new Uri("pictures/blueVillage.png", UriKind.Relative));
                    break;
                case Game.color.red:
                    buildingImg.Source = new BitmapImage(new Uri("pictures/redVillage.png", UriKind.Relative));
                    break;
                case Game.color.white:
                    buildingImg.Source = new BitmapImage(new Uri("pictures/whiteVillage.png", UriKind.Relative));
                    break;
                case Game.color.yellow:
                    buildingImg.Source = new BitmapImage(new Uri("pictures/yellowVillage.png", UriKind.Relative));
                    break;
            }
            buildingImg.Height = village.villageSize;
            buildingImg.Width = village.villageSize;
            canvasGameBorder.Children.Add(buildingImg);
            Canvas.SetTop(buildingImg, villageVertexStruct.Coordinate.Y - village.villageSize / 2);
            Canvas.SetLeft(buildingImg, villageVertexStruct.Coordinate.X - village.villageSize / 2);
            villageList.Add(new village(buildingImg, villageVertexStruct));
        }

        /// <summary>
        /// smaže vesnici na zadané souřadnici a vykreslí místo ní město
        /// </summary>
        /// <param name="townVertexStruct"></param>
        public void DrawTown(VertexDesc townVertexStruct)
        {
            canvasGameBorder.Children.Remove(villageList.Find(x => x.villageLocation == townVertexStruct).villageImage);
            Image buildingImg = new Image();
            switch (townVertexStruct.Color)
            {
                case Game.color.blue:
                    buildingImg.Source = new BitmapImage(new Uri("pictures/blueTown.png", UriKind.Relative));
                    break;
                case Game.color.red:
                    buildingImg.Source = new BitmapImage(new Uri("pictures/redTown.png", UriKind.Relative));
                    break;
                case Game.color.white:
                    buildingImg.Source = new BitmapImage(new Uri("pictures/whiteTown.png", UriKind.Relative));
                    break;
                case Game.color.yellow:
                    buildingImg.Source = new BitmapImage(new Uri("pictures/yellowTown.png", UriKind.Relative));
                    break;
             }
            
            
             buildingImg.Height = town.townSize;
             buildingImg.Width = town.townSize;
             canvasGameBorder.Children.Add(buildingImg);
             Canvas.SetTop(buildingImg, townVertexStruct.Coordinate.Y - town.townSize / 2);
             Canvas.SetLeft(buildingImg, townVertexStruct.Coordinate.X - town.townSize / 2);
             townList.Add(new town(buildingImg, townVertexStruct));
        }

        public void DrawRoad(EdgeDesc roadEg)
        {
            DrawRoad(roadEg, roadEg.Color);
        }

        /// <summary>
        /// vykreslí cestu na zadané souřadnici
        /// </summary>
        /// <param name="roadEdgeStruct"></param>
        public void DrawRoad(EdgeDesc roadEdgeStruct, Game.color col)
        {
            Line path = new Line();
            //TODO : zajistit, aby se cesty normálně roztáhly přes celou cestu, ale pokud je tam postavená vesnice, tak ne
            //computing coordinate path
            double reserve = 10;
            
            double coordX1 = roadEdgeStruct.Coordinate.Item1.X;
            double coordY1 = roadEdgeStruct.Coordinate.Item1.Y;
            double coordX2 = roadEdgeStruct.Coordinate.Item2.X;
            double coordY2 = roadEdgeStruct.Coordinate.Item2.Y;

            if (coordY1 != coordY2)
            {
                double ratio = Math.Abs(coordX1 - coordX2) / Math.Abs(coordY1 - coordY2);
                double bSquared = (reserve * reserve) / (ratio * ratio + 1);
                double aSquared = reserve * reserve - bSquared;
                double a = Math.Sqrt(bSquared);
                double b = Math.Sqrt(aSquared);

                if (coordX1 > coordX2) { coordX1 -= b; coordX2 += b; }
                else { coordX1 += b; coordX2 -= b; }

                if (coordY1 > coordY2) { coordY1 -= a; coordY2 += a; }
                else { coordY1 += a; coordY2 -= a; }
            }
            else
            {
                if (coordX1 > coordX2) { coordX1 -= reserve; coordX2 += reserve; }
                else { coordX1 += reserve; coordX2 -= reserve; }
            }

            path.X1 = coordX1;
            path.X2 = coordX2;
            path.Y1 = coordY1;
            path.Y2 = coordY2;

            /*
            path.X1 = roadEdgeStruct.GetEdgeCoordinate().Item1.X;
            path.X2 = roadEdgeStruct.GetEdgeCoordinate().Item2.X;
            path.Y1 = roadEdgeStruct.GetEdgeCoordinate().Item1.Y;
            path.Y2 = roadEdgeStruct.GetEdgeCoordinate().Item2.Y;
           */
            switch (col)
            {
                case Game.color.blue :
                    path.Stroke = Brushes.Blue;
                    break;
                case Game.color.red :
                    path.Stroke = Brushes.Red;
                    break;
                case Game.color.white :
                    path.Stroke = Brushes.White;
                    break;
                case Game.color.yellow :
                    path.Stroke = Brushes.Yellow;
                    break;
             }
             path.StrokeThickness = 6;
             
             canvasGameBorder.Children.Add(path);
        }

        /// <summary>
        /// Draw statue game, that means which player is on move, what is his possibility action, listbox to pick some material etc...
        /// </summary>
        /// <param name="gm"></param>
        public void DrawStatue(Game gm, GameWindow.gameEvent gmEv)
        {
            switch (gm.GmProp.CurrentState)
            {
                case Game.state.start: DrawStatueStart(gm);
                    break;
                case Game.state.firstPhaseOfGame: DrawStatueFirstPhaseOfTheGame(gm);
                    break;
                case Game.state.game: DrawStatueGame(gm, gmEv);
                    break;
                case Game.state.endGame: DrawEndGame(gm);
                    break;
            }
        }

        void DrawEndGame(Game gm)
        {
            actualPlayerLabel.Content = curLang.EndOfGame() + curLang.ColorToString(gm.ActualPlayer.PlProp.Color);
            gm.NextState();
            startGameButton.Content = curLang.NextGame();
            startGameButton.IsEnabled = true;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;

            String messageRes = curLang.win() + " " + curLang.ColorToString(gm.ActualPlayer.PlProp.Color) + "\n";
            List<Player> sortedList = gm.Players.OrderByDescending(x => x.PlProp.Points).ToList();
            foreach (var curPl in sortedList)
            {
                messageRes = messageRes + "\n" + curLang.ColorToString(curPl.PlProp.Color) + " - " + curPl.PlProp.Points + "b";
            }

            MessageBoxResult mResult = MessageBox.Show(messageRes, "Konec hry", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        void DrawStatueStart(Game gm)
        {
            actualPlayerLabel.Content = curLang.StartGameLabel();
        }

        void DrawStatueGame(Game gm, GameWindow.gameEvent gmEv)
        {
            DrawActualPlayer(gm.ActualPlayer.PlProp.Color);
            brickCountLabel.Content = gm.ActualPlayer.PlProp.Materials.GetQuantity(Game.materials.brick);
            grainCountLabel.Content = gm.ActualPlayer.PlProp.Materials.GetQuantity(Game.materials.grain);
            sheepCountLabel.Content = gm.ActualPlayer.PlProp.Materials.GetQuantity(Game.materials.sheep);
            stoneCountLabel.Content = gm.ActualPlayer.PlProp.Materials.GetQuantity(Game.materials.stone);
            woodCountLabel.Content = gm.ActualPlayer.PlProp.Materials.GetQuantity(Game.materials.wood);
            DrawActionCardsList(gm.ActualPlayer);
            DrawPointsAndKnights(gm.ActualPlayer);
            roundLabel.Content = gm.GmProp.Round;

            buyActionCardButton.IsEnabled = gm.ActualPlayer.PlProp.Materials.IsPossibleDelete(gm.GmProp.MaterialsForActionCard) && !gm.GmProp.wasBuildSomething;
            changeMaterialButton.IsEnabled = ExchangePossibility(gm).Count > 0;

            DrawRemainingBuildingAndCards(gm);
            
            if (gm.ActualPlayer.PlProp.LargestArmy){largestArmyImage.Visibility = Visibility.Visible;}
            else{largestArmyImage.Visibility = Visibility.Hidden; }

            if (gm.ActualPlayer.PlProp.LongestWay) { longestWayImage.Visibility = Visibility.Visible; }
            else { longestWayImage.Visibility = Visibility.Hidden; }

            switch (gmEv)
            {
                case GameWindow.gameEvent.changingMaterialPartTwo: changingMaterialTwoStatue();
                    break;
                case GameWindow.gameEvent.changingMaterials: changingMaterialStatue();
                    break;
                case GameWindow.gameEvent.knightMove: knightMoveStatue();
                    break;
                case GameWindow.gameEvent.knightMovePartTwo: knightMoveTwoStatue();
                    break;
                case GameWindow.gameEvent.materialsFromPlayers: matFromPlStatue();
                    break;
                case GameWindow.gameEvent.moveThief: moveThiefStatue();
                    break;
                case GameWindow.gameEvent.moveThiefPartTwo: moveThiefTwoStatue();
                    break;
                case GameWindow.gameEvent.none: noneStatue(gm);
                    break;
                case GameWindow.gameEvent.twoFreeRoadsFirstRoad: twoRoadsStatue(); 
                    break;
                case GameWindow.gameEvent.twoFreeRoadsSecondRoad: twoRoadsStatueTwo();
                    break;
                case GameWindow.gameEvent.twoMaterials: twoMaterialStatue();
                    break;
                case GameWindow.gameEvent.twoMaterialsPartTwo: twomaterialStatueTwo();
                    break;
            }

            if (!gm.ActualPlayer.PlProp.RealPlayer)
            {
                gameEventLabel.Content = "";
                buyActionCardButton.IsEnabled = false;
                changeMaterialButton.IsEnabled = false;
                useActionCardButton.IsEnabled = false;
            }
        }

        #region statue function

        private void changingMaterialTwoStatue()
        {
            fsSelectionListbox.Visibility = Visibility.Visible;
            gameEventLabel.Content = curLang.PickMaterialLabel();
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            changeMaterialButton.Content = curLang.CancelChangingMaterial();
            useActionCardButton.IsEnabled = false;
        }

        private void changingMaterialStatue()
        {
            fsSelectionListbox.Visibility = Visibility.Visible;
            gameEventLabel.Content = curLang.PickMaterialLabel();
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = true;
            changeMaterialButton.Content = curLang.CancelChangingMaterial();
            useActionCardButton.IsEnabled = false;
        }

        private void knightMoveStatue()
        {
            fsSelectionListbox.Visibility = Visibility.Hidden;
            gameEventLabel.Content = curLang.MoveThief();
            startGameButton.IsEnabled = false;
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;
        }

        private void knightMoveTwoStatue()
        {

            fsSelectionListbox.Visibility = Visibility.Visible;
            gameEventLabel.Content = curLang.PickPlayer();
            startGameButton.IsEnabled = false;
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;
        }

        private void matFromPlStatue()
        {
            fsSelectionListbox.Visibility = Visibility.Visible;
            gameEventLabel.Content = curLang.MaterialFromPlayers();
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;
        }

        private void moveThiefStatue()
        {
            fsSelectionListbox.Visibility = Visibility.Hidden;
            gameEventLabel.Content = curLang.MoveThief();
            startGameButton.IsEnabled = false;
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;
        }

        private void moveThiefTwoStatue()
        {
            gameEventLabel.Content = curLang.PickPlayer();
            startGameButton.IsEnabled = false;
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;
        }

        private void noneStatue(Game gm)
        {
            bool road = gm.ActualPlayer.PlProp.Materials.IsPossibleDelete(gm.GmProp.MaterialsForRoad) && !gm.GmProp.wasBuildSomething && gm.ActualPlayer.PlProp.RoadRemaining > 0;
            bool village = gm.ActualPlayer.PlProp.Materials.IsPossibleDelete(gm.GmProp.MaterialsForVillage) && !gm.GmProp.wasBuildSomething && gm.ActualPlayer.PlProp.VillageRemaining > 0 && gm.ActualPlayer.IsThereFreeSpaceForVillage();
            bool town = gm.ActualPlayer.PlProp.Materials.IsPossibleDelete(gm.GmProp.MaterialsForTown) && !gm.GmProp.wasBuildSomething && gm.ActualPlayer.PlProp.TownRemaining > 0 && gm.ActualPlayer.PlProp.Village.Count > 0;
            string result;

            fsSelectionListbox.Visibility = Visibility.Hidden;
            buyActionCardButton.IsEnabled = gm.ActualPlayer.PlProp.Materials.IsPossibleDelete(gm.GmProp.MaterialsForActionCard) && !gm.GmProp.wasBuildSomething;
            changeMaterialButton.IsEnabled = ExchangePossibility(gm).Count > 0;
            startGameButton.IsEnabled = true;

            startGameButton.Content = curLang.NextPlayerButton();

            if (!road && !village && !town) { result = ""; }
            else
            {
                result = curLang.PlayerMoveInformation();
                if (road) result += curLang.Road() + "/";
                if (village) result += curLang.Village() + "/";
                if (town) result += curLang.Town() + "/";
                result = result.Substring(0, result.Length - 1);
            }

            gameEventLabel.Content = result;
        }

        private void twoRoadsStatue()
        {
            fsSelectionListbox.Visibility = Visibility.Hidden;
            gameEventLabel.Content = curLang.TwoFreeRoadLabel();
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;
        }

        private void twoRoadsStatueTwo()
        {
            gameEventLabel.Content = curLang.TwoFreeRoadLabel();
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;
        }

        private void twoMaterialStatue()
        {
            fsSelectionListbox.Visibility = Visibility.Visible;
            gameEventLabel.Content = curLang.TwoMaterialsFirstLabel();
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;
        }

        private void twomaterialStatueTwo()
        {
            fsSelectionListbox.Visibility = Visibility.Visible;
            gameEventLabel.Content = curLang.TwoMaterialsSecondLabel();
            startGameButton.IsEnabled = false;
            buyActionCardButton.IsEnabled = false;
            changeMaterialButton.IsEnabled = false;
            useActionCardButton.IsEnabled = false;
        }

        #endregion

        List<Game.materials> ExchangePossibility(Game gm)
        {
            List<Game.materials> result = new List<Game.materials>();
            foreach (var curMat in gm.ActualPlayer.PlProp.Materials.ExchangePossibility(gm.GmProp.NoPortRate))
            {
                if (!result.Contains(curMat)) { result.Add(curMat); }
            }
            foreach (var curMat in gm.ActualPlayer.PlProp.Materials.ExchangePossibility(gm.ActualPlayer.PlProp.PortForMaterial, gm.GmProp.SpecialPortRate))
            {
                if (!result.Contains(curMat)) { result.Add(curMat); }
            }
            if (gm.ActualPlayer.PlProp.UniversalPort)
            {
                foreach (var curMat in gm.ActualPlayer.PlProp.Materials.ExchangePossibility(gm.GmProp.UniversalPortRate))
                {
                    if (!result.Contains(curMat)) { result.Add(curMat); }
                }
            }
            return result;
        }

        void DrawStatueFirstPhaseOfTheGame(Game gm)
        {
            DrawActualPlayer(gm.ActualPlayer.PlProp.Color);
            if (startGameButton.IsEnabled)
            {
                gameEventLabel.Content = "";
            }
            else
            {
                gameEventLabel.Content = curLang.PlayerFirstAndSecondMoveToString(gm.ActualPlayer.PlProp.Color);
            }
        }

        void DrawActualPlayer(Game.color actualPlayerColor)
        {
            actualPlayerLabel.Content = curLang.PlayerMoveToString(actualPlayerColor);
        
            SolidColorBrush curBrush;
            switch (actualPlayerColor)
            {
                case Game.color.blue: curBrush = Brushes.Blue; break;
                case Game.color.red: curBrush = Brushes.Red; break;
                case Game.color.white: curBrush = Brushes.Black; break;
                case Game.color.yellow: curBrush = Brushes.YellowGreen; break;
                default: curBrush = Brushes.Black; break;
            }

            actualPlayerLabel.Foreground = curBrush;
            pointsLabel.Foreground = curBrush;
        }

        void DrawPointsAndKnights(Player actPlayer)
        {
            pointsLabel.Content = curLang.Points() + actPlayer.PlProp.Points + ", " + curLang.Knights() + actPlayer.PlProp.Knights;
        }

        void DrawActionCardsList(Player actPlayer)
        {
            actionCardListbox.Items.Clear();
            foreach (var currentActionCard in actPlayer.PlProp.ActionCards.ActionCards)
            {
                if (currentActionCard.Quantity != 0)
                {
                    actionCardListbox.Items.Add(curLang.NameOfActionCard(currentActionCard.ActionCardType) + " x " + currentActionCard.Quantity);
                }
            }
        }

        public int DrawDice(int sc, int fs)
        {
            diceLabel.Content = curLang.ResultOfRollTheDiceToString()+(fs).ToString() + ", " + (sc).ToString() + "=>" +(fs+sc).ToString();
            return sc + fs;
        }

        public void DrawChangeMaterial(Game gm)
        {
            fsSelectionListbox.Items.Clear();

            List<Game.materials> changeList = ExchangePossibility(gm);

            foreach (var curMat in changeList)
            {
                if (gm.ActualPlayer.PlProp.PortForMaterial.Contains(curMat))
                {
                    fsSelectionListbox.Items.Add(curLang.MaterialToString(curMat) + " " + gm.GmProp.SpecialPortRate + ":1");
                }
                else if (gm.ActualPlayer.PlProp.UniversalPort)
                {
                    fsSelectionListbox.Items.Add(curLang.MaterialToString(curMat) + " " + gm.GmProp.UniversalPortRate + ":1");
                }
                else
                {
                    fsSelectionListbox.Items.Add(curLang.MaterialToString(curMat) + " " + gm.GmProp.NoPortRate + ":1");
                }
            }
            gameEventLabel.Content = curLang.PickMaterialLabel();
            changeMaterialButton.Content = curLang.CancelChangingMaterial();
        }

        public void DrawAllMaterialsOptions(ListBox curLB)
        {
            List<Object> mats = new List<Object>();
            foreach (Game.materials curMat in Enum.GetValues(typeof(Game.materials)))
            {
                if (curMat != Game.materials.noMaterial && curMat != Game.materials.desert)
                {
                    mats.Add(curMat);
                }
            }
            DrawOptions(mats, curLB);
        }

        public void DrawOptions(List<Object> items, ListBox curListBox)
        {
            curListBox.Items.Clear();
            foreach (var curIt in items)
            {
                if (curIt is Game.materials)
                {
                    Game.materials curMat = (Game.materials)curIt;
                    curListBox.Items.Add(curLang.MaterialToString(curMat));
                }
                else if (curIt is Game.color)
                {
                    Game.color curMat = (Game.color)curIt;
                    curListBox.Items.Add(curLang.ColorToString(curMat));
                }
            }
        }

        public void DrawMove(Move mvDesc, Game gm)
        {
            if (mvDesc is FirstPhaseGameMove)
            {
                FirstPhaseGameMove cM = (FirstPhaseGameMove)mvDesc;
                DrawVillage(gm.GmProp.GameBorderData.FindVerticesByCoordinate(cM.VillageCoord.Coordinate));
                DrawRoad(gm.GmProp.GameBorderData.FindEdgeByCoordinate(cM.RoadCoord.CentreCoordinate));
            }
            else if (mvDesc is ThiefMove)
            {
                ThiefMove cM = (ThiefMove)mvDesc;
                MoveThief(cM.ThiefCoord);
            }
            else if (mvDesc is BuildRoadMove)
            {
                BuildRoadMove cM = (BuildRoadMove)mvDesc;
                DrawRoad(gm.GmProp.GameBorderData.FindEdgeByCoordinate(cM.BuildingCoord.CentreCoordinate));
            }
            else if (mvDesc is BuildVillageMove)
            {
                BuildVillageMove cM = (BuildVillageMove)mvDesc;
                DrawVillage(gm.GmProp.GameBorderData.FindVerticesByCoordinate(cM.BuildingCoord.Coordinate));
            }
            else if(mvDesc is BuildTownMove)
            {
                BuildTownMove cM = (BuildTownMove)mvDesc;
                DrawTown(gm.GmProp.GameBorderData.FindVerticesByCoordinate(cM.BuildingCoord.Coordinate)); 
            }
            else if (mvDesc is BuyActionCardMove)
            {
                ;
            }
            else if (mvDesc is CouponMove)
            {
                ;
            }
            else if (mvDesc is KnightMove)
            {
                KnightMove cM = (KnightMove)mvDesc;
                MoveThief(cM.ThiefCoord);
            }
            else if (mvDesc is MaterialFromPlayersMove)
            {
                ;
            }
            else if (mvDesc is TwoMaterialsMove)
            {
                ;
            }
            else if (mvDesc is TwoRoadMove)
            {
                TwoRoadMove cM = (TwoRoadMove)mvDesc;
                DrawRoad(gm.GmProp.GameBorderData.FindEdgeByCoordinate(cM.FirstRoad.CentreCoordinate));
                DrawRoad(gm.GmProp.GameBorderData.FindEdgeByCoordinate(cM.SecondRoad.CentreCoordinate));
            }
        }
    }
}
