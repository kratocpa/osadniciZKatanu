using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using osadniciZKatanuAI;

namespace evolution
{
    public class Printer
    {
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

        public static void PrintBest(Population pop, XmlDocument doc, int generation)
        {


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
            XmlElement par = (XmlElement)doc.AppendChild(doc.CreateElement("bestParameter"));
            XmlElement fitness = (XmlElement)par.AppendChild(doc.CreateElement("fitness"));
            fitness.InnerText = best.fitness.ToString();
            XmlElement avarageFitness = (XmlElement)par.AppendChild(doc.CreateElement("avarageFitness"));
            avarageFitness.InnerText = avarageFit.ToString();
            PrintIndividum(best, par, generation, avarageFit);

            string path = "bestParam/" + generation + "-" + best.fitness + ".xml";
            System.IO.StreamWriter bestPerGen = new System.IO.StreamWriter(path);
            bestPerGen.Write(doc.OuterXml);
            bestPerGen.Flush();
            bestPerGen.Close();
            //output.WriteLine(" ({0}), ({1})", avarageFit, best.fitness);
        }
    }
}
