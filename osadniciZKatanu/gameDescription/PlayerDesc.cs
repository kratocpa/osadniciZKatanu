//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace osadniciZKatanu
//{
//    public class PlayerDesc
//    {
//        public int RoadRemaining { get; protected set; } // počet cest, které my ještě zbývají (tolik jich teoreticky ještě mohu postavit)
//        public int VillageRemaining { get; protected set; } // počet vesnic, které mi zbývají
//        public int TownRemaining { get; protected set; } // počet měst, které mi zbývají

//        public MaterialCollectionDesc MaterialsDesc { get; protected set; }  // seznam surovin hráče
//        public ActionCardCollectionDesc ActionCardsDesc { get; protected set; } // seznam nevyložených akčních karet hráče
//        public ActionCardCollectionDesc LinedActionCardsDesc { get; protected set; } // seznam vyložených akčních karet hráče
//        public List<VertexDesc> VillageDesc { get; protected set; } // seznam postavených vesnic hráče
//        public List<VertexDesc> TownDesc { get; protected set; } // seznam postavených měst
//        public List<EdgeDesc> RoadDesc { get; protected set; } // seznam postavených cest

//        public bool LargestArmy { get; set; } // true - tento hráč má ze všech hráčů největší počet rytířů, false - nemá
//        public bool LongestWay { get; set; } // true - tento hráč má nejdelší cestu ze všech, false - nemá
//        public int LongestWayLength { get; set; } // délka nejdelší cesty tohoto hráče
//        public bool RealPlayer { get; protected set; } // true - tento hráč zadává tahy pomocí gui aplikace, false - tento hráč jenom předává popisy tahů (MoveDescription)
//        public Game.color Color { get; protected set; } // barva hráče

//        public int Points { get; set; } // počet bodů hráče
//        public int Knights { get; set; } // počet vyložených rytířů hráče

//        public bool FirstVillageCreated { get; set; } // true - první vesnice hráče byla vytvořena 
//        public bool FirstPathCreated { get; set; } // true - první cesta hráče byla vytvořena
//        public bool SecondVillageCreated { get; set; } // true - druhá vesnice hráče byla vytvořena
//        public bool SecondPathCreated { get; set; } // true - druhá cesta hráče byla vytvořena

//        public bool UniversalPort { get; set; } // hráč má vesnici na přístavu bez suroviny (může využívat směnný kurz 3:1)
//        public List<Game.materials> PortForMaterial { get; set; } // hráč má vesnici na přístavu se surovinou (může pro tuto surovinu využívat směný kurz 2:1)

//        public PlayerDesc(Game.color playerColor, bool real, GameProperties gmProp)
//        {
//            Color = playerColor;
//            RealPlayer = real;
//            LargestArmy = false;
//            LongestWay = false;
//            LongestWayLength = 0;
//            VillageDesc = new List<VertexDesc>();
//            TownDesc = new List<VertexDesc>();
//            RoadDesc = new List<EdgeDesc>();
//            FirstVillageCreated = false;
//            FirstPathCreated = false;
//            SecondVillageCreated = false;
//            SecondPathCreated = false;
//            Points = 0;
//            Knights = 0;
//            ActionCardsDesc = new ActionCardCollectionDesc();
//            MaterialsDesc = new MaterialCollectionDesc();

//            RoadRemaining = gmProp.RoadRemaining;
//            VillageRemaining = gmProp.VillageRemaining;
//            TownRemaining = gmProp.TownRemaining;
//            UniversalPort = false;
//            PortForMaterial = new List<Game.materials>();
//        }

//        static List<EdgeDesc> DeleteEdgeDesc(EdgeDesc deletedEdge, List<EdgeDesc> deletedList)
//        {
//            var finalList = new List<EdgeDesc>();
//            foreach (var curEdge in deletedList)
//            {
//                if (deletedEdge != curEdge)
//                {
//                    finalList.Add(curEdge);
//                }
//            }
//            return finalList;
//        }

//        public static Tuple<VertexDesc, int> FindFurthermostVertexDesc(int distance, VertexDesc initialVertex, List<EdgeDesc> roadList)
//        {
//            List<VertexDesc> succesor = new List<VertexDesc>();
//            List<Tuple<VertexDesc, int>> furthermostVertices = new List<Tuple<VertexDesc, int>>();
//            foreach (var curEdge in roadList)
//            {
//                if (curEdge.VertexNeighborsDesc.First() == initialVertex)
//                {
//                    var furthermostVertex1 = FindFurthermostVertexDesc(distance + 1, curEdge.VertexNeighborsDesc.Last(), DeleteEdgeDesc(curEdge, roadList));

//                    furthermostVertices.Add(furthermostVertex1);
//                }
//                else if (curEdge.VertexNeighborsDesc.Last() == initialVertex)
//                {
//                    var furthermostVertex2 = FindFurthermostVertexDesc(distance + 1, curEdge.VertexNeighborsDesc.First(), DeleteEdgeDesc(curEdge, roadList));
//                    furthermostVertices.Add(furthermostVertex2);
//                }
//            }

//            int max = 0;
//            Tuple<VertexDesc, int> furthermostVertex = new Tuple<VertexDesc, int>(initialVertex, distance);
//            foreach (var curTuple in furthermostVertices)
//            {
//                if (curTuple.Item2 > max)
//                {
//                    furthermostVertex = curTuple;
//                    max = curTuple.Item2;
//                }
//            }
//            return furthermostVertex;
//        }

//    }
//}
