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

        public List<FirstPhaseGameMove> GenerateFirstRoadAndVillage(GameDesc gmDesc)
        {
            GenerateFsAndScMoves gener = new GenerateFsAndScMoves(movesProp);
            return gener.Generate(gmDesc);
        }

        public List<FirstPhaseGameMove> GenerateSecondRoadAndVillage(GameDesc gmDesc)
        {
            GenerateFsAndScMoves gener = new GenerateFsAndScMoves(movesProp);
            return gener.Generate(gmDesc);
        }

        public List<BuildRoadMove> GenerateBuildRoadMoves(GameDesc gmDesc)
        {
            GenerateRoadMoves gener = new GenerateRoadMoves(movesProp);
            return gener.Generate(gmDesc);
        }

        public List<ThiefMove> GenerateThiefMoves(GameDesc gmDesc)
        {
            GenerateThiefMoves gener = new GenerateThiefMoves(movesProp);
            return gener.Generate(gmDesc);
        }

        public List<BuildVillageMove> GenerateBuildVillageMoves(GameDesc gmDesc)
        {
            GenerateVillageMoves gener = new GenerateVillageMoves(movesProp);
            return gener.Generate(gmDesc);
        }

        public List<BuildTownMove> GenerateBuildTownMoves(GameDesc gmDesc)
        {
            GenerateTownMoves gener = new GenerateTownMoves(movesProp);
            return gener.Generate(gmDesc);
        }


        public List<BuyActionCardMove> GenerateBuyActionCardMoves(GameDesc gmDesc)
        {
            GenerateBuyActionCardMoves gener = new GenerateBuyActionCardMoves(movesProp);
            return gener.Generate(gmDesc);
        }

        public List<Move> GenerateUseActionCardMoves(GameDesc gmDesc)
        {
            GenerateUseActionCardMoves gener = new GenerateUseActionCardMoves(movesProp);
            return gener.Generate(gmDesc);
        }
    }
}
