using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class KnightMove : Move
    {
        public Face ThiefCoord {get; private set;} // souřadnice kam chci zloděje přesunout
        public Game.color RobbedPlayer{get; private set;} // barva okradeného hráče, noColor, pokud neokradu žádného hráče

        public KnightMove(Face thiefCoord, Game.color robbedPlayer)
        {
            ThiefCoord = thiefCoord;
            RobbedPlayer = robbedPlayer;
        }

        public KnightMove(Face thiefCoord)
        {
            ThiefCoord = thiefCoord;
            RobbedPlayer = Game.color.noColor;
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            if (RobbedPlayer == Game.color.noColor) { res += lang.MoveDescKnight(ThiefCoord.ID); }
            else { res += lang.MoveDescKnight(ThiefCoord.ID, RobbedPlayer); }
            return res;
        }
    }
}
