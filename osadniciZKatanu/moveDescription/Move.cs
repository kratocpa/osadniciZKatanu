using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class Move
    {
        public List<DoubleMaterials> ChangedMaterials { get; private set; } // seznam surovin k výměně
        public double fitnessMove { get; set; } // udává kvalitu tahu

        public Move()
        {
            ChangedMaterials = new List<DoubleMaterials>();
            fitnessMove = 0;
        }

        public Move(GameDesc.materials matFrom, GameDesc.materials matTo)
        {
            ChangedMaterials = new List<DoubleMaterials>();
            fitnessMove = 0;
            ChangedMaterials.Add(new DoubleMaterials(matFrom, matTo));
        }

        public void ChangeMaterial(GameDesc.materials matFrom, GameDesc.materials matTo)
        {
            ChangedMaterials.Add(new DoubleMaterials(matFrom, matTo));
        }

        public struct DoubleMaterials
        {
            public readonly GameDesc.materials MatFrom;
            public readonly GameDesc.materials MatTo;

            public DoubleMaterials(GameDesc.materials matFrom, GameDesc.materials matTo)
            {
                MatFrom = matFrom;
                MatTo = matTo;
            }
        }

        public virtual string MoveDescription(ILanguage lang)
        {
            string res = "";
            for (int i = 0; i < ChangedMaterials.Count; i++)
            {
                res += lang.MoveDescExchangeMat(ChangedMaterials[i].MatFrom, ChangedMaterials[i].MatTo);
                if (i != ChangedMaterials.Count - 1)
                {
                    res += "\n";
                }
            }
            return res;
        }

    }

}
