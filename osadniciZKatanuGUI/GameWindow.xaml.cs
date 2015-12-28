using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using osadniciZKatanu;
using osadniciZKatanuAI;

namespace osadniciZKatanuGUI
{

    public partial class GameWindow : Window
    {
        Game gm;
        Draw dr;
        bool showMoves;
        string pickedActionCard = "";
        List<Player> players;
        ILanguage curLang;
        IGameLogic gmLogic;
        Information information;
        gameEvent curEv;
        Game.materials firstMat;
        osadniciZKatanu.Coord firstPoint;

        public enum gameEvent { none, firstPhaseMove, moveThief, moveThiefPartTwo, knightMove, knightMovePartTwo, twoFreeRoadsFirstRoad, twoFreeRoadsSecondRoad, twoMaterials, twoMaterialsPartTwo, materialsFromPlayers, changingMaterials, changingMaterialPartTwo };

        public enum spot { vertex, edge, face, none };

        public GameWindow(int playerCount, string gameLanguage, bool randomGameBorder, bool redIsPlayer, bool blueIsPlayer, bool yellowIsPlayer, bool whiteIsPlayer, bool helpfullID, bool showMoves_)
        {
            InitializeComponent();
            showMoves = showMoves_;
            //gmLogic = new MyGameLogic(BestParameters.newBest);
            gmLogic = new MyGameLogic("param.xml");
            information = new Information();

            switch (gameLanguage)
            {
                case "čeština": curLang = new CzechLanguage();
                    break;
                case "angličtina": curLang = new EngLanguage();
                    break;
            }

            dr = new Draw(gameBorderCanvas, useActionCardButton,
                changeMaterialButton, buyActionCardButton,
                startGameButton, materialsListbox, actionCardsListbox,
                fsSelectionListbox,
                points, actionCardsLabel, materialsLabel, diceLabel, informationLabel, pathLabel, townLabel, villageLabel, actionCardLabel,
                actualPlayerLabel, gameEventLabel, roundLabel, remainingVillageLabel, remainingTownLabel, remainingRoadLabel, remainingActionCardLabel,
                woodCountLabel, brickCountLabel, grainCountLabel, sheepCountLabel, stoneCountLabel,
                largestArmyImage, longestWayImage, remainingVillageImg, remainingTownImg, remainingActionCardImg, remainingRoadLine, curLang);

            players = new List<Player>();
            GameProperties gmProp = new GameProperties(randomGameBorder, curLang);
            gmProp.LoadFromXml();
            curEv = gameEvent.none;

            switch (playerCount)
            {
                case 2:
                    players.Add(new Player(Game.color.red, redIsPlayer, gmProp));
                    players.Add(new Player(Game.color.blue, blueIsPlayer, gmProp));
                    break;
                case 3:
                    players.Add(new Player(Game.color.red, redIsPlayer, gmProp));
                    players.Add(new Player(Game.color.blue, blueIsPlayer, gmProp));
                    players.Add(new Player(Game.color.yellow, yellowIsPlayer, gmProp));
                    break;
                case 4:
                    players.Add(new Player(Game.color.red, redIsPlayer, gmProp));
                    players.Add(new Player(Game.color.blue, blueIsPlayer, gmProp));
                    players.Add(new Player(Game.color.yellow, yellowIsPlayer, gmProp));
                    players.Add(new Player(Game.color.white, whiteIsPlayer, gmProp));
                    break;
            }

            gm = new Game(players, gmProp);
            dr.DrawGameBorder(gmProp.GameBorderData);
            if (helpfullID)
            {
                dr.DrawHelpfullID(gm);
            }
        }

        private spot RecognicePoint(Coord curPt)
        {
            Edge curEg = gm.GmProp.GameBorderData.FindEdgeByCoordinate(curPt);
            if (curEg != gm.GmProp.GameBorderData.noEdge) { return spot.edge; }

            Face curFc = gm.GmProp.GameBorderData.FindFaceByCoordinate(curPt);
            if (curFc != gm.GmProp.GameBorderData.noFace) { return spot.face; }

            Vertex curVx = gm.GmProp.GameBorderData.FindVerticesByCoordinate(curPt);
            if (curVx != gm.GmProp.GameBorderData.noVertex) { return spot.vertex; }

            return spot.none;
        }

