using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    public class MyGameLogic : IGameLogic
    {
        int[] param;
        GenerateMoves genMov;
        List<Move> possibleMoves;
        GameProperties gmProp;
        PlayerProperties plProp;

        public MyGameLogic()
        {
            genMov = new GenerateMoves();
        }

        public MyGameLogic(int[] param)
        {
            this.param = param;
            genMov = new GenerateMoves(param);
        }

        public MyGameLogic(string filename)
        {
            genMov = new GenerateMoves(filename);
        }

        public Move GenerateMove(GameProperties gmProp, PlayerProperties plProp)
        {
            this.gmProp = gmProp;
            this.plProp = plProp;
            possibleMoves = GeneratePossibleMoves();

            //Random rand = new Random();

            //double sum = 0;
            //foreach (var curMv in possibleMoves) { sum = sum + curMv.fitnessMove; }

            ////výběr tahu stylem ruleta
            //double rnd = rand.Next(0, (int)sum);
            //sum = 0;
            //for (int i = 0; i < possibleMoves.Count; i++)
            //{
            //    sum = sum + possibleMoves[i].fitnessMove;

            //    if (sum > rnd)
            //    {
            //        return possibleMoves[i];
            //    }
            //}

            ////nic jsem nevybral, končím s tahem
            //MoveDescription mvDesc = new MoveDescription();
            //mvDesc.IAmDone();
            //return mvDesc;

            //výběr nejlepšího tahu
            double max = 0;
            Move bestMove = null;
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                if (max < possibleMoves[i].fitnessMove)
                {
                    max = possibleMoves[i].fitnessMove;
                    bestMove = possibleMoves[i];
                }
            }

            if (bestMove == null)
            {
                return new NothingToDoMove();
            }
            else
            {
                return bestMove;
            }
        }

        public List<Move> GetAllPossibleMoves()
        {
            return possibleMoves;
        }

        private List<Move> GeneratePossibleMoves()
        {
            List<Move> possibleMoves = new List<Move>();

            if (!plProp.FirstPathCreated && !plProp.FirstVillageCreated &&
                !plProp.SecondPathCreated && !plProp.SecondVillageCreated)
            {
                foreach (var curIt in genMov.GenerateFirstRoadAndVillage(gmProp, plProp)) { possibleMoves.Add(curIt); }
                return possibleMoves;
            }
            else if (plProp.FirstPathCreated && plProp.FirstVillageCreated &&
                !plProp.SecondPathCreated && !plProp.SecondVillageCreated)
            {
                foreach (var curIt in genMov.GenerateFirstRoadAndVillage(gmProp, plProp)) { possibleMoves.Add(curIt); }
                return possibleMoves;
            }
            else if (gmProp.NeedToMoveThief)
            {
                foreach (var curIt in genMov.GenerateThiefMoves(gmProp, plProp)) { possibleMoves.Add(curIt); }
                return possibleMoves;
            }
            else
            {
                if (!gmProp.wasBuildSomething)
                {
                    foreach (var curIt in genMov.GenerateBuildRoadMoves(gmProp, plProp)) { possibleMoves.Add(curIt); }
                }

                if (!gmProp.wasBuildSomething)
                {
                    foreach (var curIt in genMov.GenerateBuildVillageMoves(gmProp, plProp)) { possibleMoves.Add(curIt); }
                }

                if (!gmProp.wasBuildSomething)
                {
                    foreach (var curIt in genMov.GenerateBuildTownMoves(gmProp, plProp)) { possibleMoves.Add(curIt); }
                }

                if (!gmProp.wasBuildSomething)
                {
                    foreach (var curIt in genMov.GenerateBuyActionCardMoves(gmProp, plProp)) { possibleMoves.Add(curIt); }
                }

                if (!gmProp.wasUseActionCard)
                {
                    foreach (var curIt in genMov.GenerateUseActionCardMoves(gmProp, plProp)) { possibleMoves.Add(curIt); }
                }
            }
            return possibleMoves;
        }

    }
}
