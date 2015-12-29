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
            int popSize = 20;
            int numOfGeneration=100;
            int gamesNum=100;
            GenerateMovesProperties gmMovProp = new GenerateMovesProperties();
            int individumSize = gmMovProp.Parameters.Count;
            double xOverProb = 0.8;
            double mutationProb = 0.04;
            double genChangeProb = 0.08;
            //System.IO.StreamWriter bestPerGen = new System.IO.StreamWriter("bestParamPerGen.xml");
            System.IO.StreamWriter evolutionPop = new System.IO.StreamWriter("wholePopulationPerGen.txt");

            EvolutionAlgorithm eva;
            ISelector rouSel = new RouletteWheelSelector();
            IOperator mating = new OnePtXOver(xOverProb);
            IOperator mutation = new IntegerMutation(mutationProb, genChangeProb);
            
            Population parents = new Population(individumSize, 0, 300);
            Population offspring;

            Console.WriteLine();
            Console.Write("zadejte prvniho hrace: ");
            string fs = Console.ReadLine();
            Console.Write("zadejte druheho hrace: ");
            string sc = Console.ReadLine();
            Console.Write("zadejte tretiho hrace: ");
            string th = Console.ReadLine();
            IFitnessEvaluator fitEval = new OneStrategyEvaluator(fs, sc, th, gamesNum);

            parents.GenerateRandomPopulation(popSize);
            eva = new EvolutionAlgorithm(popSize);
            eva.matingSelectors = rouSel;
            eva.operators.Add(mating);
            eva.operators.Add(mutation);
            eva.eval = fitEval;

            int cX = Console.CursorLeft;
            int cY = Console.CursorTop;

            XmlDocument doc = new XmlDocument();
            System.IO.Directory.CreateDirectory("bestParam");

            while (eva.generationNo < numOfGeneration)
            {
                fitEval.Evaluate(parents);
                Printer.PrintBest(parents, doc, eva.generationNo);
                //PrintPopulation(parents, evolutionPop, eva.generationNo);
                offspring = eva.Evolve(parents);
                parents = offspring;
                Console.CursorLeft = cX;
                Console.CursorTop = cY;
                Console.Write(eva.generationNo);
            }



        }

        //public static void PrintPopulation(Population pop, System.IO.StreamWriter output, int generation)
        //{
        //    for (int i = 0; i < pop.sizeOfPopulation; i++)
        //    {
        //        PrintIndividum(pop.population[i], output, generation);
        //        output.WriteLine();
        //    }
        //    output.WriteLine("----------------------------------------------------------------");
        //    output.Flush();
        //}

        
    }
}
