using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    public class GenerateMovesProperties
    {
        public List<Parameter> Parameters = new List<Parameter>();

        public double weightGoodNumbers { get { return Parameters.Find(x => x.Name == "weightGoodNumbers").Scale; } }
        public double weightPortThreeOne { get { return Parameters.Find(x => x.Name == "weightPortThreeOne").Scale; } }
        public double weightPortTwoOne { get { return Parameters.Find(x => x.Name == "weightPortTwoOne").Scale; } }
        public double weightMissingMaterial { get { return Parameters.Find(x => x.Name == "weightMissingMaterial").Scale; } }
        public double weightEdgeGeneral { get { return Parameters.Find(x => x.Name == "weightEdgeGeneral").Scale; } }
        public double weightWood { get { return Parameters.Find(x => x.Name == "weightWood").Scale; } }
        public double weightBrick { get { return Parameters.Find(x => x.Name == "weightBrick").Scale; } }
        public double weightGrain { get { return Parameters.Find(x => x.Name == "weightGrain").Scale; } }
        public double weightSheep { get { return Parameters.Find(x => x.Name == "weightSheep").Scale; } }
        public double weightStone { get { return Parameters.Find(x => x.Name == "weightStone").Scale; } }

        public double weightRoadGeneral { get { return Parameters.Find(x => x.Name == "weightRoadGeneral").Scale; } }
        public double weightLongestRoad { get { return Parameters.Find(x => x.Name == "weightLongestRoad").Scale; } }
        public double weightExtensionRoad { get { return Parameters.Find(x => x.Name == "weightExtensionRoad").Scale; } }
        public double weightSpaceForVillage { get { return Parameters.Find(x => x.Name == "weightSpaceForVillage").Scale; } }

        public double weightThiefMoveBase { get { return Parameters.Find(x => x.Name == "weightThiefMoveBase").Scale; } }
        public double weightThiefMoveOtherBuilding { get { return Parameters.Find(x => x.Name == "weightThiefMoveOtherBuilding").Scale; } }
        public double weightThiefMoveMyBuilding { get { return Parameters.Find(x => x.Name == "weightThiefMoveMyBuilding").Scale; } }
        public double weightThiefMoveProb { get { return Parameters.Find(x => x.Name == "weightThiefMoveProb").Scale; } }

        public double weightVillageGeneral { get { return Parameters.Find(x => x.Name == "weightVillageGeneral").Scale; } }
        public double weightVillageGoodNumbers { get { return Parameters.Find(x => x.Name == "weightVillageGoodNumbers").Scale; } }
        public double weightVillagePortThreeOne { get { return Parameters.Find(x => x.Name == "weightVillagePortThreeOne").Scale; } }
        public double weightVillagePortTwoOne { get { return Parameters.Find(x => x.Name == "weightVillagePortTwoOne").Scale; } }

        public double weightTownGeneral { get { return Parameters.Find(x => x.Name == "weightTownGeneral").Scale; } }
        public double weightTownGoodNumbers { get { return Parameters.Find(x => x.Name == "weightTownGoodNumbers").Scale; } }

        public double weightBuyActionCardGeneral { get { return Parameters.Find(x => x.Name == "weightBuyActionCardGeneral").Scale; } }
        public double weightUseActionCardGeneral { get { return Parameters.Find(x => x.Name == "weightUseActionCardGeneral").Scale; } }
        public double weightUseMatFromPlGeneral { get { return Parameters.Find(x => x.Name == "weightUseMatFromPlGeneral").Scale; } }
        public double weightUseTwoMatRoadGeneral { get { return Parameters.Find(x => x.Name == "weightUseTwoMatRoadGeneral").Scale; } }
        public double weightUseTwoMatVillageGeneral { get { return Parameters.Find(x => x.Name == "weightUseTwoMatVillageGeneral").Scale; } }
        public double weightUseTwoMatTownGeneral { get { return Parameters.Find(x => x.Name == "weightUseTwoMatTownGeneral").Scale; } }
        public double weightUseTwoMatActGeneral { get { return Parameters.Find(x => x.Name == "weightUseTwoMatActGeneral").Scale; } }
        public double weightUseTwoRoadGeneral { get { return Parameters.Find(x => x.Name == "weightUseTwoRoadGeneral").Scale; } }

        public GenerateMovesProperties() 
        {
            Parameters.Add(new Parameter("weightGoodNumbers", 60));
            Parameters.Add(new Parameter("weightPortThreeOne", 10));
            Parameters.Add(new Parameter("weightPortTwoOne", 10));
            Parameters.Add(new Parameter("weightMissingMaterial", 40));
            Parameters.Add(new Parameter("weightEdgeGeneral", 10));
            Parameters.Add(new Parameter("weightWood", 50));
            Parameters.Add(new Parameter("weightBrick", 50));
            Parameters.Add(new Parameter("weightGrain", 40));
            Parameters.Add(new Parameter("weightSheep", 40));
            Parameters.Add(new Parameter("weightStone", 20));
            Parameters.Add(new Parameter("weightRoadGeneral", 10));
            Parameters.Add(new Parameter("weightLongestRoad", 50));
            Parameters.Add(new Parameter("weightExtensionRoad", 35));
            Parameters.Add(new Parameter("weightSpaceForVillage", 30));
            Parameters.Add(new Parameter("weightThiefMoveBase", 5));
            Parameters.Add(new Parameter("weightThiefMoveOtherBuilding", 40));
            Parameters.Add(new Parameter("weightThiefMoveMyBuilding", 60));
            Parameters.Add(new Parameter("weightThiefMoveProb", 10));
            Parameters.Add(new Parameter("weightVillageGeneral", 200));
            Parameters.Add(new Parameter("weightVillageGoodNumbers", 60));
            Parameters.Add(new Parameter("weightVillagePortThreeOne", 10));
            Parameters.Add(new Parameter("weightVillagePortTwoOne", 10));
            Parameters.Add(new Parameter("weightTownGeneral", 150));
            Parameters.Add(new Parameter("weightTownGoodNumbers", 60));
            Parameters.Add(new Parameter("weightBuyActionCardGeneral", 80));
            Parameters.Add(new Parameter("weightUseActionCardGeneral", 60));
            Parameters.Add(new Parameter("weightUseMatFromPlGeneral", 100));
            Parameters.Add(new Parameter("weightUseTwoMatRoadGeneral", 50));
            Parameters.Add(new Parameter("weightUseTwoMatVillageGeneral", 70));
            Parameters.Add(new Parameter("weightUseTwoMatTownGeneral", 90));
            Parameters.Add(new Parameter("weightUseTwoMatActGeneral", 60));
            Parameters.Add(new Parameter("weightUseTwoRoadGeneral", 20));           
        }

        public void LoadFromArray(int[] parameters)
        {
            for(int i=0; i<parameters.Count(); i++)
            {
                Parameters[i].Scale=parameters[i];
            }
        }

        public bool LoadFromXml(string fileName)
        {
            XmlDocument propDoc = new XmlDocument();

            try
            {
                propDoc.Load(fileName);
                foreach (var par in Parameters)
                {
                    par.Scale = double.Parse(propDoc.DocumentElement.Attributes[par.Name].Value, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return true;
        }

        public class Parameter
        {
            public string Name { get; private set; }
            public double Scale { get; set; }

            public Parameter(string name, int scale)
            {
                Name = name;
                Scale = scale;
            }

            public Parameter(string name)
            {
                Name = name;
                Scale = 0;
            }
        }
    }
}
