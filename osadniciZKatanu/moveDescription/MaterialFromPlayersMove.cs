using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class MaterialFromPlayersMove : Move
    {
        public GameDesc.materials PickedMaterial { get; private set; }

        public MaterialFromPlayersMove(GameDesc.materials pickedMaterial)
        {
            PickedMaterial = pickedMaterial;
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            res += lang.MoveDescMatFromPl(PickedMaterial);
            return res;
        }
    }
}
