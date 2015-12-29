using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;
using osadniciZKatanuAI;

namespace simulator
{
    class Program
    {
        
        static void Main(string[] args)
        {
            ILanguage curLang = new CzechLanguage();
            System.IO.StreamWriter movesWr = new System.IO.StreamWriter("movesInGames.txt");
            System.IO.StreamWriter resultWr = new System.IO.StreamWriter("resultOfGames.txt");
            System.IO.StreamWriter overallResultWr = new System.IO.StreamWriter("overalResult.txt");

            int gamesNum = 1000;
            bool viewProgressBar = true;
            Statistics statistic = new Statistics(curLang, gamesNum, viewProgressBar);

            GameProperties gmProp;

            //Console.WriteLine();
            //Console.Write("zadejte prvniho hrace: ");
            //string fs = Console.ReadLine();
            //Console.Write("zadejte druheho hrace: ");
            //string sc = Console.ReadLine();
            //Console.Write("zadejte tretiho hrace: ");
            //string th = Console.ReadLine();
            //Console.Write("zadejte ctvrteho hrace: ");
            //string fo = Console.ReadLine();


            int i = 0;
            while(i<gamesNum)
            {
                gmProp = new GameProperties(true, curLang);
                //gmProp.LoadFromXml();
                
                List<Player> players = new List<Player>();
                players.Add(new Player(Game.color.red, false, gmProp));
                players.Add(new Player(Game.color.blue, false, gmProp));
                //players.Add(new Player(Game.color.yellow, false, gmProp));
                //players.Add(new Player(Game.color.white, false, gmProp));

                Simulator simul = new Simulator(players, gmProp);
                simul.redPl = new MyGameLogic("bestParam.xml");
                simul.bluePl = new MyGameLogic("superbest.xml");
                //if (fs != "def") { simul.firstPl = new MyGameLogic(fs); } else { simul.firstPl = new MyGameLogic(); }
                //if (sc != "def") { simul.secondPl = new MyGameLogic(sc); } else { simul.secondPl = new MyGameLogic(); }
                //if (th != "def") { simul.thirdPl = new MyGameLogic(th); } else { simul.thirdPl = new MyGameLogic(); }
                //if (fo != "def") { simul.fourthPl = new MyGameLogic(fo); } else { simul.fourthPl = new MyGameLogic(); }
                simul.output = movesWr;

                try
                {
                    var result = simul.run();
                    statistic.AddToStatistic(result);
                    statistic.PrintGameResult(result, resultWr);
                }
                catch (TooManyMovesException e)
                {
                    Console.Write("\n" + e.Message + " in game number " + i);
                    statistic.AddToStatistic();
                }
                catch (TooManyRoundsException e)
                {
                    Console.Write("\n" + e.Message + " in game number " + i);
                    statistic.AddToStatistic();
                }
                catch (WrongNumberOfPlayersException e)
                {
                    Console.Write("\n" + e.Message + " in game number " + i);
                }
                i++;
            }

            statistic.PrintOverallStatistics(Console.Out);

            movesWr.Close();
            resultWr.Close();
            overallResultWr.Close();
        }
    }
}
