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
        public int Generation { get; set; }
        public ILanguage CurLang { get; set; }
        public int GamesNum { get; set; } // počet her (které se mají provést) k ohodnocení jedince
        public int NumOfPlayers { get; set; }
        public bool ViewProgressBar { get; set; }
        Statistics statistic; // statistika k jednoduššímu zjištění výsledků
        string fs, sc, th; // názvy .xml souborů pro jiné než implicitní jedince

        public OneStrategyEvaluator(string fs, string sc, string th, int gamesNum, int numOfPlayers)
        {
            GamesNum = gamesNum;
            NumOfPlayers = numOfPlayers;
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
                Simulator simul;
                if (NumOfPlayers == 4)
                {
                    simul = new Simulator(Common.SimulateFourPlayers(true, gmProp, i), gmProp);
                    simul.redPl = new MyGameLogic(curId.individualArray);
                    if (fs != "") { simul.bluePl = new MyGameLogic(fs); } else { simul.bluePl = new MyGameLogic(); }
                    if (sc != "") { simul.yellowPl = new MyGameLogic(sc); } else { simul.yellowPl = new MyGameLogic(); }
                    if (th != "") { simul.whitePl = new MyGameLogic(th); } else { simul.whitePl = new MyGameLogic(); }
                }
                else if (NumOfPlayers == 3)
                {
                    simul = new Simulator(Common.SimulateThreePlayers(true, gmProp, i), gmProp);
                    simul.redPl = new MyGameLogic(curId.individualArray);
                    if (fs != "") { simul.bluePl = new MyGameLogic(fs); } else { simul.bluePl = new MyGameLogic(); }
                    if (sc != "") { simul.yellowPl = new MyGameLogic(sc); } else { simul.yellowPl = new MyGameLogic(); }
                }
                else
                {
                    simul = new Simulator(Common.SimulateTwoPlayers(true, gmProp, i), gmProp);
                    simul.redPl = new MyGameLogic(curId.individualArray);
                    if (fs != "") { simul.bluePl = new MyGameLogic(fs); } else { simul.bluePl = new MyGameLogic(); }
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
