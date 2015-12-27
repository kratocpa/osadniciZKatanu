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
    class OneStrategyEvaluator : IFitnessEvaluator
    {
        public ILanguage CurLang { get; set; }
        public int GamesNum { get; set; }
        public bool ViewProgressBar { get; set; }
        Statistics statistic;
        string fs, sc, th;

        public OneStrategyEvaluator(string fs, string sc, string th)
        {
            GamesNum = 100;
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
            int res = 0;

            int i = 0;
            while (i < GamesNum)
            {
                GameProperties gmProp = new GameProperties(true, new CzechLanguage());
                //gmProp.LoadFromXml();

                List<Player> players = new List<Player>();
                players.Add(new Player(Game.color.red, false, gmProp));
                players.Add(new Player(Game.color.blue, false, gmProp));
                players.Add(new Player(Game.color.yellow, false, gmProp));
                players.Add(new Player(Game.color.white, false, gmProp));
                
                Simulator simul = new Simulator(players, gmProp);
                if (i % 4 == 0)
                {
                    simul.firstPl = new MyGameLogic(curId.individualArray);
                    if (fs != "def") { simul.secondPl = new MyGameLogic(fs); } else { simul.secondPl = new MyGameLogic(); }
                    if (sc != "def") { simul.thirdPl = new MyGameLogic(sc); } else { simul.thirdPl = new MyGameLogic(); }
                    if (th != "def") { simul.fourthPl = new MyGameLogic(th); } else { simul.fourthPl = new MyGameLogic(); }
                }
                else if (i % 4 == 1)
                {
                    if (th != "def") { simul.firstPl = new MyGameLogic(th); } else { simul.firstPl = new MyGameLogic(); }                    
                    simul.secondPl = new MyGameLogic(curId.individualArray);
                    if (fs != "def") { simul.thirdPl = new MyGameLogic(fs); } else { simul.thirdPl = new MyGameLogic(); }
                    if (sc != "def") { simul.fourthPl = new MyGameLogic(sc); } else { simul.fourthPl = new MyGameLogic(); }
                    

                }
                else if (i % 4 == 2)
                {
                    if (sc != "def") { simul.firstPl = new MyGameLogic(sc); } else { simul.firstPl = new MyGameLogic(); }
                    if (th != "def") { simul.secondPl = new MyGameLogic(th); } else { simul.secondPl = new MyGameLogic(); }
                    simul.thirdPl = new MyGameLogic(curId.individualArray);
                    if (fs != "def") { simul.fourthPl = new MyGameLogic(fs); } else { simul.fourthPl = new MyGameLogic(); }
                }
                else if (i % 4 == 3)
                {
                    if (fs != "def") { simul.firstPl = new MyGameLogic(fs); } else { simul.firstPl = new MyGameLogic(); }
                    if (sc != "def") { simul.secondPl = new MyGameLogic(sc); } else { simul.secondPl = new MyGameLogic(); }
                    if (th != "def") { simul.thirdPl = new MyGameLogic(th); } else { simul.thirdPl = new MyGameLogic(); }
                    simul.fourthPl = new MyGameLogic(curId.individualArray);
                }

                try
                {
                    var result = simul.run();
                    statistic.AddToStatistic(result);
                    if (i % 4 == 0 && result.ActualPlayer.PlProp.Color == Game.color.red) { res++; }
                    else if (i % 4 == 1 && result.ActualPlayer.PlProp.Color == Game.color.blue) { res++; }
                    else if (i % 4 == 2 && result.ActualPlayer.PlProp.Color == Game.color.yellow) { res++; }
                    else if (i % 4 == 3 && result.ActualPlayer.PlProp.Color == Game.color.white) { res++; }
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

            return res;
        }
    }
}
