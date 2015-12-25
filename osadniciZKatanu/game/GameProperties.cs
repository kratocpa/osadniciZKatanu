using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace osadniciZKatanu
{
    public class GameProperties
    {
        public int MinPointsToWin { get; private set; }
        public int LongestRoad { get; private set; }
        public int MaxKnights { get; private set; }

        public int VillageProduction { get; private set; }
        public int TownProduction { get; private set; }
        public int LongestRoadProduction { get; private set; }
        public int LargestArmyProduction { get; private set; }

        public int SpecialPortRate { get; private set; }
        public int UniversalPortRate { get; private set; }
        public int NoPortRate { get; private set; }

        public int RoadRemaining { get; private set; }
        public int VillageRemaining { get; private set; }
        public int TownRemaining { get; private set; }

        public ActionCardCollection RemainingActionCards { get; private set; }
        public MaterialCollection MaterialsForRoad { get; private set; }
        public MaterialCollection MaterialsForVillage { get; private set; }
        public MaterialCollection MaterialsForTown { get; private set; }
        public MaterialCollection MaterialsForActionCard { get; private set; }

        public List<Vertex> Vertices;
        public List<Face> Faces;
        public List<Edge> Edges;

        public GameProperties()
        {
            RemainingActionCards = new ActionCardCollection();
            MaterialsForRoad = new MaterialCollection();
            MaterialsForVillage = new MaterialCollection();
            MaterialsForTown = new MaterialCollection();
            MaterialsForActionCard = new MaterialCollection();

            Vertices = new List<Vertex>();
            Faces = new List<Face>();
            Edges = new List<Edge>();
        }

        public void LoadFromXml()
        {
            SetGameSettings();
            SetPlayerSettings();
            SetGameBorderSetings();
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
                    case "knight": RemainingActionCards.SetQuantity(GameDesc.actionCards.knight, count); break;
                    case "coupon": RemainingActionCards.SetQuantity(GameDesc.actionCards.coupon, count); break;
                    case "materialFromPlayers": RemainingActionCards.SetQuantity(GameDesc.actionCards.materialsFromPlayers, count); break;
                    case "twoMaterials": RemainingActionCards.SetQuantity(GameDesc.actionCards.twoMaterials, count); break;
                    case "twoRoad": RemainingActionCards.SetQuantity(GameDesc.actionCards.twoRoad, count); break;
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
                    case "wood": result.SetQuantity(GameDesc.materials.wood, count); break;
                    case "brick": result.SetQuantity(GameDesc.materials.brick, count); break;
                    case "sheep": result.SetQuantity(GameDesc.materials.sheep, count); break;
                    case "grain": result.SetQuantity(GameDesc.materials.grain, count); break;
                    case "stone": result.SetQuantity(GameDesc.materials.stone, count); break;

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
                    Vertices.Add(curVx);
                }

                //načtení souřadnic portů
                VxDoc.LoadXml(Properties.Resources.ports);
                foreach (XmlNode curNode in VxDoc.DocumentElement.ChildNodes)
                {
                    Coord point = new Coord();
                    point.X = double.Parse(curNode.Attributes["xCoord"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    point.Y = double.Parse(curNode.Attributes["yCoord"].Value, System.Globalization.CultureInfo.InvariantCulture);

                    GameDesc.materials portMaterials = GameDesc.RecogniseMaterials(curNode.FirstChild.InnerText);
                    Vertices.Find(x => x.Coordinate.X == point.X &&
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
                    GameDesc.materials matFace = GameDesc.RecogniseMaterials(curNode.FirstChild.InnerText);
                    int numFace = int.Parse(curNode.LastChild.InnerText);
                    Face curFc = new Face(point, matFace, numFace);
                    curFc.ID = ID;
                    ID++;
                    Faces.Add(curFc);
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
            foreach (Vertex fsCurVx in Vertices)
            {
                foreach (Vertex scCurVx in Vertices)
                {
                    if(SetGameBorder.NeighboringVertices(fsCurVx, scCurVx) && fsCurVx != scCurVx)
                    {
                        bool notAdd = false;
                        var curL = new Tuple<Coord, Coord>(fsCurVx.Coordinate, scCurVx.Coordinate);
                        foreach (var findL in Edges)
                        {
                            if (GameBorderDesc.SameLine(curL, findL.Coordinate))
                            {
                                notAdd = true;
                            }
                        }
                        if (!notAdd) {
                            Edge curEg = new Edge(curL);
                            curEg.ID = ID;
                            ID++;
                            Edges.Add(curEg); 
                        }
                    }
                }
            }
        }
    }
}
