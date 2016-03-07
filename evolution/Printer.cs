using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using osadniciZKatanuAI;

namespace evolution
{
    public class Printer
    {
        public static void PrintIndividum(Individual ind, XmlElement par, int generation)
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

        public static void PrintBest(Population pop, XmlDocument doc, int generation, string folderName)
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
            PrintIndividum(best, par, generation);
            string gen = String.Format("{0,3:D3}", generation);
            string path = folderName + "/" + gen + "-" + best.fitness + "-best.xml";

            System.IO.StreamWriter bestPerGen = new System.IO.StreamWriter(path);
            bestPerGen.Write(doc.OuterXml);
            bestPerGen.Flush();
            bestPerGen.Close();
            //output.WriteLine(" ({0}), ({1})", avarageFit, best.fitness);
        }

        public static void PrintPopulation(Population pop, XmlDocument doc, int generation, string folderName)
        {
            XmlElement population = (XmlElement)doc.AppendChild(doc.CreateElement("population"));
            for (int i = 0; i < pop.population.Count; i++)
            {
                XmlElement param = (XmlElement)population.AppendChild(doc.CreateElement("param"));
                XmlElement fitness = (XmlElement)param.AppendChild(doc.CreateElement("fitness"));
                fitness.InnerText = pop.population[i].fitness.ToString();
                PrintIndividum(pop.population[i], param, generation);
            }
            string gen = String.Format("{0,3:D3}", generation);
            string path = folderName + "/" + gen + "-allPopulation.xml";

            StreamWriter bestPerGen = new StreamWriter(path);
            PrettyXml(doc.DocumentElement, bestPerGen, "");
            bestPerGen.Close();
        }

        static void PrettyXml(XmlNode doc, StreamWriter sw, string depth)
        {
            if (!doc.HasChildNodes)
            {
                sw.Write(doc.InnerText);
            }
            else
            {
                bool needEnd;
                sw.Write(depth+"<" + doc.Name);
                if (doc.Attributes != null)
                {
                    for (int i = 0; i < doc.Attributes.Count; i++)
                    {
                        sw.Write(" " + doc.Attributes[i].Name + "=\"" + doc.Attributes[i].Value + "\"");
                    }
                }
                if (doc.HasChildNodes)
                {
                    sw.Write(">");
                    needEnd = true;
                }
                else
                {
                    sw.Write("/>");
                    needEnd = false;
                }

                if (doc.Name != "fitness")
                {
                    sw.WriteLine();
                }

                foreach (XmlNode el in doc.ChildNodes)
                {
                    PrettyXml(el, sw, depth+"\t");
                }

                if (doc.Name == "fitness")
                {
                    sw.WriteLine("</" + doc.Name + ">");
                }
                else
                {
                    if (needEnd)
                    {
                        sw.WriteLine(depth + "</" + doc.Name + ">");
                    }
                }
            }
        }
    }
}
