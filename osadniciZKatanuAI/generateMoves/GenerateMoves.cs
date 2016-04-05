using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    class GenerateMoves
    {

        GenerateMovesProperties movesProp;
        
        public GenerateMoves()
        {
            movesProp = new GenerateMovesProperties();
        }

        public GenerateMoves(int[] param)
        {
            movesProp = new GenerateMovesProperties();
            movesProp.LoadFromArray(param);
        }

        public GenerateMoves(string filename)
        {
            movesProp = new GenerateMovesProperties();
            movesProp.LoadFromXml(filename);
        }

        public GenerateMoves(XmlNode xmlNode)
        {
            movesProp = new GenerateMovesProperties();
            movesProp.LoadFromXmlNode(xmlNode);
        }

        public GenerateMoves(GenerateMovesProperties genMvProp)
        {
            movesProp = genMvProp;
        }

        public List<FirstPhaseGameMove> GenerateFirstRoadAndVillage(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateFsAndScMoves gener = new GenerateFsAndScMoves(movesProp, gmProp, plProp);
            return gener.Generate();
        }

        public List<FirstPhaseGameMove> GenerateSecondRoadAndVillage(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateFsAndScMoves gener = new GenerateFsAndScMoves(movesProp, gmProp, plProp);
            return gener.Generate();
        }

        public List<BuildRoadMove> GenerateBuildRoadMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateRoadMoves gener = new GenerateRoadMoves(movesProp, gmProp, plProp);
            return gener.Generate();
        }

        public List<ThiefMove> GenerateThiefMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateThiefMoves gener = new GenerateThiefMoves(movesProp, gmProp, plProp);
            return gener.Generate();
        }

        public List<BuildVillageMove> GenerateBuildVillageMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateVillageMoves gener = new GenerateVillageMoves(movesProp, gmProp, plProp);
            return gener.Generate();
        }

        public List<BuildTownMove> GenerateBuildTownMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateTownMoves gener = new GenerateTownMoves(movesProp, gmProp, plProp);
            return gener.Generate();
        }


        public List<BuyActionCardMove> GenerateBuyActionCardMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateBuyActionCardMoves gener = new GenerateBuyActionCardMoves(movesProp, gmProp, plProp);
            return gener.Generate();
        }

        public List<Move> GenerateUseActionCardMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateUseActionCardMoves gener = new GenerateUseActionCardMoves(movesProp, gmProp, plProp);
            return gener.Generate();
        }
    }
}
