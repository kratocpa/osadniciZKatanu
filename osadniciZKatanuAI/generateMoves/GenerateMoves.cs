using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public List<FirstPhaseGameMove> GenerateFirstRoadAndVillage(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateFsAndScMoves gener = new GenerateFsAndScMoves(movesProp);
            return gener.Generate(gmProp, plProp);
        }

        public List<FirstPhaseGameMove> GenerateSecondRoadAndVillage(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateFsAndScMoves gener = new GenerateFsAndScMoves(movesProp);
            return gener.Generate(gmProp, plProp);
        }

        public List<BuildRoadMove> GenerateBuildRoadMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateRoadMoves gener = new GenerateRoadMoves(movesProp);
            return gener.Generate(gmProp, plProp);
        }

        public List<ThiefMove> GenerateThiefMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateThiefMoves gener = new GenerateThiefMoves(movesProp);
            return gener.Generate(gmProp, plProp);
        }

        public List<BuildVillageMove> GenerateBuildVillageMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateVillageMoves gener = new GenerateVillageMoves(movesProp);
            return gener.Generate(gmProp, plProp);
        }

        public List<BuildTownMove> GenerateBuildTownMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateTownMoves gener = new GenerateTownMoves(movesProp);
            return gener.Generate(gmProp, plProp);
        }


        public List<BuyActionCardMove> GenerateBuyActionCardMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateBuyActionCardMoves gener = new GenerateBuyActionCardMoves(movesProp);
            return gener.Generate(gmProp, plProp);
        }

        public List<Move> GenerateUseActionCardMoves(GameProperties gmProp, PlayerProperties plProp)
        {
            GenerateUseActionCardMoves gener = new GenerateUseActionCardMoves(movesProp);
            return gener.Generate(gmProp, plProp);
        }
    }
}
