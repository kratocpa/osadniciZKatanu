using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
                    case "evaluatorManner": SetEvaluatorManner(curNode); break;
                    default: break;
                }
            }
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
                    default: break;
                }
            }
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
