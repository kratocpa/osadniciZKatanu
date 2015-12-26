using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;
using osadniciZKatanuAI;

namespace simulator
{

    public class Simulator
    {

        public const int MAX_MOVES = 20;
        public const int MAX_ROUNDS = 5000;

        public IGameLogic firstPl { set; get; }
        public IGameLogic secondPl { set; get; }
        public IGameLogic thirdPl { set; get; }
        public IGameLogic fourthPl { set; get; }
        Game gm;
        ILanguage curLang;
        public System.IO.StreamWriter output { set; get; }

        IGameLogic actAI;
        //GameDesc gmDes;
        Move mvDes;
        string moveToStr;

        public Simulator(List<Player> players, GameProperties gmProp)
        {
            gm = new Game(players, gmProp);
            this.output = output;
        }

        public Game run()
        {
            if (gm.Players.Count < 2 || gm.Players.Count > 4) { throw new WrongNumberOfPlayersException("Wrong number of players. Two, three or four players is okey."); }
            gm.NextState();
            while (!gm.EndGame)
            {
                if (gm.GmProp.Round > MAX_ROUNDS) { throw new TooManyRoundsException("Too many rounds"); }

                actAI = PickCurrentPlayer();

                if (gm.GmProp.CurrentState == Game.state.firstPhaseOfGame)
                {
                    RunFirstPhaseOfGame();
                }
                else if (gm.GmProp.CurrentState == Game.state.game)
                {
                    runGame();
                }

            }
            return gm;
        }

        private void RunFirstPhaseOfGame()
        {
            mvDes = actAI.GenerateMove(gm.GmProp, gm.ActualPlayer.PlProp);
            moveToStr = gm.MakeMove(mvDes);

            if (moveToStr != null && !gm.EndOfFirstPhaseOfTheGame())
            {
                gm.NextPlayer();
            }
            else if (gm.EndOfFirstPhaseOfTheGame())
            {
                gm.NextState();
                gm.GetMaterials();
            }

            if (output != null)
            {
                WriteHead();
                WriteMove(moveToStr);
                WriteFooter();
            }
        }

        private void runGame()
        {
            gm.RollTheDice();
            if (gm.GmProp.FallenNum != 7) { gm.GetMaterials(gm.GmProp.FallenNum); }
            else { gm.GmProp.NeedToMoveThief = true; }

            WriteHead();
            int counter = 0;
            do
            {
                if (counter > MAX_MOVES) { throw new TooManyMovesException("Too many moves"); }

                mvDes = actAI.GenerateMove(gm.GmProp, gm.ActualPlayer.PlProp);
                moveToStr = gm.MakeMove(mvDes);
                WriteMove(moveToStr);
                counter++;
            } while (!(mvDes is NothingToDoMove));
            WriteFooter();
            gm.NextPlayer();
        }

        IGameLogic PickCurrentPlayer()
        {
            switch (gm.ActualPlayer.PlProp.Color)
            {
                case Game.color.red: return firstPl;
                case Game.color.blue: return secondPl;
                case Game.color.yellow: return thirdPl;
                case Game.color.white: return fourthPl;
                default: return null;
            }
        }

        void WriteHead()
        {
            if (output != null)
            {
                output.WriteLine(gm.GmProp.CurLang.ColorToString(gm.ActualPlayer.PlProp.Color) + ": ");
            }
        }

        void WriteMove(string moveToStr)
        {
            if (moveToStr != "" && output != null)
            {
                output.WriteLine(moveToStr);
            }
        }

        void WriteFooter()
        {
            if (output != null)
            {
                output.WriteLine("----------------------------------");
            }
        }
    }
}
