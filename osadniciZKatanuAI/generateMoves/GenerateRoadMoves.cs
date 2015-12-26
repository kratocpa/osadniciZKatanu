using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    class GenerateRoadMoves
    {
        GenerateMovesProperties movesProp;
        GenerateExchangeMoves exchange;
        CommonFeatures common;

        public GenerateRoadMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
            common = new CommonFeatures();
        }

        public GenerateRoadMoves(GenerateMovesProperties movesProp)
        {
            this.movesProp = movesProp;
            exchange = new GenerateExchangeMoves(movesProp);
            common = new CommonFeatures(movesProp);
        }

        public List<BuildRoadMove> Generate(GameProperties gmProp, PlayerProperties plProp)
        {
            List<BuildRoadMove> possibleRoadMoves = new List<BuildRoadMove>();

            if (plProp.Materials.IsPossibleDelete(gmProp.MaterialsForRoad) && plProp.RoadRemaining>0)
            {
                var possibleEdges = common.GeneratePossibleEdgesToBuildRoad(gmProp, plProp);
                foreach (var curEg in possibleEdges)
                {
                    BuildRoadMove mvDesc = new BuildRoadMove(curEg);
                    mvDesc.fitnessMove = common.RateRoad(gmProp, plProp, curEg);
                    possibleRoadMoves.Add(mvDesc);
                }
            }
            else if (plProp.RoadRemaining > 0)
            {
                BuildRoadMove mvDesc = (BuildRoadMove)exchange.Generate(gmProp, plProp, gmProp.MaterialsForRoad, GenerateExchangeMoves.typeMove.buildRoad);
                if (mvDesc != null)
                {
                    var possibleEdges = common.GeneratePossibleEdgesToBuildRoad(gmProp, plProp);
                    foreach (var curEg in possibleEdges)
                    {
                        mvDesc = (BuildRoadMove)exchange.Generate(gmProp, plProp, gmProp.MaterialsForRoad, GenerateExchangeMoves.typeMove.buildRoad);
                        mvDesc.BuildRoad(curEg);
                        mvDesc.fitnessMove = common.RateRoad(gmProp, plProp, curEg);
                        possibleRoadMoves.Add(mvDesc);
                    }
                }
            }

            return possibleRoadMoves;
        }


    }
}
