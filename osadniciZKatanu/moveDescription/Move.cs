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
        public double fitnessMove { get; set; } // udává kvalitu (fitness) tahu

        public Move()
        {
            ChangedMaterials = new List<DoubleMaterials>();
            fitnessMove = 0;
        }

        public Move(Game.materials matFrom, Game.materials matTo)
        {
            ChangedMaterials = new List<DoubleMaterials>();
            fitnessMove = 0;
            ChangedMaterials.Add(new DoubleMaterials(matFrom, matTo));
        }

        public void ChangeMaterial(Game.materials matFrom, Game.materials matTo)
        {
            ChangedMaterials.Add(new DoubleMaterials(matFrom, matTo));
        }

        public struct DoubleMaterials
        {
            public readonly Game.materials MatFrom;
            public readonly Game.materials MatTo;

            public DoubleMaterials(Game.materials matFrom, Game.materials matTo)
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
