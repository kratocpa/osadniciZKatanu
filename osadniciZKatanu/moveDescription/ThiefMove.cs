using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class ThiefMove : Move
    {
        public Face ThiefCoord {get; set;} // souřadnice kam chci zloděje přesunout
        public Game.color RobbedPlayer{get; set;} // barva okradeného hráče, noColor pokud neokradu žádného hráče

        public ThiefMove(Face thiefCoord, Game.color robbedPlayer)
        {
            ThiefCoord = thiefCoord;
            RobbedPlayer = robbedPlayer;
        }

        public ThiefMove(Face thiefCoord)
        {
            ThiefCoord = thiefCoord;
            RobbedPlayer = Game.color.noColor;
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            if (RobbedPlayer == Game.color.noColor) { res += lang.MoveDescThief(ThiefCoord.ID); }
            else { res += lang.MoveDescThief(ThiefCoord.ID, RobbedPlayer); }
            return res;
        }
    }
}
