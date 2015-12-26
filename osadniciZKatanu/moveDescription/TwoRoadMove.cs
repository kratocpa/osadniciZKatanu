using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class TwoRoadMove : Move
    {
        public Edge FirstRoad {get; private set;}
        public Edge SecondRoad {get; private set;}

        public TwoRoadMove(Edge firstRoad, Edge secondRoad)
        {
            FirstRoad = firstRoad;
            SecondRoad = secondRoad;
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            res += lang.MoveDescTwoRoad(FirstRoad.ID, SecondRoad.ID);
            return res;
        }
    }
}
