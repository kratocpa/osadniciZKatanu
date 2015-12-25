using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class ActionCardCollectionDesc : ICloneable
    {
        public List<ActionCardStruct> ActionCards { get; protected set; }
        private Random rnd;

        public ActionCardCollectionDesc()
        {
            ActionCards = new List<ActionCardStruct>();
            ActionCards.Add(new ActionCardStruct(GameDesc.actionCards.coupon, 0));
            ActionCards.Add(new ActionCardStruct(GameDesc.actionCards.knight, 0));
            ActionCards.Add(new ActionCardStruct(GameDesc.actionCards.materialsFromPlayers, 0));
            ActionCards.Add(new ActionCardStruct(GameDesc.actionCards.twoMaterials, 0));
            ActionCards.Add(new ActionCardStruct(GameDesc.actionCards.twoRoad, 0));

            rnd = new Random();
        }

        public int GetSumAllActionCard()
        {
            int sum = 0;
            foreach (var curAct in ActionCards)
            {
                sum += curAct.Quantity;
            }
            return sum;
        }

        public GameDesc.actionCards PickRandomActionCard()
        {
            int sum = GetSumAllActionCard();

            if (sum > 0)
            {
                int curSum = 0;
                int pickNum = rnd.Next(1, sum);
                foreach (var curAct in ActionCards)
                {
                    if (curSum < pickNum && curSum + curAct.Quantity >= pickNum)
                    {
                        return curAct.ActionCardType;
                    }
                    curSum += curAct.Quantity;
                }
            }
            return GameDesc.actionCards.noActionCard;
        }

        public class ActionCardStruct
        {
            public int Quantity { get; set; }
            public GameDesc.actionCards ActionCardType { get; set; }

            public ActionCardStruct(GameDesc.actionCards actionCardType, int quantity)
            {
                ActionCardType = actionCardType;
                Quantity = quantity;
            }
        }

        public virtual object Clone()
        {
            ActionCardCollectionDesc clone = new ActionCardCollectionDesc();

            foreach (var curAct in ActionCards)
            {
                clone.ActionCards.Find(x => x.ActionCardType == curAct.ActionCardType).Quantity = curAct.Quantity;
            }

            return clone;
        }
    }
}
