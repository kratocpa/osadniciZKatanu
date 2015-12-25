using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class BuildRoadMove : Move
    {
        public EdgeDesc BuildingCoord { get; private set; } // souřadnice stavby (pokud je to cesta, pak souřadnice středu cesty)

        public BuildRoadMove(EdgeDesc buildingCoord)
        {
            BuildingCoord = buildingCoord;
        }

        public BuildRoadMove()
        {
        }

        public void BuildRoad(EdgeDesc buildingCoord)
        {
            BuildingCoord = buildingCoord;
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            res += lang.MoveDescBuildRoad(BuildingCoord.ID);

            return res;
        }
    }
}
