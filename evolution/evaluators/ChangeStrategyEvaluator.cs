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
    class ChangeStrategyEvaluator : IFitnessEvaluator
    {
       
        public ILanguage CurLang { get; set; }
        public int GamesNum { get; set; } // počet her (které se mají provést) k ohodnocení jedince
        public int NumOfPlayers { get; set; }
        public int ChangeRivals { get; set; }
        public int Generation { get; set; }
        public bool ChangePopulation { get; set; }
        Statistics statistic; // statistika k jednoduššímu zjištění výsledků

        private Individual firstRival { get; set; }
        private Individual secondRival { get; set; }
        private Individual thirdRival { get; set; }

        public ChangeStrategyEvaluator(int gamesNum, int numOfPlayers, int changeRivals, bool changePopulation)
        {
            GamesNum = gamesNum;
            NumOfPlayers = numOfPlayers;
            ChangeRivals = changeRivals;
            ChangePopulation = changePopulation;
            Generation = 0;
        }

        public void Evaluate(Population pop)
        {
            if (Generation == 0)
            {
                firstRival = pop.population[0];
                secondRival = pop.population[1];
                thirdRival = pop.population[2];
            }

            for (int i = 0; i < pop.sizeOfPopulation; i++)
            {
                pop.population[i].fitness = FitnessFunction(pop.population[i]);
            }

            if (Generation % ChangeRivals == 0)
            {
                Population newPop = (Population)pop.Clone();
                newPop.population.OrderBy(x => x.fitness);
                firstRival = pop.population[0];
                secondRival = pop.population[1];
                thirdRival = pop.population[2];
                if (ChangePopulation)
                {
                    pop = new Population(pop.lengthOfEachIndividual, pop.lowerEachIndividual, pop.upperEachIndividual);
                }
            }
            Generation++;
        }

        public double FitnessFunction(Individual curId)
        {
            statistic = new Statistics(CurLang, GamesNum, false);

            int i = 0;
            while (i < GamesNum)
            {
                GameProperties gmProp = new GameProperties(true, new CzechLanguage());
                gmProp.LoadFromXml();
                Simulator simul;
                if (NumOfPlayers == 4)
                {
                    simul = new Simulator(Common.SimulateFourPlayers(true, gmProp, i), gmProp);
                    simul.redPl = new MyGameLogic(curId.individualArray);
                    simul.bluePl = new MyGameLogic(firstRival.individualArray);
                    simul.yellowPl = new MyGameLogic(secondRival.individualArray);
                    simul.whitePl = new MyGameLogic(thirdRival.individualArray);
                }
                else if (NumOfPlayers == 3)
                {
                    simul = new Simulator(Common.SimulateThreePlayers(true, gmProp, i), gmProp);
                    simul.redPl = new MyGameLogic(curId.individualArray);
                    simul.bluePl = new MyGameLogic(firstRival.individualArray);
                    simul.yellowPl = new MyGameLogic(secondRival.individualArray);
                }
                else
                {
                    simul = new Simulator(Common.SimulateTwoPlayers(true, gmProp, i), gmProp);
                    simul.redPl = new MyGameLogic(curId.individualArray);
                    simul.bluePl = new MyGameLogic(firstRival.individualArray);
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
