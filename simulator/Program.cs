using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using osadniciZKatanu;
using osadniciZKatanuAI;

namespace simulator
{
    class Program
    {
        
        static void Main(string[] args)
        {
            int gamesNum = 10;
            string xmlFile;
            if (args.Count() >= 2)
            {
                xmlFile = args[0];
                gamesNum = int.Parse(args[1]);
            }
            else
            {
                Console.WriteLine("You must specify an xml file and number of games");
                return;
            }

            List<GenerateMovesProperties> strategies = new List<GenerateMovesProperties>();

            XmlDocument propDoc = new XmlDocument();
            propDoc.Load(xmlFile);

            foreach (XmlNode curNode in propDoc.DocumentElement.ChildNodes)
            {
                GenerateMovesProperties gen = new GenerateMovesProperties();
                gen.LoadFromXmlNode(curNode);
                strategies.Add(gen);
            }

            int[] strategiesFitness = new int[strategies.Count];
            int[,] strategieFitnessTwoDim = new int[strategies.Count, strategies.Count];

            for (int i = 0; i < strategiesFitness.Count(); i++)
            {
                strategiesFitness[i] = 0;
            }

            ILanguage curLang = new CzechLanguage();
            System.IO.StreamWriter movesWr = new System.IO.StreamWriter("movesInGames.txt");
            System.IO.StreamWriter tournament = new System.IO.StreamWriter("tournament.txt");
            System.IO.StreamWriter resultWr = new System.IO.StreamWriter("resultOfGames.txt");
            System.IO.StreamWriter overallResultWr = new System.IO.StreamWriter("overalResult.txt");

            
            bool viewProgressBar = false;
            

            GameProperties gmProp;


            for (int k = 0; k < strategies.Count; k++)
            {
                for (int l = 0; l < strategies.Count; l++)
                {
                    if (k != l)
                    {
                        Statistics statistic = new Statistics(curLang, gamesNum, viewProgressBar);
                        int i = 0;
                        while (i < gamesNum)
                        {
                            gmProp = new GameProperties(true, curLang);
                            gmProp.LoadFromXml();

                            List<Player> players = new List<Player>();
                            players.Add(new Player(Game.color.red, false, gmProp));
                            players.Add(new Player(Game.color.blue, false, gmProp));

                            Simulator simul = new Simulator(players, gmProp);
                            simul.redPl = new MyGameLogic(strategies[k]);
                            simul.bluePl = new MyGameLogic(strategies[l]);
                            simul.output = movesWr;

                            try
                            {
                                var result = simul.run();
                                statistic.AddToStatistic(result);
                                //statistic.PrintGameResult(result, resultWr);
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

                        //statistic.PrintOverallStatistics(Console.Out);
                        strategiesFitness[k] += statistic.RedWins;
                        strategiesFitness[l] += statistic.BlueWins;
                        strategieFitnessTwoDim[k, l] = statistic.RedWins;
                    }
                }
            }

            for(int i=0; i<strategiesFitness.Count(); i++){
                tournament.WriteLine(strategiesFitness[i]);
            }
            tournament.WriteLine();
            tournament.WriteLine();

            for (int i = 0; i < strategiesFitness.Count(); i++)
            {
                for (int j = 0; j < strategiesFitness.Count(); j++)
                {
                    tournament.Write(strategieFitnessTwoDim[i, j] + "  ");
                }
                tournament.WriteLine();
            }

            movesWr.Close();
            resultWr.Close();
            overallResultWr.Close();
            tournament.Close();
        }
    }
}
