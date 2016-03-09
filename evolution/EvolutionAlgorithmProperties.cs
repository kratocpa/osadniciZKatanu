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
    public class EvolutionAlgorithmProperties : ICloneable
    {
        public int PopSize { get; set; } // velikost populace
        public int GenerationCount { get; set; } // počet generací
        public int UpperBoundaryEachIndividual { get; set; } // maximální hodnota každého parametru jedince
        public EvolutionAlgorithm.mating MatingManner { get; set; } // způsob křížení (jednobodové, dvoubodové, uniformní)
        public double MatingProb { get; set; } // pravděpodobnost křížení
        public double MutationProb { get; set; } // pravděpodobnost mutace
        public double MutationChangeBitProb { get; set; } // pravděpodobnost mutace jednoho bitu
        public Population InitialPopulation { get; set; }
        public int EliteCount { get; set; }

        //nastavení fitness funkce
        public EvolutionAlgorithm.fitnessEvaluator EvaluatorManner { get; set; } // fitness funkce (pevně danný protihráči, všichni proti všem, elo)
        public int PlayersCountInGame { get; set; } // počet hráčů v jedné testované hře při ohodnocování jedinců
        
        //nastavení pro fitness funkci s pevně danými protihráči
        public int GamesCount; // počet her pro ohodnocení jednoho jedince
        public string FirstRival; // první soupeř  
        public string SecondRival; // druhý soupeř 
        public string ThirdRival; // třetí soupeř
        
        //nastavení pro fitness funci se střídáním protihráčů
        public double InitialElo { get; set; } // elo, které má hráč který zatím nehrál žádnou hru
        public int ChangeRivals { get; set; }
        public bool ChangePopulation { get; set; }

        private List<string> initialPopString { get; set; }

        public EvolutionAlgorithmProperties()
        {
            PopSize = 0;
            GenerationCount = 0;
            MatingManner = EvolutionAlgorithm.mating.none;
            MutationProb = 0;
            MutationChangeBitProb = 0;
            EvaluatorManner = EvolutionAlgorithm.fitnessEvaluator.none;
            PlayersCountInGame = 0;
            FirstRival = "";
            SecondRival = "";
            ThirdRival = "";
            InitialElo = 0;
            EliteCount = 0;
        }

        public void LoadFromXml(string xmlFile)
        {
            XmlDocument EvaPropDoc = new XmlDocument();

            EvaPropDoc.Load(xmlFile);
            PopSize = int.Parse(EvaPropDoc.DocumentElement.Attributes["popSize"].Value);
            GenerationCount = int.Parse(EvaPropDoc.DocumentElement.Attributes["generationCount"].Value);
            UpperBoundaryEachIndividual = int.Parse(EvaPropDoc.DocumentElement.Attributes["upperBoundaryEachIndividual"].Value);
            foreach (XmlNode curNode in EvaPropDoc.DocumentElement.ChildNodes)
            {
                switch (curNode.Name)
                {
                    case "matingManner": SetMatingManner(curNode); break;
                    case "mutation": SetMutation(curNode); break;
                    case "elite": SetElite(curNode); break;
                    case "evaluatorManner": SetEvaluatorManner(curNode); break;
                    default: break;
                }
            }
            if (initialPopString != null)
            {
                GenerateMovesProperties gmMovProp = new GenerateMovesProperties();
                int individumSize = gmMovProp.Parameters.Count;
                InitialPopulation = new Population(individumSize, 0, UpperBoundaryEachIndividual);
                int countPerPop = PopSize / initialPopString.Count;
                int counter = 0;
                foreach (var curStr in initialPopString)
                {
                    List<Individual> curPop = LoadPopulation(curStr);
                    var newPop = curPop.OrderByDescending(x => x.fitness);
                    for (int i = 0; i < countPerPop; i++)
                    {
                        InitialPopulation.AddIndividual(newPop.ElementAt(i));
                        counter++;
                    }
                }
                while (counter < PopSize)
                {
                    InitialPopulation.AddIndividual(new Individual(individumSize, 0, UpperBoundaryEachIndividual, counter));
                    counter++;
                }
            }
        }

        private void SetElite(XmlNode curNode)
        {
            EliteCount = int.Parse(curNode.Attributes["eliteCount"].Value);
        }

        private void SetMatingManner(XmlNode curNode)
        {
            string type = curNode.Attributes["type"].Value;
            MatingProb = double.Parse(curNode.Attributes["matingProb"].Value, System.Globalization.CultureInfo.InvariantCulture);
            switch (type)
            {
                case "OnePtXOver": MatingManner = EvolutionAlgorithm.mating.OnePtXOver; break;
                case "TwoPtXOver": MatingManner = EvolutionAlgorithm.mating.TwoPtXOver; break;
                case "UniformMating": MatingManner = EvolutionAlgorithm.mating.UniformMating; break;
                default: MatingManner = EvolutionAlgorithm.mating.none; break;
            }
        }

        private void SetMutation(XmlNode curNode)
        {
            MutationProb = double.Parse(curNode.Attributes["mutationProb"].Value, System.Globalization.CultureInfo.InvariantCulture);
            MutationChangeBitProb = double.Parse(curNode.Attributes["mutationChangeBitProb"].Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        private void SetEvaluatorManner(XmlNode curNode)
        {
            string type = curNode.Attributes["type"].Value;
            switch (type)
            {
                case "basic": EvaluatorManner = EvolutionAlgorithm.fitnessEvaluator.Basic; break;
                case "ebdWithEbd": EvaluatorManner = EvolutionAlgorithm.fitnessEvaluator.EbdWithEbd; break;
                case "changeRivals": EvaluatorManner = EvolutionAlgorithm.fitnessEvaluator.ChangeRivals; break;
                default: EvaluatorManner = EvolutionAlgorithm.fitnessEvaluator.none; break;
            }

            PlayersCountInGame = int.Parse(curNode.Attributes["playersCountInGame"].Value);

            foreach (XmlNode cN in curNode.ChildNodes)
            {
                switch (cN.Name)
                {
                    case "firstRival": FirstRival = cN.InnerText; break;
                    case "secondRival": SecondRival = cN.InnerText; break;
                    case "thirdRival": ThirdRival = cN.InnerText; break;
                    case "gameCount": GamesCount = int.Parse(cN.InnerText); break;
                    case "changingTime": ChangeRivals = int.Parse(cN.InnerText); break;
                    case "changePopulation": if (cN.InnerText == "true") { ChangePopulation = true; } else { ChangePopulation = false; } break;
                    case "initialPopulation": SetInitialPopulation(cN); break;
                    default: break;
                }
            }
        }

        private void SetInitialPopulation(XmlNode curNode)
        {            
            if (initialPopString == null)
            {
                initialPopString = new List<string>();
            }
            initialPopString.Add(curNode.InnerText);
        }

        private List<Individual> LoadPopulation(string xmlFile)
        {
            XmlDocument popDoc = new XmlDocument();
            List<Individual> pop = new List<Individual>();
            popDoc.Load(xmlFile);
            foreach (XmlNode curNode in popDoc.DocumentElement.ChildNodes)
            {
                switch (curNode.Name)
                {
                    case "param": pop.Add(LoadIndividual(curNode)); break;
                    default: break;
                }
            }
            return pop;
        }

        private Individual LoadIndividual(XmlNode curNode)
        {
            int fitness=0;
            GenerateMovesProperties movProp = new GenerateMovesProperties();
            movProp.LoadFromXmlNode(curNode);
            foreach (XmlNode cN in curNode)
            {
                switch (cN.Name)
                {
                    case "fitness": fitness = int.Parse(curNode.InnerText); break;
                    default: break;
                }
            }
            return new Individual(movProp, fitness, 0, UpperBoundaryEachIndividual);
        }

        public object Clone()
        {
            EvolutionAlgorithmProperties clonEAP = new EvolutionAlgorithmProperties();
            clonEAP.PopSize = PopSize;
            clonEAP.GenerationCount = GenerationCount;
            clonEAP.UpperBoundaryEachIndividual = UpperBoundaryEachIndividual;
            clonEAP.MatingManner = MatingManner;
            clonEAP.MatingProb = MatingProb;
            clonEAP.MutationProb = MutationProb;
            clonEAP.MutationChangeBitProb = MutationChangeBitProb;
            clonEAP.EvaluatorManner = EvaluatorManner;
            clonEAP.PlayersCountInGame = PlayersCountInGame;
            clonEAP.GamesCount = GamesCount;
            clonEAP.FirstRival = FirstRival;
            clonEAP.SecondRival = SecondRival;
            clonEAP.ThirdRival = ThirdRival;
            clonEAP.InitialElo = InitialElo;
            return clonEAP;
        }
    }
}
