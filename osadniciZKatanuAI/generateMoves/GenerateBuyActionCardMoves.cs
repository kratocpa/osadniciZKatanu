using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osadniciZKatanu;

namespace osadniciZKatanuAI
{
    class GenerateBuyActionCardMoves
    {

        GenerateMovesProperties movesProp;
        GenerateExchangeMoves exchange;

        public GenerateBuyActionCardMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
        }

        public GenerateBuyActionCardMoves(GenerateMovesProperties movesProp)
        {
            this.movesProp = movesProp;
            exchange = new GenerateExchangeMoves(movesProp);
        }

        public List<BuyActionCardMove> Generate(GameDesc gmDesc)
        {
            List<BuyActionCardMove> possibleBuyActCardMoves = new List<BuyActionCardMove>();

            if (gmDesc.ActualPlayerDesc.MaterialsDesc.IsPossibleDelete(gmDesc.materialForActionCardDesc) &&
                gmDesc.RemainingActionCardsDesc.GetSumAllActionCard() > 0 &&
                !gmDesc.wasBuildSomething)
            {
                BuyActionCardMove mvDesc = new BuyActionCardMove();
                mvDesc.fitnessMove = RateBuyActionCard(gmDesc);
                possibleBuyActCardMoves.Add(mvDesc);
            }
            else if (!gmDesc.wasBuildSomething && gmDesc.RemainingActionCardsDesc.GetSumAllActionCard() > 0)
            {
                BuyActionCardMove mvDesc = (BuyActionCardMove)exchange.Generate(gmDesc.materialForActionCardDesc, gmDesc, GenerateExchangeMoves.typeMove.buyActionCard);
                if (mvDesc != null)
                {
                    mvDesc.fitnessMove = RateBuyActionCard(gmDesc);
                    possibleBuyActCardMoves.Add(mvDesc);
                }
            }

            return possibleBuyActCardMoves;
        }

        private double RateBuyActionCard(GameDesc gmDesc)
        {
            return movesProp.weightBuyActionCardGeneral;
        }
    }
}