        private void GameBorderEvent(object sender, RoutedEventArgs e)
        {
            Coord now = new Coord(Mouse.GetPosition(gameBorderCanvas).X, Mouse.GetPosition(gameBorderCanvas).Y);

            if (gm.ActualPlayer.PlProp.RealPlayer)
            {
                switch (gm.GmProp.CurrentState)
                {
                    case Game.state.firstPhaseOfGame:
                        GameBoardMoveFirstPhaseOfTheGame(now);
                        break;
                    case Game.state.game:
                        GameBoardMove(now);
                        break;
                }
                dr.DrawStatue(gm, curEv);
            }
        }

        public void GameBoardMoveFirstPhaseOfTheGame(Coord curPt)
        {
            spot sp = RecognicePoint(curPt);
            if (sp == spot.none || sp == spot.face) { return; }

            if (curEv == gameEvent.none && sp == spot.vertex)
            {
                Vertex curVx = gm.GmProp.GameBorderData.FindVerticesByCoordinate(curPt);
                if (curVx.IsFreePlaceForVillage() && !gm.GmProp.wasBuildSomething)
                {
                    firstPoint = curPt;
                    dr.DrawVillage(curVx, gm.ActualPlayer.PlProp.Color);
                    curEv = gameEvent.firstPhaseMove;
                }
            }
            else if (curEv == gameEvent.firstPhaseMove && sp == spot.edge)
            {
                Vertex vill = gm.GmProp.GameBorderData.FindVerticesByCoordinate(firstPoint);
                Edge curEg = gm.GmProp.GameBorderData.FindEdgeByCoordinate(curPt);
                if (curEg.VertexNeighbors.Contains(vill) && !gm.GmProp.wasBuildSomething && !curEg.Road)
                {
                    gm.MakeMove(new FirstPhaseGameMove(vill, curEg));
                    dr.DrawRoad(curEg);
                    curEv = gameEvent.none;
                    startGameButton.IsEnabled = true;
                    startGameButton.Content = curLang.NextPlayerButton();
                }
            }
        }

        public void GameBoardMove(Coord curPt)
        {
            spot sp = RecognicePoint(curPt);
            if (sp == spot.none) { return; }

            if (sp == spot.face)
            {
                Face curFc = gm.GmProp.GameBorderData.FindFaceByCoordinate(curPt);
                if (curEv == gameEvent.moveThief || curEv == gameEvent.knightMove)
                {
                    dr.MoveThief(curFc);
                    List<Object> toRobbed = new List<Object>();
                    foreach (var curVx in gm.GmProp.GameBorderData.FindFaceByCoordinate(curPt).VerticesNeighbors)
                    {
                        if (curVx.Building && curVx.Color != gm.ActualPlayer.PlProp.Color && !toRobbed.Contains(curVx.Color)) { toRobbed.Add(curVx.Color); }
                    }
                    if (toRobbed.Count > 1)
                    {
                        fsSelectionListbox.Visibility = System.Windows.Visibility.Visible;
                        dr.DrawOptions(toRobbed, fsSelectionListbox);
                        if (curEv == gameEvent.moveThief) { curEv = gameEvent.moveThiefPartTwo; }
                        else { curEv = gameEvent.knightMovePartTwo; }
                        firstPoint = curPt;
                    }
                    else if (toRobbed.Count == 1)
                    {
                        if (curEv == gameEvent.moveThief)
                        {
                            gm.MakeMove(new ThiefMove(curFc, (Game.color)toRobbed.First()));
                        }
                        else
                        {
                            gm.MakeMove(new KnightMove(curFc, (Game.color)toRobbed.First()));
                        }
                        curEv = gameEvent.none;
                    }
                    else
                    {
                        if (curEv == gameEvent.moveThief)
                        {
                            gm.MakeMove(new ThiefMove(curFc));
                        }
                        else
                        {
                            gm.MakeMove(new KnightMove(curFc));
                        }
                        curEv = gameEvent.none;
                    }
                }
            }
            else if (sp == spot.edge)
            {
                Edge curEg = gm.GmProp.GameBorderData.FindEdgeByCoordinate(curPt);
                if (curEv == gameEvent.none)
                {
                    if (curEg.IsHereAdjacentRoadWithColor(gm.ActualPlayer.PlProp.Color) && !curEg.Road)
                    {
                        gm.MakeMove(new BuildRoadMove(curEg));
                        dr.DrawRoad(curEg);
                    }
                }
                else if (curEv == gameEvent.twoFreeRoadsFirstRoad)
                {
                    if (curEg.IsHereAdjacentRoadWithColor(gm.ActualPlayer.PlProp.Color) && !curEg.Road)
                    {
                        dr.DrawRoad(curEg, gm.ActualPlayer.PlProp.Color);
                        curEv = gameEvent.twoFreeRoadsSecondRoad;
                        firstPoint = curPt;
                    }
                }
                else if (curEv == gameEvent.twoFreeRoadsSecondRoad)
                {
                    Edge fsEg = gm.GmProp.GameBorderData.FindEdgeByCoordinate(firstPoint);
                    if ((curEg.IsHereAdjacentRoadWithColor(gm.ActualPlayer.PlProp.Color) || curEg.EdgeNeighbors.Contains(fsEg)) &&
                        !curEg.Road && curEg != fsEg)
                    {
                        dr.DrawRoad(curEg, gm.ActualPlayer.PlProp.Color);
                        curEv = gameEvent.none;
                        gm.MakeMove(new TwoRoadMove(fsEg, curEg));
                    }

                }
            }
            else if (sp == spot.vertex)
            {
                if (curEv == gameEvent.none)
                {
                    Vertex curVx = gm.GmProp.GameBorderData.FindVerticesByCoordinate(curPt);
                    if (curVx.Village && curVx.Color == gm.ActualPlayer.PlProp.Color)
                    {
                        gm.MakeMove(new BuildTownMove(curVx));
                        dr.DrawTown(curVx);
                    }
                    else if (curVx.IsFreePlaceForVillage() && curVx.IsHereAdjectedRoadWithColor(gm.ActualPlayer.PlProp.Color))
                    {
                        gm.MakeMove(new BuildVillageMove(curVx));
                        dr.DrawVillage(curVx);
                    }
                }
            }
        }

