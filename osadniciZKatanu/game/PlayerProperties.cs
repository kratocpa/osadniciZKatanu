using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class PlayerProperties
    {
        public int RoadRemaining { get; set; } // počet cest, které my ještě zbývají (tolik jich teoreticky ještě mohu postavit)
        public int VillageRemaining { get; set; } // počet vesnic, které mi zbývají
        public int TownRemaining { get; set; } // počet měst, které mi zbývají

        public MaterialCollection Materials { get; private set; }  // seznam surovin hráče
        public ActionCardCollection ActionCards { get; private set; } // seznam nevyložených akčních karet hráče
        public ActionCardCollection LinedActionCards { get; private set; } // seznam vyložených akčních karet hráče
        public List<Vertex> Village { get; private set; } // seznam postavených vesnic hráče
        public List<Vertex> Town { get; private set; } // seznam postavených měst
        public List<Edge> Road { get; protected set; } // seznam postavených cest

        public bool LargestArmy { get; set; } // true - tento hráč má ze všech hráčů největší počet rytířů, false - nemá
        public bool LongestWay { get; set; } // true - tento hráč má nejdelší cestu ze všech, false - nemá
        public int LongestWayLength { get; set; } // délka nejdelší cesty tohoto hráče
        public bool RealPlayer { get; private set; } // true - tento hráč zadává tahy pomocí gui aplikace, false - tento hráč jenom předává popisy tahů (MoveDescription)
        public Game.color Color { get; private set; } // barva hráče

        public int Points { get; set; } // počet bodů hráče
        public int Knights { get; set; } // počet vyložených rytířů hráče

        public bool FirstVillageCreated { get; set; } // true - první vesnice hráče byla vytvořena 
        public bool FirstPathCreated { get; set; } // true - první cesta hráče byla vytvořena
        public bool SecondVillageCreated { get; set; } // true - druhá vesnice hráče byla vytvořena
        public bool SecondPathCreated { get; set; } // true - druhá cesta hráče byla vytvořena

        public bool UniversalPort { get; set; } // hráč má vesnici na přístavu bez suroviny (může využívat směnný kurz 3:1)
        public List<Game.materials> PortForMaterial { get; set; } // hráč má vesnici na přístavu se surovinou (může pro tuto surovinu využívat směný kurz 2:1)

        public PlayerProperties(Game.color plCol, bool real, int roadRemaining, int villageRemaining, int townRemaining)
        {
            Village = new List<Vertex>();
            Town = new List<Vertex>();
            Road = new List<Edge>();
            Materials = new MaterialCollection();
            ActionCards = new ActionCardCollection();
            LinedActionCards = new ActionCardCollection();

            LargestArmy = false;
            LongestWay = false;
            LongestWayLength = 0;
            RealPlayer = real;
            Color = plCol;

            Points = 0;
            Knights = 0;
            
            FirstVillageCreated = false;
            FirstPathCreated = false;
            SecondVillageCreated = false;
            SecondPathCreated = false;

            RoadRemaining = roadRemaining;
            VillageRemaining = villageRemaining;
            TownRemaining = townRemaining;
            UniversalPort = false;
            PortForMaterial = new List<Game.materials>();
        }
    }
}
