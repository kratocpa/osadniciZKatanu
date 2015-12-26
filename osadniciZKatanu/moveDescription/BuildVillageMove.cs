using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class BuildVillageMove : Move
    {
        public Vertex BuildingCoord { get; private set; } // souřadnice stavby (pokud je to cesta, pak souřadnice středu cesty)

        public BuildVillageMove(Vertex buildingCoord)
        {
            BuildingCoord = buildingCoord;
        }

        public BuildVillageMove()
        {
        }

        public void BuildVillage(Vertex buildingCoord)
        {
            BuildingCoord = buildingCoord;
        }

        public override string MoveDescription(ILanguage lang)
        {
            string res;
            res = base.MoveDescription(lang);
            if (res != "") { res += "\n"; }

            res += lang.MoveDescBuildVillage(BuildingCoord.ID);

            return res;
        }
    }
}
