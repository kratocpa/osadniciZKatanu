using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class TwoMaterialsMove : Move
    {
        public Game.materials FirstMaterial { get; private set; }
        public Game.materials SecondMaterial { get; private set; }

        public TwoMaterialsMove(Game.materials firstMaterial, Game.materials secondMaterial)
        {
            FirstMaterial = firstMaterial;
            SecondMaterial = secondMaterial;
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            res += lang.MoveDescTwoMat(FirstMaterial, SecondMaterial);
            return res;
        }
    }
}
