using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace osadniciZKatanu
{
    public class GameProperties : ICloneable
    {
        public int MinPointsToWin { get; set; } // minimální počet bodů k vítězství
        public int LongestRoad { get; set; } // zatím nejdelší cesta (na začátku je hodnota 4)
        public int MaxKnights { get; set; } // nejvíc rytířů které má nějaký hráč (na začátku 2)

        public int VillageProduction { get; set; } // počet bodů za vesnici (také kolik produkuje vesnice suroviny)
        public int TownProduction { get; set; } // počet bodů za město (také kolik suroviny produkuje město)
        public int LongestRoadProduction { get; set; } // počet bodů za nejdelší cestu
        public int LargestArmyProduction { get; set; } // počet bodů za největší počet rytířů

        public int SpecialPortRate { get; set; } // kurz výměny suroviny u speciálního portu (přístav se surovinou) 
        public int UniversalPortRate { get; set; } // kurz výměny u univerzálního přístavu ( 3:1 )
        public int NoPortRate { get; set; } // kurz výměny surovin bez přístavu

        public int RoadRemaining { get; set; } // počet cest které má každý hráč na začátku hry
        public int VillageRemaining { get; set; } // počet vesnic které má každý hráč na začátku hry
        public int TownRemaining { get; set; } // počet měst které má každý hráč na začátku hry

        public ActionCardCollection RemainingActionCards { get; private set; } // akční karty v balíčku
        public MaterialCollection MaterialsForRoad { get; private set; } // seznam surovin potřebných na koupi cesty
        public MaterialCollection MaterialsForVillage { get; private set; } // seznam surovin potřebných na koupi vesnice
        public MaterialCollection MaterialsForTown { get; private set; } // seznam surovin potřebných na koupi města
        public MaterialCollection MaterialsForActionCard { get; private set; } // seznam surovin potřebných na koupi akční karty

        public Face ThiefFace { get; set; } // stěna, na který je zloděj

        public int Round { get; set; } // uvádí kolikáté kolo hry se odehrává
        public Game.state CurrentState { get; set; } // aktuální stav hry (začátek, hra, konec...)
        public int FallenNum { get { return FirstDice + SecondDice; } } // číslo které padlo na obou hracích kostkách
        public int FirstDice { get; set; } // číslo které padlo na první kostce
        public int SecondDice { get; set; } // číslo které padlo na druhé kostce
        public bool NeedToMoveThief { get; set; } // padla 7, je třeba přemístit zloděje
        public bool wasBuildSomething { get; set; } // bylo už za tah něco postaveno
        public bool wasUseActionCard { get; set; } // byla za tah užita akční karta
        public bool actionCardsPackedIsEmpty { get { return RemainingActionCards.GetSumAllActionCard() == 0; } } // je balíček akčních karet prázdný

        public GameBorder GameBorderData; // popis hrací desky
        public ILanguage CurLang { get; private set; } // jazyk hry

        private List<Vertex> vertices;
        private List<Face> faces;
        private List<Edge> edges;

        private SetGameBorder st;
        private bool randomGameBorder;

        public GameProperties(bool isRandomGameBorder, ILanguage curLang)
        {
            RemainingActionCards = new ActionCardCollection();
            MaterialsForRoad = new MaterialCollection();
            MaterialsForVillage = new MaterialCollection();
            MaterialsForTown = new MaterialCollection();
            MaterialsForActionCard = new MaterialCollection();

            vertices = new List<Vertex>();
            faces = new List<Face>();
            edges = new List<Edge>();

            st = new SetGameBorder();
            randomGameBorder = isRandomGameBorder;
            //LoadFromXml();

            Round = 0;
            CurrentState = Game.state.start;
            FirstDice = 0;
            SecondDice = 0;
            NeedToMoveThief = false;
            wasBuildSomething = false;
            wasUseActionCard = false;
            CurLang = curLang;
            //GameBorderData = st.GenerateGameBorder(isRandomGameBorder, vertices, faces, edges);
            //ThiefFace = GameBorderData.Faces.Find(x => x.Material == Game.materials.desert);
        }

        public void LoadFromXml()
        {
            SetGameSettings();
            SetPlayerSettings();
            SetGameBorderSetings();
            GameBorderData = st.GenerateGameBorder(randomGameBorder, vertices, faces, edges);
            ThiefFace = GameBorderData.Faces.Find(x => x.Material == Game.materials.desert);
        }

        /// <summary>
        /// Načte nastavení hry ze souboru game.xml
        /// </summary>
        private void SetGameSettings()
        {
            XmlDocument GameDoc = new XmlDocument();

            try
            {
                GameDoc.LoadXml(Properties.Resources.game);
                MinPointsToWin = int.Parse(GameDoc.DocumentElement.Attributes["pointsToWin"].Value);
                LongestRoad = int.Parse(GameDoc.DocumentElement.Attributes["longestRoad"].Value);
                MaxKnights = int.Parse(GameDoc.DocumentElement.Attributes["largestArmy"].Value);

                VillageProduction = int.Parse(GameDoc.DocumentElement.Attributes["villageProduction"].Value);
                TownProduction = int.Parse(GameDoc.DocumentElement.Attributes["townProduction"].Value);
                LongestRoadProduction = int.Parse(GameDoc.DocumentElement.Attributes["longestRoadProduction"].Value);
                LargestArmyProduction = int.Parse(GameDoc.DocumentElement.Attributes["largestArmyProduction"].Value);

                foreach (XmlNode curNode in GameDoc.DocumentElement.ChildNodes)
                {
                    switch (curNode.Name)
                    {
                        case "actionCardPackage": SetActionCardPackage(curNode); break;
                        case "ports": SetPorts(curNode); break;
                        case "materialsForActions": SetMaterialsForActions(curNode); break;
                        default: break;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: přidat výjimky
                Console.WriteLine(ex.Message);
            }
        }

        private void SetActionCardPackage(XmlNode curNode)
        {
            foreach (XmlNode chNode in curNode.ChildNodes)
            {
                int count = int.Parse(chNode.Attributes["count"].Value);
                switch (chNode.Name)
                {
                    case "knight": RemainingActionCards.SetQuantity(Game.actionCards.knight, count); break;
                    case "coupon": RemainingActionCards.SetQuantity(Game.actionCards.coupon, count); break;
                    case "materialFromPlayers": RemainingActionCards.SetQuantity(Game.actionCards.materialsFromPlayers, count); break;
                    case "twoMaterials": RemainingActionCards.SetQuantity(Game.actionCards.twoMaterials, count); break;
                    case "twoRoad": RemainingActionCards.SetQuantity(Game.actionCards.twoRoad, count); break;
                    default: break;
                }
            }
        }

        private void SetPorts(XmlNode curNode)
        {
            foreach (XmlNode chNode in curNode.ChildNodes)
            {
                int rate = int.Parse(chNode.Attributes["rate"].Value);
                switch (chNode.Name)
                {
                    case "specialPort": SpecialPortRate = rate; break;
                    case "universalPort": UniversalPortRate = rate; break;
                    case "noPort": NoPortRate = rate; break;
                    default: break;
                }
            }
        }

        private void SetMaterialsForActions(XmlNode curNode)
        {
            foreach (XmlNode chNode in curNode.ChildNodes)
            {
                switch (chNode.Name)
                {
                    case "road": MaterialsForRoad = GetMaterialListFromXMl(chNode); break;
                    case "village": MaterialsForVillage = GetMaterialListFromXMl(chNode); break;
                    case "town": MaterialsForTown = GetMaterialListFromXMl(chNode); break;
                    case "actionCard": MaterialsForActionCard = GetMaterialListFromXMl(chNode); break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// načte seznam surovin z XML a vrátí je v Listu
        /// </summary>
        /// <param name="curNode">element v xml, který obsahuje seznam surovin</param>
        /// <returns></returns>
        private MaterialCollection GetMaterialListFromXMl(XmlNode curNode)
        {
            MaterialCollection result = new MaterialCollection();

            foreach (XmlNode chNode in curNode.ChildNodes)
            {
                int count = int.Parse(chNode.Attributes["count"].Value);
                switch (chNode.Name)
                {
                    case "wood": result.SetQuantity(Game.materials.wood, count); break;
                    case "brick": result.SetQuantity(Game.materials.brick, count); break;
                    case "sheep": result.SetQuantity(Game.materials.sheep, count); break;
                    case "grain": result.SetQuantity(Game.materials.grain, count); break;
                    case "stone": result.SetQuantity(Game.materials.stone, count); break;

                    default: break;
                }
            }
            return result;
        }

        /// <summary>
        /// Načte nastavení hráče ze souboru player.xml
        /// </summary>
        private void SetPlayerSettings()
        {
            XmlDocument PlayerDoc = new XmlDocument();

            try
            {
                PlayerDoc.LoadXml(Properties.Resources.player);
                RoadRemaining = int.Parse(PlayerDoc.DocumentElement.Attributes["roadCount"].Value);
                VillageRemaining = int.Parse(PlayerDoc.DocumentElement.Attributes["villageCount"].Value);
                TownRemaining = int.Parse(PlayerDoc.DocumentElement.Attributes["townCount"].Value);
            }
            catch (Exception ex)
            {
                //TODO: přidat výjimky
                Console.WriteLine(ex.Message);
            }
        }

        private void SetGameBorderSetings()
        {
            SetVertices();
            SetFaces();
            SetEdges();
        }

        private void SetVertices()
        {
            XmlDocument VxDoc = new XmlDocument();
            int ID = 0;
            try
            {
                //načtení souřadnic vrcholů
                VxDoc.LoadXml(Properties.Resources.vertices);
                foreach (XmlNode curNode in VxDoc.DocumentElement.ChildNodes)
                {
                    Coord point = new Coord();
                    point.X = double.Parse(curNode.Attributes["xCoord"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    point.Y = double.Parse(curNode.Attributes["yCoord"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    Vertex curVx = new Vertex(point);
                    curVx.ID = ID;
                    ID++;
                    vertices.Add(curVx);
                }

                //načtení souřadnic portů
                VxDoc.LoadXml(Properties.Resources.ports);
                foreach (XmlNode curNode in VxDoc.DocumentElement.ChildNodes)
                {
                    Coord point = new Coord();
                    point.X = double.Parse(curNode.Attributes["xCoord"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    point.Y = double.Parse(curNode.Attributes["yCoord"].Value, System.Globalization.CultureInfo.InvariantCulture);

                    Game.materials portMaterials = Game.RecogniseMaterials(curNode.FirstChild.InnerText);
                    vertices.Find(x => x.Coordinate.X == point.X &&
                        x.Coordinate.Y == point.Y).addPort(portMaterials);
                }
            }
            catch (Exception ex)
            {
                //TODO: přidat výjimky
                Console.WriteLine(ex.Message);
            }
        }

        private void SetFaces()
        {

            XmlDocument VxDoc = new XmlDocument();
            int ID = 0;
            try
            {
                VxDoc.LoadXml(Properties.Resources.faces);
                foreach (XmlNode curNode in VxDoc.DocumentElement.ChildNodes)
                {
                    Coord point = new Coord();
                    point.X = double.Parse(curNode.Attributes["xCoord"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    point.Y = double.Parse(curNode.Attributes["yCoord"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    Game.materials matFace = Game.RecogniseMaterials(curNode.FirstChild.InnerText);
                    int numFace = int.Parse(curNode.LastChild.InnerText);
                    Face curFc = new Face(point, matFace, numFace);
                    curFc.ID = ID;
                    ID++;
                    faces.Add(curFc);
                }
            }
            catch (Exception ex)
            {
                //TODO: přidat výjimky
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// vrátí list s hranami, hrany vytvoří jako všechny dvojice v určité vzdálenosti od sebe 
        /// </summary>
        private void SetEdges()
        {
            int ID = 0;
            foreach (Vertex fsCurVx in vertices)
            {
                foreach (Vertex scCurVx in vertices)
                {
                    if(SetGameBorder.NeighboringVertices(fsCurVx, scCurVx) && fsCurVx != scCurVx)
                    {
                        bool notAdd = false;
                        var curL = new Tuple<Coord, Coord>(fsCurVx.Coordinate, scCurVx.Coordinate);
                        foreach (var findL in edges)
                        {
                            if (GameBorder.SameLine(curL, findL.Coordinate))
                            {
                                notAdd = true;
                            }
                        }
                        if (!notAdd) {
                            Edge curEg = new Edge(curL);
                            curEg.ID = ID;
                            ID++;
                            edges.Add(curEg); 
                        }
                    }
                }
            }
        }

        public object Clone()
        {
            GameProperties clonGP = new GameProperties(this.randomGameBorder, this.CurLang);
            clonGP.MinPointsToWin = this.MinPointsToWin;
            clonGP.LongestRoad = this.LongestRoad;
            clonGP.MaxKnights = this.MaxKnights;

            clonGP.VillageProduction = this.VillageProduction;
            clonGP.TownProduction = this.TownProduction;
            clonGP.LongestRoadProduction = this.LongestRoadProduction;
            clonGP.LargestArmyProduction = this.LargestArmyProduction;

            clonGP.SpecialPortRate = this.SpecialPortRate;
            clonGP.UniversalPortRate = this.UniversalPortRate;
            clonGP.NoPortRate = this.NoPortRate;

            clonGP.RoadRemaining = this.RoadRemaining;
            clonGP.VillageRemaining = this.VillageRemaining;
            clonGP.TownRemaining = this.TownRemaining;

            clonGP.RemainingActionCards = (ActionCardCollection)this.RemainingActionCards.Clone();
            clonGP.MaterialsForRoad = (MaterialCollection)this.MaterialsForRoad.Clone();
            clonGP.MaterialsForVillage = (MaterialCollection)this.MaterialsForVillage.Clone();
            clonGP.MaterialsForTown = (MaterialCollection)this.MaterialsForTown.Clone();
            clonGP.MaterialsForActionCard = (MaterialCollection)this.MaterialsForActionCard.Clone();

            clonGP.ThiefFace = (Face)this.ThiefFace.Clone();

            clonGP.Round = this.Round;
            clonGP.CurrentState = this.CurrentState;
            clonGP.FirstDice = this.FirstDice;
            clonGP.SecondDice = this.SecondDice;
            clonGP.NeedToMoveThief = this.NeedToMoveThief;
            clonGP.wasBuildSomething = this.wasBuildSomething;
            clonGP.wasUseActionCard = this.wasUseActionCard;

            clonGP.CurLang = this.CurLang;

            foreach (Vertex curVx in this.vertices) { clonGP.vertices.Add((Vertex)curVx.Clone()); }
            foreach (Edge curEg in this.edges) { clonGP.edges.Add((Edge)curEg.Clone()); }
            foreach (Face curFc in this.faces) { clonGP.faces.Add((Face)curFc.Clone()); }

            GameBorderData = st.GenerateGameBorder(randomGameBorder, vertices, faces, edges);
            clonGP.GameBorderData = clonGP.st.GenerateGameBorder(this.randomGameBorder, clonGP.vertices, clonGP.faces, clonGP.edges);
            return clonGP;
        }
    }
}
