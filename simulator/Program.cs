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

            GameProperties gmProp = new GameProperties();
            gmProp.LoadFromXml();

            int i = 0;
            while(i<gamesNum)
            {
                List<Player> players = new List<Player>();
                players.Add(new Player(GameDesc.color.red, false, gmProp));
                players.Add(new Player(GameDesc.color.blue, false, gmProp));
                players.Add(new Player(GameDesc.color.yellow, false, gmProp));
                players.Add(new Player(GameDesc.color.white, false, gmProp));

                Simulator simul = new Simulator(players, false, curLang, gmProp);
                simul.firstPl = new MyGameLogic();
                simul.secondPl = new MyGameLogic();
                simul.thirdPl = new MyGameLogic();
                simul.fourthPl = new MyGameLogic();
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
