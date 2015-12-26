//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace osadniciZKatanu
//{
//    public class GameDesc
//    {
//        public const int MAX_DISTANCE_VERTEX = 40;
//        public const int MAX_DISTANCE_FACE = 70;

//        public int LongestRoad { get; protected set; } // zatím nejdelší cesta ( výchozí je 4, kdo má 5 získá jako první kartu nejdelší cesta )
//        public int MaxKnights { get; protected set; } //největší armáda, výchozí je 2

//        public int VillageProduction { get; protected set; } // počet bodů za vesnici
//        public int TownProduction { get; protected set; } // počet bodů za město
//        public int LongestRoadProduction { get; protected set; } // počet bodů za nejdelší cestu
//        public int LargestArmyProduction { get; protected set; } // počet bodů za největší vojsko

//        public int MinPointsToWin { get; protected set; } // počet bodů na výhru
//        public int SpecialPortRate { get; protected set; } // kurz výměny suroviny u přístavu se zadanou surovinou
//        public int UniversalPortRate { get; protected set; } // kurz výměny suroviny u přístavu bez uvedené suroviny
//        public int NoPortRate { get; protected set; } // kurz výměny suroviny bez přístavu

//        public int Round { get; protected set; } // uvádí kolikáté kolo hry se odehrává
//        public state CurrentState { get; protected set; } // aktuální stav hry (začátek, hra, konec...)
//        public int FallenNum { get { return FirstDice + SecondDice; } } // číslo které padlo na obou hracích kostkách
//        public int FirstDice { get; protected set; } // číslo které padlo na první kostce
//        public int SecondDice { get; protected set; } // číslo které padlo na druhé kostce
//        public bool NeedToMoveThief { get; set; } // padla 7, je třeba přemístit zloděje
//        public bool wasBuildSomething { get; protected set; } // bylo už za tah něco postaveno
//        public bool wasUseActionCard { get; protected set; } // byla za tah užita akční karta
//        public bool actionCardsPackedIsEmpty { get; protected set; } // je balíček akčních karet prázdný

//        public List<PlayerDesc> PlayersDesc; // seznam všech hráčů
//        public PlayerDesc ActualPlayerDesc; // hráč na tahu
//        public FaceDesc ThiefFaceDesc; // stěna, na které se nachází zloděj

//        public GameBorderDesc GameBorderDesc;

//        public ActionCardCollectionDesc RemainingActionCardsDesc { get; protected set; } // seznam zbývajících akčních karet
//        public MaterialCollectionDesc materialForVillageDesc { get; protected set; } // suroviny potřebné na stavbu vesnice
//        public MaterialCollectionDesc materialForTownDesc { get; protected set; } // suroviny potřebné na stavbu města
//        public MaterialCollectionDesc materialForActionCardDesc { get; protected set; } // suroviny potřebné na koupi akční karty
//        public MaterialCollectionDesc materialForRoadDesc { get; protected set; } // suroviny potřebné na stavbu cesty

//        public GameDesc(GameProperties gmProp)
//        {
//            PlayersDesc = new List<PlayerDesc>();
//            RemainingActionCardsDesc = new ActionCardCollectionDesc();

//            materialForVillageDesc = (MaterialCollectionDesc)gmProp.MaterialsForVillage.Clone();
//            materialForTownDesc = (MaterialCollectionDesc)gmProp.MaterialsForTown.Clone();
//            materialForActionCardDesc = (MaterialCollectionDesc)gmProp.MaterialsForActionCard.Clone();
//            materialForRoadDesc = (MaterialCollectionDesc)gmProp.MaterialsForRoad.Clone();

//            CurrentState = state.start;
//            LongestRoad = gmProp.LongestRoad;
//            MaxKnights = gmProp.MaxKnights;

//            VillageProduction = gmProp.VillageProduction;
//            TownProduction = gmProp.TownProduction;
//            LongestRoadProduction = gmProp.LongestRoadProduction;
//            LargestArmyProduction = gmProp.LargestArmyProduction;

//            MinPointsToWin = gmProp.MinPointsToWin;
//            SpecialPortRate = gmProp.SpecialPortRate;
//            UniversalPortRate = gmProp.UniversalPortRate;
//            NoPortRate = gmProp.NoPortRate;

//            NeedToMoveThief = false;
//            wasBuildSomething = false;
//            wasUseActionCard = false;
//        }

//        public static materials RecogniseMaterials(string strMaterials)
//        {
//            //TODO: přidat aby to nebylo case sensitive, tedy aby při vstupu např. GraIN vrátila funkce materials.grain
//            materials portMaterials;
//            switch (strMaterials)
//            {
//                case "brick": portMaterials = materials.brick; break;
//                case "grain": portMaterials = materials.grain; break;
//                case "desert": portMaterials = materials.desert; break;
//                case "noMaterial": portMaterials = materials.noMaterial; break;
//                case "sheep": portMaterials = materials.sheep; break;
//                case "stone": portMaterials = materials.stone; break;
//                case "wood": portMaterials = materials.wood; break;
//                default: portMaterials = materials.noMaterial; break;
//            }

//            return portMaterials;
//        }

//        public static actionCards RecogniseActionCard(string strActionCard)
//        {
//            //TODO: přidat aby to nebylo case sensitive
//            actionCards act;
//            switch (strActionCard)
//            {
//                case "coupon": act = actionCards.coupon; break;
//                case "knight": act = actionCards.knight; break;
//                case "materialsFromPlayers": act = actionCards.materialsFromPlayers; break;
//                case "noActionCard": act = actionCards.noActionCard; break;
//                case "twoMaterials": act = actionCards.twoMaterials; break;
//                case "twoRoad": act = actionCards.twoRoad; break;
//                default: act = actionCards.noActionCard; break;
//            }

//            return act;
//        }

//    }
//}