        private void StartGameClick(object sender, RoutedEventArgs e)
        {
            switch (gm.GmProp.CurrentState)
            {
                case Game.state.start:
                    StartGameMove();
                    dr.DrawStatue(gm, curEv);
                    break;
                case Game.state.firstPhaseOfGame:
                    FirstPhaseofTheGameMove();
                    dr.DrawStatue(gm, curEv);
                    break;
                case Game.state.game:
                    Move();
                    dr.DrawStatue(gm, curEv);
                    break;
                case Game.state.endGame:
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                    break;
            }
        }

        private void ActionCardsListboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (actionCardsListbox.SelectedItem != null)
            {
                if (curEv == gameEvent.none && !gm.GmProp.wasUseActionCard)
                {
                    pickedActionCard = actionCardsListbox.SelectedItem.ToString();
                    Game.actionCards curActionCard = RecognizeActionCard(pickedActionCard);
                    useActionCardButton.IsEnabled = true;

                    if (curActionCard == Game.actionCards.twoRoad)
                    {
                        if (gm.ActualPlayer.PlProp.RoadRemaining < 2)
                        {
                            useActionCardButton.IsEnabled = false;
                        }
                    }
                }
            }
            else
            {
                pickedActionCard = "";
                useActionCardButton.IsEnabled = false;
            }
        }

        private void BuyActionCardButtonClick(object sender, RoutedEventArgs e)
        {
            if (gm.ActualPlayer.PlProp.Materials.IsPossibleDelete(gm.GmProp.MaterialsForActionCard))
            {
                gm.MakeMove(new BuyActionCardMove());
                dr.DrawStatue(gm, curEv);
            }
        }

        private void ChangeMaterialButtonClick(object sender, RoutedEventArgs e)
        {
            switch (curEv)
            {
                case gameEvent.changingMaterials:
                    curEv = gameEvent.none;
                    fsSelectionListbox.Visibility = System.Windows.Visibility.Hidden;
                    changeMaterialButton.Content = curLang.ChangeMaterialButton();
                    break;
                case gameEvent.none:
                    curEv = gameEvent.changingMaterials;
                    fsSelectionListbox.Visibility = System.Windows.Visibility.Visible;
                    dr.DrawChangeMaterial(gm);
                    break;
            }
            dr.DrawStatue(gm, curEv);

        }

        private void UseActionCardButtonClick(object sender, RoutedEventArgs e)
        {
            Game.actionCards curActionCard = RecognizeActionCard(pickedActionCard);
            useActionCardButton.IsEnabled = false;
            switch (curActionCard)
            {
                case Game.actionCards.coupon: gm.MakeMove(new CouponMove()); break;
                case Game.actionCards.knight: curEv = gameEvent.knightMove; break;
                case Game.actionCards.materialsFromPlayers:
                    curEv = gameEvent.materialsFromPlayers;
                    fsSelectionListbox.Visibility = System.Windows.Visibility.Visible;
                    dr.DrawAllMaterialsOptions(fsSelectionListbox);
                    break;
                case Game.actionCards.noActionCard: fsSelectionListbox.Visibility = System.Windows.Visibility.Hidden; break;
                case Game.actionCards.twoMaterials:
                    fsSelectionListbox.Visibility = System.Windows.Visibility.Visible;
                    dr.DrawAllMaterialsOptions(fsSelectionListbox);
                    curEv = gameEvent.twoMaterials;
                    break;
                case Game.actionCards.twoRoad: curEv = gameEvent.twoFreeRoadsFirstRoad; break;
            }
            dr.DrawStatue(gm, curEv);
        }

