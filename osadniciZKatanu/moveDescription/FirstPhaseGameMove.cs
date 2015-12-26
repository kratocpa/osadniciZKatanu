using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class FirstPhaseGameMove : Move
    {
        public Vertex VillageCoord { get; private set; } // souřadnice vesnice
        public Edge RoadCoord { get; private set; } // souřadnice středu cesty

        public FirstPhaseGameMove(Vertex villageCoord, Edge roadCoord)
        {
            VillageCoord = villageCoord;
            RoadCoord = roadCoord;
        }

        public FirstPhaseGameMove()
        {
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            res += lang.MoveDescFirstVillAndRoad(VillageCoord.ID, RoadCoord.ID);
            return res;
        }

    }
}
