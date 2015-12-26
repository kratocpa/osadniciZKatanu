using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class MaterialCollection : ICloneable
    {
        public List<MaterialStruct> Materials { get; protected set; }
        private Random rnd;

        public MaterialCollection()
        {
            Materials = new List<MaterialStruct>();
            Materials.Add(new MaterialStruct(Game.materials.brick, 0));
            Materials.Add(new MaterialStruct(Game.materials.grain, 0));
            Materials.Add(new MaterialStruct(Game.materials.sheep, 0));
            Materials.Add(new MaterialStruct(Game.materials.stone, 0));
            Materials.Add(new MaterialStruct(Game.materials.wood, 0));

            rnd = new Random();
        }

        /// <summary>
        /// navýší množství suroviny typu raisedMaterial
        /// </summary>
        /// <param name="raisedMaterial">která surovina se má navýšit</param>
        /// <param name="umbel">o kolik se má počet suroviny navýšit</param>
        /// <returns>true - povedlo se navýšit, tedy surovina typu raisedMaterial se nachází v seznamu materiálů</returns>
        public void RaiseQuantity(Game.materials raisedMaterial, int umbel)
        {
            MaterialStruct mat = Materials.Find(x => x.MaterialType == raisedMaterial);
            if (mat == null) { throw new CantDeleteMaterialsException("Can't delete materials"); }
            mat.Quantity += umbel;
        }

        /// <summary>
        /// sníží množství suroviny typu decreaseMaterial
        /// </summary>
        /// <param name="decreaseMaterial">u které suroviny se má snížit počet</param>
        /// <param name="umbel">o kolik se má množství suroviny snížit</param>
        /// <returns>true - povedlo se snížit množství, tedy surovina typu decreaseMaterial je v seznamu surovin a po odečtení hodnoty umbel nebude množství suroviny záporné</returns>
        public void DecreaseQuantity(Game.materials decreaseMaterial, int umbel)
        {
            MaterialStruct mat = Materials.Find(x => x.MaterialType == decreaseMaterial);
            if (mat == null || mat.Quantity < umbel) { throw new CantDeleteMaterialsException("Can't delete materials"); }
            mat.Quantity -= umbel;
        }

        /// <summary>
        /// nastaví množství suroviny na zadanou hodnotu
        /// </summary>
        /// <param name="setMaterial">o kterou surovinu se jedná</param>
        /// <param name="setValue">na kolik se má nastavit množství</param>
        /// <returns>true - hodnota setValue není záporná a zadaná surovina se nachází v seznamu</returns>
        public void SetQuantity(Game.materials setMaterial, int setValue)
        {
            MaterialStruct mat = Materials.Find(x => x.MaterialType == setMaterial);
            if (mat == null || setValue < 0) { throw new CantDeleteMaterialsException("Can't delete materials"); }
            mat.Quantity = setValue;
        }

        public void AddMaterial(Game.materials addedMaterial)
        {
            RaiseQuantity(addedMaterial, 1);
        }

        public void DeleteMaterial(Game.materials deletedMaterial)
        {
            DecreaseQuantity(deletedMaterial, 1);
        }

        public void DeleteMaterials(MaterialCollection deletedMaterials)
        {
            if (!IsPossibleDelete(deletedMaterials))
            {
                throw new CantDeleteMaterialsException("Can't delete materials");
            }

            foreach (var curMat in deletedMaterials.Materials)
            {
                DecreaseQuantity(curMat.MaterialType, curMat.Quantity);
            }
        }

        /// <summary>
        /// Nastaví množství všech materiálů v seznamu na 0
        /// </summary>
        public void NullAllMaterials()
        {
            foreach (var curMat in Materials)
            {
                curMat.Quantity = 0;
            }
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

        public bool IsPossibleDelete(MaterialCollection testedMat)
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

        public object Clone()
        {
            MaterialCollection clone = new MaterialCollection();

            foreach (var curMat in Materials)
            {
                clone.Materials.Find(x => x.MaterialType == curMat.MaterialType).Quantity = curMat.Quantity;
            }

            return clone;
        }

    }
}
