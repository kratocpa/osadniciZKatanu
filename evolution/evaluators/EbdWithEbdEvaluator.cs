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
    class EbdWithEbdEvaluator : IFitnessEvaluator
    {
        public ILanguage CurLang { get; set; }
        public int NumOfPlayers;
        public int Generation { get; set; }
        Statistics statistic; // statistika k jednoduššímu zjištění výsledků

        public EbdWithEbdEvaluator(int numOfPlayers)
        {
            NumOfPlayers = numOfPlayers;
        }

        public void Evaluate(Population pop)
        {
            for (int i = 0; i < pop.sizeOfPopulation; i++)
            {
                pop.population[i].fitness = 0;
            }

            if (NumOfPlayers == 2)
            {
                EvaluateTwoPlayers(pop);
            }
            else if (NumOfPlayers == 3)
            {
                EvaluateThreePlayers(pop);
            }
            else if (NumOfPlayers == 4)
            {
                EvaluateFourPlayers(pop);
            }


        }

        public void EvaluateTwoPlayers(Population pop)
        {
            Simulator simul;
            for (int i = 0; i < pop.sizeOfPopulation; i++)
            {
                for (int j = 0; j < pop.sizeOfPopulation; j++)
                {
                    if (i != j)
                    {
                        statistic = new Statistics(CurLang, 1, false);
                        GameProperties gmProp = new GameProperties(true, new CzechLanguage());
                        gmProp.LoadFromXml();
                        List<Player> players = new List<Player>();
                        players.Add(new Player(Game.color.red, false, gmProp));
                        players.Add(new Player(Game.color.blue, false, gmProp));
                        simul = new Simulator(players, gmProp);
                        simul.redPl = new MyGameLogic(pop.population[i].individualArray);
                        simul.bluePl = new MyGameLogic(pop.population[j].individualArray);

                        try
                        {
                            var result = simul.run();
                            statistic.AddToStatistic(result);
                            pop.population[i].fitness += statistic.RedWins;
                            pop.population[j].fitness += statistic.BlueWins;
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
                    }
                }
            }
        }

        public void EvaluateThreePlayers(Population pop)
        {
            Simulator simul;
            for (int i = 0; i < pop.sizeOfPopulation; i++)
            {
                for (int j = 0; j < pop.sizeOfPopulation; j++)
                {
                    for (int k = 0; k < pop.sizeOfPopulation; k++)
                    {
                        if (i != j && j != k && k != i)
                        {
                            statistic = new Statistics(CurLang, 1, false);
                            GameProperties gmProp = new GameProperties(true, new CzechLanguage());
                            gmProp.LoadFromXml();
                            List<Player> players = new List<Player>();
                            players.Add(new Player(Game.color.red, false, gmProp));
                            players.Add(new Player(Game.color.blue, false, gmProp));
                            players.Add(new Player(Game.color.yellow, false, gmProp));
                            simul = new Simulator(players, gmProp);
                            simul.redPl = new MyGameLogic(pop.population[i].individualArray);
                            simul.bluePl = new MyGameLogic(pop.population[j].individualArray);
                            simul.yellowPl = new MyGameLogic(pop.population[k].individualArray);
                            try
                            {
                                var result = simul.run();
                                statistic.AddToStatistic(result);
                                pop.population[i].fitness += statistic.RedWins;
                                pop.population[j].fitness += statistic.BlueWins;
                                pop.population[k].fitness += statistic.YellowWins;
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
                        }
                    }
                }
            }
        }

        public void EvaluateFourPlayers(Population pop)
        {
            Simulator simul;
            for (int i = 0; i < pop.sizeOfPopulation; i++)
            {
                for (int j = 0; j < pop.sizeOfPopulation; j++)
                {
                    for (int k = 0; k < pop.sizeOfPopulation; k++)
                    {
                        for (int l = 0; l < pop.sizeOfPopulation; l++)
                        {
                            if (i != j && j != k && k != l && l != i && i != k && j != l)
                            {
                                statistic = new Statistics(CurLang, 1, false);
                                GameProperties gmProp = new GameProperties(true, new CzechLanguage());
                                gmProp.LoadFromXml();
                                List<Player> players = new List<Player>();
                                players.Add(new Player(Game.color.red, false, gmProp));
                                players.Add(new Player(Game.color.blue, false, gmProp));
                                players.Add(new Player(Game.color.yellow, false, gmProp));
                                players.Add(new Player(Game.color.white, false, gmProp));
                                simul = new Simulator(players, gmProp);
                                simul.redPl = new MyGameLogic(pop.population[i].individualArray);
                                simul.bluePl = new MyGameLogic(pop.population[j].individualArray);
                                simul.yellowPl = new MyGameLogic(pop.population[k].individualArray);
                                simul.whitePl = new MyGameLogic(pop.population[l].individualArray);
                                try
                                {
                                    var result = simul.run();
                                    statistic.AddToStatistic(result);
                                    pop.population[i].fitness += statistic.RedWins;
                                    pop.population[j].fitness += statistic.BlueWins;
                                    pop.population[k].fitness += statistic.YellowWins;
                                    pop.population[l].fitness += statistic.WhiteWins;
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
                            }
                        }
                    }
                }
            }
        }
    }
}
