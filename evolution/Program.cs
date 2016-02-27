using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using osadniciZKatanu;
using osadniciZKatanuAI;

namespace evolution
{
    class Program
    {
        static void Main(string[] args)
        {
            //int popSize = 20;
            //int numOfGeneration=100;
            //int gamesNum=100;

            //double xOverProb = 0.8;
            //double mutationProb = 0.04;
            //double genChangeProb = 0.08;
            //System.IO.StreamWriter bestPerGen = new System.IO.StreamWriter("bestParamPerGen.xml");
            string xmlFile;
            if (args.Count() >= 1)
            {
                xmlFile = args[0];
            }
            else
            {
                Console.WriteLine("You must specify an xml file");
                return;
            }

            GenerateMovesProperties gmMovProp = new GenerateMovesProperties();
            int individumSize = gmMovProp.Parameters.Count;
            System.IO.StreamWriter evolutionPop = new System.IO.StreamWriter("wholePopulationPerGen.txt");

            EvolutionAlgorithmProperties evaProp = new EvolutionAlgorithmProperties();
            try
            {
                evaProp.LoadFromXml(xmlFile);
            }
            catch (Exception ex)
            {
                //TODO: přidat výjimky
                Console.WriteLine(ex.Message);
                return;
            }

            ISelector rouSel = new RouletteWheelSelector();

            IOperator mating;
            switch (evaProp.MatingManner)
            {
                case EvolutionAlgorithm.mating.OnePtXOver: mating = new OnePtXOver(evaProp.MatingProb); break;
                case EvolutionAlgorithm.mating.TwoPtXOver: mating = new TwoPtXOver(evaProp.MatingProb); break;
                case EvolutionAlgorithm.mating.UniformMating: mating = new Uniform(evaProp.MatingProb); break;
                case EvolutionAlgorithm.mating.none: mating = null; break;
                default: mating = null; break;
            }

            IOperator mutation = new IntegerMutation(evaProp.MutationProb, evaProp.MutationChangeBitProb);
            
            Population parents = new Population(individumSize, 0, evaProp.UpperBoundaryEachIndividual);
            Population offspring;

            IFitnessEvaluator fitEval;
            switch (evaProp.EvaluatorManner)
            {
                case EvolutionAlgorithm.fitnessEvaluator.Basic: fitEval = new OneStrategyEvaluator(evaProp.FirstRival, evaProp.SecondRival, evaProp.ThirdRival, evaProp.GamesCount, evaProp.PlayersCountInGame); break;
                case EvolutionAlgorithm.fitnessEvaluator.EbdWithEbd: fitEval = new EbdWithEbdEvaluator(evaProp.PlayersCountInGame); break;
                case EvolutionAlgorithm.fitnessEvaluator.Elo: fitEval = new EloEvaluator(); break;
                case EvolutionAlgorithm.fitnessEvaluator.none: fitEval = null; break;
                default: fitEval = null; break;
            }

            parents.GenerateRandomPopulation(evaProp.PopSize);

            //vytvoření evolučního algoritmu
            EvolutionAlgorithm eva;
            eva = new EvolutionAlgorithm(evaProp.PopSize);
            eva.matingSelectors = rouSel;
            eva.operators.Add(mating);
            eva.operators.Add(mutation);
            eva.eval = fitEval;

            //int cX = Console.CursorLeft;
            //int cY = Console.CursorTop;

            string folderName = xmlFile.Substring(0, xmlFile.Length - 4) + "Results";
            System.IO.Directory.CreateDirectory(folderName);

            while (eva.generationNo < evaProp.GenerationCount)
            {
                fitEval.Evaluate(parents);
                XmlDocument doc = new XmlDocument();
                Printer.PrintBest(parents, doc, eva.generationNo, folderName);
                doc = new XmlDocument();
                Printer.PrintPopulation(parents, doc, eva.generationNo, folderName);
                //PrintPopulation(parents, evolutionPop, eva.generationNo);
                offspring = eva.Evolve(parents);
                parents = offspring;
                //Console.CursorLeft = cX;
                //Console.CursorTop = cY;
                //Console.Write(eva.generationNo);
            }
        }       
    }
}
