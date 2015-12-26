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

        public List<BuyActionCardMove> Generate(GameProperties gmProp, PlayerProperties plProp)
        {
            List<BuyActionCardMove> possibleBuyActCardMoves = new List<BuyActionCardMove>();

            if (plProp.Materials.IsPossibleDelete(gmProp.MaterialsForActionCard) &&
                !gmProp.actionCardsPackedIsEmpty &&
                !gmProp.wasBuildSomething)
            {
                BuyActionCardMove mvDesc = new BuyActionCardMove();
                mvDesc.fitnessMove = RateBuyActionCard(gmProp);
                possibleBuyActCardMoves.Add(mvDesc);
            }
            else if (!gmProp.wasBuildSomething && gmProp.RemainingActionCards.GetSumAllActionCard() > 0)
            {
                BuyActionCardMove mvDesc = (BuyActionCardMove)exchange.Generate(gmProp, plProp, gmProp.MaterialsForActionCard, GenerateExchangeMoves.typeMove.buyActionCard);
                if (mvDesc != null)
                {
                    mvDesc.fitnessMove = RateBuyActionCard(gmProp);
                    possibleBuyActCardMoves.Add(mvDesc);
                }
            }

            return possibleBuyActCardMoves;
        }

        private double RateBuyActionCard(GameProperties gmProp)
        {
            return movesProp.weightBuyActionCardGeneral;
        }
    }
}
