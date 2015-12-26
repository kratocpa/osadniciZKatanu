using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class MaterialCollectionDesc : ICloneable
    {
        public List<MaterialStruct> Materials { get; protected set; }
        private Random rnd;

        public MaterialCollectionDesc()
        {
            Materials = new List<MaterialStruct>();
            Materials.Add(new MaterialStruct(Game.materials.brick, 0));
            Materials.Add(new MaterialStruct(Game.materials.grain, 0));
            Materials.Add(new MaterialStruct(Game.materials.sheep, 0));
            Materials.Add(new MaterialStruct(Game.materials.stone, 0));
            Materials.Add(new MaterialStruct(Game.materials.wood, 0));

            rnd = new Random();
        }

        public Game.materials PickRandomMaterial()
        {
            int sum = GetSumAllMaterial();

            if (sum > 0)
            {
                int curSum = 0;
                int pickNum = rnd.Next(1, sum);
                foreach (var curMat in Materials)
                {
                    if (curSum < pickNum && curSum + curMat.Quantity >= pickNum)
                    {
                        return curMat.MaterialType;
                    }
                    curSum += curMat.Quantity;
                }
            }
            return Game.materials.noMaterial;
        }
        

        public int GetSumAllMaterial()
        {
            int sum = 0;
            foreach (var curMat in Materials)
            {
                sum += curMat.Quantity;
            }
            return sum;
        }

        public int GetQuantity(Game.materials searchedMaterial)
        {
            return Materials.Find(x => x.MaterialType == searchedMaterial).Quantity;
        }

        public bool IsPossibleDelete(MaterialCollectionDesc testedMat)
        {
            bool succes = true;
            foreach (MaterialStruct curMat in testedMat.Materials)
            {
                succes = succes && Materials.Find(x => x.MaterialType == curMat.MaterialType).Quantity >= curMat.Quantity;
            }
            return succes;
        }

        public List<Game.materials> ExchangePossibility(int portRate)
        {
            List<Game.materials> possibility = new List<Game.materials>();
            foreach (var curMat in Materials)
            {
                if (curMat.Quantity >= portRate)
                {
                    possibility.Add(curMat.MaterialType);
                }
            }
            return possibility;
        }

        public List<Game.materials> ExchangePossibility(List<Game.materials> portMat, int portRate)
        {
            List<Game.materials> possibility = new List<Game.materials>();
            foreach (var curMat in portMat)
            {
                if (GetQuantity(curMat) >= portRate)
                {
                    possibility.Add(curMat);
                }
            }

            return possibility;
        }

        public class MaterialStruct
        {
            public int Quantity { get; set; }
            public Game.materials MaterialType { get; set; }

            public MaterialStruct(Game.materials materialType, int quantity)
            {
                MaterialType = materialType;
                Quantity = quantity;
            }
        }

        public virtual object Clone()
        {
            MaterialCollectionDesc clone = new MaterialCollectionDesc();

            foreach (var curMat in Materials)
            {
                clone.Materials.Find(x => x.MaterialType == curMat.MaterialType).Quantity = curMat.Quantity;
            }

            return clone;
        }
    }
}
