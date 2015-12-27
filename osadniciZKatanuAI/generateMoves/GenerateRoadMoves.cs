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
        GameProperties gmProp;
        PlayerProperties plProp;
        GenerateExchangeMoves exchange;
        CommonFeatures common;

        public GenerateRoadMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
            common = new CommonFeatures();
        }

        public GenerateRoadMoves(GenerateMovesProperties movesProp, GameProperties gmProp, PlayerProperties plProp)
        {
            this.movesProp = movesProp;
            this.gmProp = gmProp;
            this.plProp = plProp;
            exchange = new GenerateExchangeMoves(movesProp, gmProp, plProp);
            common = new CommonFeatures(movesProp, gmProp, plProp);
        }

        public List<BuildRoadMove> Generate()
        {
            List<BuildRoadMove> possibleRoadMoves = new List<BuildRoadMove>();

            if (plProp.Materials.IsPossibleDelete(gmProp.MaterialsForRoad) && plProp.RoadRemaining>0)
            {
                var possibleEdges = common.GeneratePossibleEdgesToBuildRoad();
                foreach (var curEg in possibleEdges)
                {
                    BuildRoadMove mvDesc = new BuildRoadMove(curEg);
                    mvDesc.fitnessMove = common.RateRoad(curEg);
                    possibleRoadMoves.Add(mvDesc);
                }
            }
            else if (plProp.RoadRemaining > 0)
            {
                BuildRoadMove mvDesc = (BuildRoadMove)exchange.Generate(gmProp.MaterialsForRoad, GenerateExchangeMoves.typeMove.buildRoad);
                if (mvDesc != null)
                {
                    var possibleEdges = common.GeneratePossibleEdgesToBuildRoad();
                    foreach (var curEg in possibleEdges)
                    {
                        mvDesc = (BuildRoadMove)exchange.Generate(gmProp.MaterialsForRoad, GenerateExchangeMoves.typeMove.buildRoad);
                        mvDesc.BuildRoad(curEg);
                        mvDesc.fitnessMove = common.RateRoad(curEg);
                        possibleRoadMoves.Add(mvDesc);
                    }
                }
            }

            return possibleRoadMoves;
        }


    }
}
