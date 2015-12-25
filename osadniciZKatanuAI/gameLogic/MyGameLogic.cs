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
        GenerateMoves genMov;
        List<Move> possibleMoves;

        public MyGameLogic()
        {
            genMov = new GenerateMoves();
        }

        public MyGameLogic(int[] param)
        {
            genMov = new GenerateMoves(param);
        }

        public Move GenerateMove(GameDesc gmDesc)
        {

            possibleMoves = GeneratePossibleMoves(gmDesc);

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

        private List<Move> GeneratePossibleMoves(GameDesc gmDesc)
        {
            List<Move> possibleMoves = new List<Move>();

            if (!gmDesc.ActualPlayerDesc.FirstPathCreated && !gmDesc.ActualPlayerDesc.FirstVillageCreated &&
                !gmDesc.ActualPlayerDesc.SecondPathCreated && !gmDesc.ActualPlayerDesc.SecondVillageCreated)
            {
                foreach (var curIt in genMov.GenerateFirstRoadAndVillage(gmDesc)) { possibleMoves.Add(curIt); }
                return possibleMoves;
            }
            else if (gmDesc.ActualPlayerDesc.FirstPathCreated && gmDesc.ActualPlayerDesc.FirstVillageCreated &&
                !gmDesc.ActualPlayerDesc.SecondPathCreated && !gmDesc.ActualPlayerDesc.SecondVillageCreated)
            {
                foreach (var curIt in genMov.GenerateFirstRoadAndVillage(gmDesc)) { possibleMoves.Add(curIt); }
                return possibleMoves;
            }
            else if (gmDesc.NeedToMoveThief)
            {
                foreach (var curIt in genMov.GenerateThiefMoves(gmDesc)) { possibleMoves.Add(curIt); }
                return possibleMoves;
            }

            if (!gmDesc.wasBuildSomething)
            {
                foreach (var curIt in genMov.GenerateBuildRoadMoves(gmDesc)) { possibleMoves.Add(curIt); }
            }

            if (!gmDesc.wasBuildSomething)
            {
                foreach (var curIt in genMov.GenerateBuildVillageMoves(gmDesc)) { possibleMoves.Add(curIt); }
            }

            if (!gmDesc.wasBuildSomething)
            {
                foreach (var curIt in genMov.GenerateBuildTownMoves(gmDesc)) { possibleMoves.Add(curIt); }
            }

            if (!gmDesc.wasBuildSomething)
            {
                foreach (var curIt in genMov.GenerateBuyActionCardMoves(gmDesc)) { possibleMoves.Add(curIt); }
            }

            if (!gmDesc.wasUseActionCard)
            {
                foreach (var curIt in genMov.GenerateUseActionCardMoves(gmDesc)) { possibleMoves.Add(curIt); }
            }

            return possibleMoves;
        }

    }
}
