using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class MaterialCollection : MaterialCollectionDesc, ICloneable
    {
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

        public override object Clone()
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
