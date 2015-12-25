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
        GameProperties gmProp;

        public OneStrategyEvaluator(GameProperties gmProp)
        {
            CurLang = new CzechLanguage();
            GamesNum = 24;
            ViewProgressBar = false;
            this.gmProp = gmProp;
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
                List<Player> players = new List<Player>();
                players.Add(new Player(GameDesc.color.red, false, gmProp));
                players.Add(new Player(GameDesc.color.blue, false, gmProp));
                players.Add(new Player(GameDesc.color.yellow, false, gmProp));
                players.Add(new Player(GameDesc.color.white, false, gmProp));

                Simulator simul = new Simulator(players, true, CurLang, gmProp);
                if (i % 4 == 0)
                {
                    simul.firstPl = new MyGameLogic(curId.individualArray);
                    simul.secondPl = new MyGameLogic();
                    simul.thirdPl = new MyGameLogic();
                    simul.fourthPl = new MyGameLogic();
                }
                else if (i % 4 == 1)
                {
                    simul.firstPl = new MyGameLogic();
                    simul.secondPl = new MyGameLogic(curId.individualArray);
                    simul.thirdPl = new MyGameLogic();
                    simul.fourthPl = new MyGameLogic();
                }
                else if (i % 4 == 2)
                {
                    simul.firstPl = new MyGameLogic();
                    simul.secondPl = new MyGameLogic();
                    simul.thirdPl = new MyGameLogic(curId.individualArray);
                    simul.fourthPl = new MyGameLogic();
                }
                else if (i % 4 == 3)
                {
                    simul.firstPl = new MyGameLogic();
                    simul.secondPl = new MyGameLogic();
                    simul.thirdPl = new MyGameLogic();
                    simul.fourthPl = new MyGameLogic(curId.individualArray);
                }

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
    }
}
