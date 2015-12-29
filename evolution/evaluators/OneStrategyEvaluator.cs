using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simulator;
using osadniciZKatanu;
using osadniciZKatanuAI;

namespace evolution
{
    public class OneStrategyEvaluator : IFitnessEvaluator
    {
        public ILanguage CurLang { get; set; }
        public int GamesNum { get; set; }
        public bool ViewProgressBar { get; set; }
        Statistics statistic;
        string fs, sc, th;

        public OneStrategyEvaluator(string fs, string sc, string th, int gamesNum)
        {
            GamesNum = gamesNum;
            ViewProgressBar = false;
            this.fs = fs; this.sc = sc; this.th = th;
        }

        public void Evaluate(Population pop)
        {
            for (int i = 0; i < pop.sizeOfPopulation; i++)
            {
                pop.population[i].fitness = FitnessFunction(pop.population[i]);
            }
        }

        public double FitnessFunction(Individual curId)
        {
            statistic = new Statistics(CurLang, GamesNum, ViewProgressBar);

            int i = 0;
            while (i < GamesNum)
            {
                GameProperties gmProp = new GameProperties(true, new CzechLanguage());
                gmProp.LoadFromXml();

                Simulator simul = new Simulator(simulateFourPlayers(true, gmProp, i), gmProp);

                simul.redPl = new MyGameLogic(curId.individualArray);
                if (fs != "") { simul.bluePl = new MyGameLogic(fs); } else { simul.bluePl = new MyGameLogic(); }
                if (sc != "") { simul.yellowPl = new MyGameLogic(sc); } else { simul.yellowPl = new MyGameLogic(); }
                if (th != "") { simul.whitePl = new MyGameLogic(th); } else { simul.whitePl = new MyGameLogic(); }

                try
                {
                    var result = simul.run();
                    statistic.AddToStatistic(result);
                }
                catch (TooManyMovesException e)
                {
                    Console.Write("\n" + e.Message + " in game number " + i + "\n");
                    statistic.AddToStatistic();
                }
                catch (TooManyRoundsException e)
                {
                    Console.Write("\n" + e.Message + " in game number " + i + "\n");
                    statistic.AddToStatistic();
                }
                i++;
            }

            return statistic.RedWins;
        }

        private List<Player> simulateFourPlayers(bool rotatePl, GameProperties gmProp, int gameNum)
        {
            List<Player> players = new List<Player>();
            if (rotatePl)
            {
                if (gameNum % 4 == 0)
                {
                    players.Add(new Player(Game.color.red, false, gmProp));
                    players.Add(new Player(Game.color.blue, false, gmProp));
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                    players.Add(new Player(Game.color.white, false, gmProp));
                }
                else if (gameNum % 4 == 1)
                {
                    players.Add(new Player(Game.color.white, false, gmProp));
                    players.Add(new Player(Game.color.red, false, gmProp));
                    players.Add(new Player(Game.color.blue, false, gmProp));
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                }
                else if (gameNum % 4 == 2)
                {
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                    players.Add(new Player(Game.color.white, false, gmProp));
                    players.Add(new Player(Game.color.red, false, gmProp));
                    players.Add(new Player(Game.color.blue, false, gmProp));
                }
                else
                {
                    players.Add(new Player(Game.color.blue, false, gmProp));
                    players.Add(new Player(Game.color.yellow, false, gmProp));
                    players.Add(new Player(Game.color.white, false, gmProp));
                    players.Add(new Player(Game.color.red, false, gmProp));
                }
            }
            else
            {
                players.Add(new Player(Game.color.red, false, gmProp));
                players.Add(new Player(Game.color.blue, false, gmProp));
                players.Add(new Player(Game.color.yellow, false, gmProp));
                players.Add(new Player(Game.color.white, false, gmProp));
            }
            return players;
        }
    }
}
