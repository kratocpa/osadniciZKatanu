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
        public EvolutionAlgorithm.mating MatingManner { get; set; } // způsob křížení (jednobodové, dvoubodové, uniformní)
        public double MutationProb { get; set; } // pravděpodobnost mutace
        public double MutationChangeBitProb { get; set; } // pravděpodobnost mutace jednoho bitu

        //nastavení fitness funkce
        public EvolutionAlgorithm.fitnessEvaluator EvaluatorManner { get; set; } // fitness funkce (pevně danný protihráči, všichni proti všem, elo)
        public int PlayersCountInGame { get; set; } // počet hráčů v jedné testované hře při ohodnocování jedinců
        
        //nastavení pro fitness funkci s pevně danými protihráči
        public string FirstRival; // první soupeř  
        public string SecondRival; // druhý soupeř 
        public string ThirdRival; // třetí soupeř
        
        //nastavení pro fitness funci elo
        public double InitialElo { get; set; } // elo, které má hráč který zatím nehrál žádnou hru

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
            try
            {
                EvaPropDoc.Load(xmlFile);
                PopSize = int.Parse(EvaPropDoc.DocumentElement.Attributes["popSize"].Value);
                GenerationCount = int.Parse(EvaPropDoc.DocumentElement.Attributes["generationCount"].Value);

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
            catch (Exception ex)
            {
                //TODO: přidat výjimky
                Console.WriteLine(ex.Message);
            }
        }

        private void SetMatingManner(XmlNode curNode)
        {
            string type = curNode.Attributes["type"].Value;
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
            MutationProb = int.Parse(curNode.Attributes["mutationProb"].Value);
            MutationChangeBitProb = int.Parse(curNode.Attributes["mutationChangeBitProb"].Value);
        }

        private void SetEvaluatorManner(XmlNode curNode)
        {
            string type = curNode.Attributes["type"].Value;
            switch (type)
            {
                case "basic": EvaluatorManner = EvolutionAlgorithm.fitnessEvaluator.Basic; break;
                case "ebdWithEbd": EvaluatorManner = EvolutionAlgorithm.fitnessEvaluator.EbdWithEbd; break;
                case "elo": EvaluatorManner = EvolutionAlgorithm.fitnessEvaluator.Elo; break;
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
                    default: break;
                }
            }
        }

        public object Clone()
        {
            EvolutionAlgorithmProperties clonEAP = new EvolutionAlgorithmProperties();
            clonEAP.PopSize = PopSize;
            clonEAP.GenerationCount = GenerationCount;
            clonEAP.MatingManner = MatingManner;
            clonEAP.MutationProb = MutationProb;
            clonEAP.MutationChangeBitProb = MutationChangeBitProb;
            clonEAP.EvaluatorManner = EvaluatorManner;
            clonEAP.PlayersCountInGame = PlayersCountInGame;
            clonEAP.FirstRival = FirstRival;
            clonEAP.SecondRival = SecondRival;
            clonEAP.ThirdRival = ThirdRival;
            clonEAP.InitialElo = InitialElo;
            return clonEAP;
        }
    }
}