        private void FsSelectionListboxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (curEv == gameEvent.moveThiefPartTwo || curEv == gameEvent.knightMovePartTwo)
            {
                if (fsSelectionListbox.SelectedItem != null)
                {
                    Game.color curCol = RecognizePlayer(fsSelectionListbox.SelectedItem.ToString());
                    Face curFc = gm.GmProp.GameBorderData.FindFaceByCoordinate(firstPoint);
                    if (curCol != Game.color.noColor)
                    {
                        if (curEv == gameEvent.moveThiefPartTwo)
                        {
                            gm.MakeMove(new ThiefMove(curFc, curCol));
                        }
                        else if (curEv == gameEvent.knightMovePartTwo)
                        {
                            gm.MakeMove(new KnightMove(curFc, curCol));
                        }
                        fsSelectionListbox.Visibility = System.Windows.Visibility.Hidden;
                        curEv = gameEvent.none;
                    }
                }
            }
            else
            {
                if (fsSelectionListbox.SelectedItem != null)
                {
                    Game.materials curGameMaterial = RecognizeMaterial(fsSelectionListbox.SelectedItem.ToString());

                    if (curGameMaterial != Game.materials.noMaterial)
                    {
                        switch (curEv)
                        {
                            case gameEvent.changingMaterials:
                                firstMat = curGameMaterial;
                                dr.DrawAllMaterialsOptions(fsSelectionListbox);
                                curEv = gameEvent.changingMaterialPartTwo;
                                break;
                            case gameEvent.changingMaterialPartTwo:
                                gm.MakeMove(new Move(firstMat, curGameMaterial));
                                fsSelectionListbox.Visibility = System.Windows.Visibility.Hidden;
                                changeMaterialButton.Content = curLang.ChangeMaterialButton();
                                curEv = gameEvent.none;
                                break;
                            case gameEvent.materialsFromPlayers:
                                gm.MakeMove(new MaterialFromPlayersMove(curGameMaterial));
                                curEv = gameEvent.none;
                                break;
                            case gameEvent.twoMaterials:
                                curEv = gameEvent.twoMaterialsPartTwo;
                                firstMat = curGameMaterial;
                                break;
                            case gameEvent.twoMaterialsPartTwo:
                                gm.MakeMove(new TwoMaterialsMove(firstMat, curGameMaterial));
                                curEv = gameEvent.none;
                                break;
                        }
                    }
                }
            }
            dr.DrawStatue(gm, curEv);
        }

        private void StartGameMove()
        {
            information.Show();
            startGameButton.Content = curLang.NextPlayerButton();
            startGameButton.IsEnabled = false;
            gm.NextState();
            curEv = gameEvent.none;
            if (!gm.ActualPlayer.PlProp.RealPlayer)
            {
                Move mvDesc = gmLogic.GenerateMove(gm.GmProp, gm.ActualPlayer.PlProp);

                information.ClearInfoText();
                mvDesc = gmLogic.GenerateMove(gm.GmProp, gm.ActualPlayer.PlProp);
                string moveToStr = gm.MakeMove(mvDesc);
                dr.DrawMove(mvDesc, gm);
                foreach (var curMove in gmLogic.GetAllPossibleMoves()) { information.AddPossibleMoves(curMove, gm); }

                if (moveToStr != "" && showMoves)
                {
                    AImoveLabel.Visibility = Visibility.Visible;
                    AImoveLabel.Content = moveToStr;
                }

                startGameButton.IsEnabled = true;
            }
        }

        private void FirstPhaseofTheGameMove()
        {
            if (gm.EndOfFirstPhaseOfTheGame())
            {
                gm.NextState();
                gm.GetMaterials();
                gm.Previous();
                Move();
            }
            else
            {
                AImoveLabel.Content = "";
                gm.NextPlayer();

                startGameButton.IsEnabled = false;

                if (!gm.ActualPlayer.PlProp.RealPlayer)
                {
                    Move mvDesc = gmLogic.GenerateMove(gm.GmProp, gm.ActualPlayer.PlProp);
                    information.ClearInfoText();
                    mvDesc = gmLogic.GenerateMove(gm.GmProp, gm.ActualPlayer.PlProp);
                    string moveToStr = gm.MakeMove(mvDesc);
                    dr.DrawMove(mvDesc, gm);
                    foreach (var curMove in gmLogic.GetAllPossibleMoves()) { information.AddPossibleMoves(curMove, gm); }

                    if (moveToStr != "" && showMoves)
                    {
                        AImoveLabel.Visibility = Visibility.Visible;
                        AImoveLabel.Content = moveToStr;
                    }

                    startGameButton.IsEnabled = true;
                    dr.DrawStatue(gm, curEv);
                }

            }
        }

        private void Move()
        {
            if (!gm.EndGame)
            {
                AImoveLabel.Content = "";
                foreach (Player curPl in gm.Players)
                {
                    curPl.CurrentAddedMaterials.NullAllMaterials();
                    curPl.CurrentDeletedMaterials.NullAllMaterials();
                }

                curEv = gameEvent.none;

                gm.RollTheDice();
                dr.DrawDice(gm.GmProp.FirstDice, gm.GmProp.SecondDice);

                if (gm.GmProp.FallenNum != 7) { gm.GetMaterials(gm.GmProp.FallenNum); }
                else { curEv = gameEvent.moveThief; }
                AImoveLabel.Visibility = Visibility.Hidden;
                gm.NextPlayer();

                if (!gm.ActualPlayer.PlProp.RealPlayer)
                {
                    curEv = gameEvent.none;
                    Move mvDesc = gmLogic.GenerateMove(gm.GmProp, gm.ActualPlayer.PlProp);
                    int counter = 0;
                    string result = "";
                    List<Move> possibleMoves = new List<Move>();

                    information.ClearInfoText();
                    //počet tahů za jedno kolo je omezen na 20
                    while (!(mvDesc is NothingToDoMove) && counter < 20)
                    {
                        mvDesc = gmLogic.GenerateMove(gm.GmProp, gm.ActualPlayer.PlProp);
                        string moveToStr = gm.MakeMove(mvDesc);
                        dr.DrawMove(mvDesc, gm);
                        if (moveToStr != "")
                        {
                            result = result + moveToStr + "\n";
                        }
                        counter++;

                        foreach (var curMove in gmLogic.GetAllPossibleMoves()) { information.AddPossibleMoves(curMove, gm); }
                    }
                    if (result != "" && showMoves)
                    {
                        AImoveLabel.Visibility = Visibility.Visible;
                        AImoveLabel.Content = result;
                    }
                }
                dr.DrawStatue(gm, curEv);
            }

        }

        Game.actionCards RecognizeActionCard(string actionCardString)
        {
            if (actionCardString.StartsWith(curLang.NameOfActionCard(Game.actionCards.coupon))) { return Game.actionCards.coupon; }
            else if (actionCardString.StartsWith(curLang.NameOfActionCard(Game.actionCards.knight))) { return Game.actionCards.knight; }
            else if (actionCardString.StartsWith(curLang.NameOfActionCard(Game.actionCards.materialsFromPlayers))) { return Game.actionCards.materialsFromPlayers; }
            else if (actionCardString.StartsWith(curLang.NameOfActionCard(Game.actionCards.twoMaterials))) { return Game.actionCards.twoMaterials; }
            else if (actionCardString.StartsWith(curLang.NameOfActionCard(Game.actionCards.twoRoad))) { return Game.actionCards.twoRoad; }
            else { return Game.actionCards.noActionCard; }
        }

        Game.materials RecognizeMaterial(string materialString)
        {
            if (materialString.StartsWith(curLang.MaterialToString(Game.materials.brick))) { return Game.materials.brick; }
            else if (materialString.StartsWith(curLang.MaterialToString(Game.materials.grain))) { return Game.materials.grain; }
            else if (materialString.StartsWith(curLang.MaterialToString(Game.materials.sheep))) { return Game.materials.sheep; }
            else if (materialString.StartsWith(curLang.MaterialToString(Game.materials.stone))) { return Game.materials.stone; }
            else if (materialString.StartsWith(curLang.MaterialToString(Game.materials.wood))) { return Game.materials.wood; }
            else { return Game.materials.noMaterial; }
        }

        Game.color RecognizePlayer(string playerString)
        {
            foreach (Player curPl in players)
            {
                if (curLang.ColorToString(curPl.PlProp.Color) == playerString)
                {
                    return curPl.PlProp.Color;
                }
            }
            return Game.color.noColor;
        }
    }
}

