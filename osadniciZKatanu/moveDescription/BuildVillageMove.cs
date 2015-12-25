﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class BuildVillageMove : Move
    {
        public VertexDesc BuildingCoord { get; private set; } // souřadnice stavby (pokud je to cesta, pak souřadnice středu cesty)

        public BuildVillageMove(VertexDesc buildingCoord)
        {
            BuildingCoord = buildingCoord;
        }

        public BuildVillageMove()
        {
        }

        public void BuildVillage(VertexDesc buildingCoord)
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
