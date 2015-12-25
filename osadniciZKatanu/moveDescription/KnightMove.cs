using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class KnightMove : Move
    {
        public FaceDesc ThiefCoord {get; private set;} // souřadnice kam chci zloděje přesunout
        public GameDesc.color RobbedPlayer{get; private set;} // barva okradeného hráče, noColor, pokud neokradu žádného hráče

        public KnightMove(FaceDesc thiefCoord, GameDesc.color robbedPlayer)
        {
            ThiefCoord = thiefCoord;
            RobbedPlayer = robbedPlayer;
        }

        public KnightMove(FaceDesc thiefCoord)
        {
            ThiefCoord = thiefCoord;
            RobbedPlayer = GameDesc.color.noColor;
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            if (RobbedPlayer == GameDesc.color.noColor) { res += lang.MoveDescKnight(ThiefCoord.ID); }
            else { res += lang.MoveDescKnight(ThiefCoord.ID, RobbedPlayer); }
            return res;
        }
    }
}
