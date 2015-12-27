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

        GameProperties gmProp;
        PlayerProperties plProp;

        public GenerateBuyActionCardMoves()
        {
            movesProp = new GenerateMovesProperties();
            exchange = new GenerateExchangeMoves();
        }

        public GenerateBuyActionCardMoves(GenerateMovesProperties movesProp, GameProperties gmProp, PlayerProperties plProp)
        {
            this.movesProp = movesProp;
            this.gmProp = gmProp;
            this.plProp = plProp;
            exchange = new GenerateExchangeMoves(movesProp, gmProp, plProp);
        }

        public List<BuyActionCardMove> Generate()
        {
            List<BuyActionCardMove> possibleBuyActCardMoves = new List<BuyActionCardMove>();

            if (plProp.Materials.IsPossibleDelete(gmProp.MaterialsForActionCard) &&
                !gmProp.actionCardsPackedIsEmpty &&
                !gmProp.wasBuildSomething)
            {
                BuyActionCardMove mvDesc = new BuyActionCardMove();
                mvDesc.fitnessMove = RateBuyActionCard();
                possibleBuyActCardMoves.Add(mvDesc);
            }
            else if (!gmProp.wasBuildSomething && gmProp.RemainingActionCards.GetSumAllActionCard() > 0)
            {
                BuyActionCardMove mvDesc = (BuyActionCardMove)exchange.Generate(gmProp.MaterialsForActionCard, GenerateExchangeMoves.typeMove.buyActionCard);
                if (mvDesc != null)
                {
                    mvDesc.fitnessMove = RateBuyActionCard();
                    possibleBuyActCardMoves.Add(mvDesc);
                }
            }

            return possibleBuyActCardMoves;
        }

        private double RateBuyActionCard()
        {
            return movesProp.weightBuyActionCardGeneral;
        }
    }
}
