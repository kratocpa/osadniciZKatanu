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
        GameDesc gmDes;
        Move mvDes;
        string moveToStr;

        public Simulator(List<Player> players, bool randomGameBorder, ILanguage curLang, GameProperties gmProp)
        {
            gm = new Game(players, randomGameBorder, curLang, gmProp);
            this.curLang = curLang;
            this.output = output;
        }

        public GameDesc run()
        {
            if (gm.Players.Count < 2 || gm.Players.Count > 4) { throw new WrongNumberOfPlayersException("Wrong number of players. Two, three or four players is okey."); }
            gm.NextState();
            while (!gm.EndGame)
            {
                if (gm.Round > MAX_ROUNDS) { throw new TooManyRoundsException("Too many rounds"); }

                actAI = PickCurrentPlayer();

                if (gm.CurrentState == GameDesc.state.firstPhaseOfGame)
                {
                    RunFirstPhaseOfGame();
                }
                else if (gm.CurrentState == GameDesc.state.game)
                {
                    runGame();
                }

            }
            gm.Sychronize();
            return gm;
        }

        private void RunFirstPhaseOfGame()
        {
            gm.Sychronize();
            gmDes = gm;
            mvDes = actAI.GenerateMove(gmDes);
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
            if (gm.FallenNum != 7) { gm.GetMaterials(gm.FallenNum); }
            else { gm.NeedToMoveThief = true; }

            WriteHead();
            int counter = 0;
            do
            {
                if (counter > MAX_MOVES) { throw new TooManyMovesException("Too many moves"); }
                gm.Sychronize();
                gmDes = gm;
                mvDes = actAI.GenerateMove(gmDes);
                moveToStr = gm.MakeMove(mvDes);
                WriteMove(moveToStr);
                counter++;
            } while (!(mvDes is NothingToDoMove));
            WriteFooter();
            gm.NextPlayer();
        }

        IGameLogic PickCurrentPlayer()
        {
            switch (gm.ActualPlayer.Color)
            {
                case GameDesc.color.red: return firstPl;
                case GameDesc.color.blue: return secondPl;
                case GameDesc.color.yellow: return thirdPl;
                case GameDesc.color.white: return fourthPl;
                default: return null;
            }
        }

        void WriteHead()
        {
            if (output != null)
            {
                output.WriteLine(curLang.ColorToString(gm.ActualPlayer.Color) + ": ");
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
