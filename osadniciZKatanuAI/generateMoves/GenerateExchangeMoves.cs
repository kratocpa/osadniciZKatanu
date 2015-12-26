using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    class GenerateExchangeMoves
    {
        GenerateMovesProperties movesProp;
        public enum typeMove { buyActionCard, buildRoad, buildVillage, buildTown };
        public GenerateExchangeMoves()
        {
            movesProp = new GenerateMovesProperties();
        }

        public GenerateExchangeMoves(GenerateMovesProperties movesProp)
        {
            this.movesProp = movesProp;
        }

        public Move Generate(GameProperties gmProp, PlayerProperties plProp, MaterialCollection whatIWant, typeMove tm)
        {
            Move result = new Move();

            switch (tm)
            {
                case typeMove.buildRoad: result = new BuildRoadMove(); break;
                case typeMove.buildTown: result = new BuildTownMove(); break;
                case typeMove.buildVillage: result = new BuildVillageMove(); break;
                case typeMove.buyActionCard: result = new BuyActionCardMove(); break;
            }

            MaterialCollection copyOfMyMaterial = (MaterialCollection)plProp.Materials.Clone();

            int countOfMissMat = CountOfMissingMaterials(whatIWant, copyOfMyMaterial);

            foreach (var curMatToChange in copyOfMyMaterial.Materials)
            {
                if (countOfMissMat > 0)
                {
                    int portRate;
                    int count = curMatToChange.Quantity - whatIWant.GetQuantity(curMatToChange.MaterialType);
                    if (plProp.PortForMaterial.Contains(curMatToChange.MaterialType))
                    {
                        count = count / gmProp.SpecialPortRate;
                        portRate = gmProp.SpecialPortRate;
                    }
                    else if (plProp.UniversalPort)
                    {
                        count = count / gmProp.UniversalPortRate;
                        portRate = gmProp.UniversalPortRate;
                    }
                    else
                    {
                        count = count / gmProp.NoPortRate;
                        portRate = gmProp.NoPortRate;
                    }

                    while (count > 0 && countOfMissMat > 0)
                    {
                        foreach (var curMat in whatIWant.Materials)
                        {
                            if (curMat.Quantity > copyOfMyMaterial.GetQuantity(curMat.MaterialType))
                            {
                                copyOfMyMaterial.AddMaterial(curMat.MaterialType);
                                curMatToChange.Quantity-=portRate;
                                count--;
                                countOfMissMat--;
                                result.ChangeMaterial(curMatToChange.MaterialType, curMat.MaterialType);
                                break;
                            }
                        }

                    }
                }
                else
                {
                    return result;
                }
            }

            if (countOfMissMat > 0)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public int CountOfMissingMaterials(MaterialCollection whatIWant, MaterialCollection whatIHave)
        {
            int countOfMissMat = 0;
            foreach (var curMat in whatIWant.Materials)
            {
                int iHave = whatIHave.GetQuantity(curMat.MaterialType);
                if (iHave < curMat.Quantity)
                {
                    countOfMissMat += curMat.Quantity - iHave;
                }
            }
            return countOfMissMat;
        }

    }
}
