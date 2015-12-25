using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class ThiefMove : Move
    {
        public FaceDesc ThiefCoord {get; set;} // souřadnice kam chci zloděje přesunout
        public GameDesc.color RobbedPlayer{get; set;} // barva okradeného hráče, noColor pokud neokradu žádného hráče

        public ThiefMove(FaceDesc thiefCoord, GameDesc.color robbedPlayer)
        {
            ThiefCoord = thiefCoord;
            RobbedPlayer = robbedPlayer;
        }

        public ThiefMove(FaceDesc thiefCoord)
        {
            ThiefCoord = thiefCoord;
            RobbedPlayer = GameDesc.color.noColor;
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            if (RobbedPlayer == GameDesc.color.noColor) { res += lang.MoveDescThief(ThiefCoord.ID); }
            else { res += lang.MoveDescThief(ThiefCoord.ID, RobbedPlayer); }
            return res;
        }
    }
}
