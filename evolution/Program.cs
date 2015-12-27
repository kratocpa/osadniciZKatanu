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
            GenerateMovesProperties gmMovProp = new GenerateMovesProperties();
            int individumSize = gmMovProp.Parameters.Count;
            double xOverProb = 0.8;
            double mutationProb = 0.04;
            double genChangeProb = 0.08;
            System.IO.StreamWriter bestPerGen = new System.IO.StreamWriter("bestParamPerGen.txt");
            System.IO.StreamWriter evolutionPop = new System.IO.StreamWriter("wholePopulationPerGen.txt");

            EvolutionAlgorithm eva;
            ISelector rouSel = new RouletteWheelSelector();
            IOperator mating = new OnePtXOver(xOverProb);
            IOperator mutation = new IntegerMutation(mutationProb, genChangeProb);
            IFitnessEvaluator fitEval = new OneStrategyEvaluator();
            Population parents = new Population(individumSize, 0, 300);
            Population offspring;

            parents.GenerateRandomPopulation(popSize);
            eva = new EvolutionAlgorithm(popSize);
            eva.matingSelectors = rouSel;
            eva.operators.Add(mating);
            eva.operators.Add(mutation);
            eva.eval = fitEval;

            int cX = Console.CursorLeft;
            int cY = Console.CursorTop;

            while (eva.generationNo < numOfGeneration)
            {
                fitEval.Evaluate(parents);
                PrintBest(parents, bestPerGen, eva.generationNo);
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

        public static void PrintIndividum(Individual ind, XmlElement par, int generation, double avarage)
        {
            //output.Write(String.Format("{0:000}", generation) + ". ");
            GenerateMovesProperties genMoPr = new GenerateMovesProperties();
            genMoPr.LoadFromArray(ind.individualArray);
            for (int i = 0; i < genMoPr.Parameters.Count(); i++)
            {
                par.SetAttribute(genMoPr.Parameters[i].Name, genMoPr.Parameters[i].Scale.ToString());
                //output.Write(String.Format("{0:000}", ind.individualArray[i]) + " ");
            }
            //output.Write("(" + ind.fitness + ")");
        }

        public static void PrintBest(Population pop, System.IO.StreamWriter output, int generation)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("bestParameters"));
            
            double bestFit = pop.population[0].fitness;
            double avarageFit = 0;
            Individual best = pop.population[0];

            for (int i = 0; i < pop.sizeOfPopulation; i++)
            {
                if (pop.population[i].fitness > bestFit)
                {
                    best = pop.population[i];
                    bestFit = pop.population[i].fitness;
                }
                avarageFit += pop.population[i].fitness;
            }
            avarageFit = avarageFit / pop.sizeOfPopulation;
            XmlElement par = (XmlElement)el.AppendChild(doc.CreateElement("bestParameter"));
            
            PrintIndividum(best, par, generation, avarageFit);
            output.WriteLine(" ({0}), ({1})", avarageFit, best.fitness);

            output.WriteLine(doc.OuterXml);
            output.Flush();
        }
    }
}
