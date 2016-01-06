using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class ActionCardCollection : ICloneable
    {
        public List<ActionCardStruct> ActionCards { get; protected set; }
        private Random rnd;

        public ActionCardCollection()
        {
            ActionCards = new List<ActionCardStruct>();
            ActionCards.Add(new ActionCardStruct(Game.actionCards.coupon, 0));
            ActionCards.Add(new ActionCardStruct(Game.actionCards.knight, 0));
            ActionCards.Add(new ActionCardStruct(Game.actionCards.materialsFromPlayers, 0));
            ActionCards.Add(new ActionCardStruct(Game.actionCards.twoMaterials, 0));
            ActionCards.Add(new ActionCardStruct(Game.actionCards.twoRoad, 0));

            rnd = new Random();
        }

        /// <summary>
        /// navýší množství akční karty typu raisedActCard
        /// </summary>
        /// <param name="raisedActCard">která akční karta se má navýšit</param>
        /// <param name="umbel">o kolik se má počet navýšit</param>
        public void RaiseQuantity(Game.actionCards raisedActCard, int umbel)
        {
            ActionCardStruct act = ActionCards.Find(x => x.ActionCardType == raisedActCard);
            if (act == null) { throw new CantDeleteActionCardException("Can't delete action card"); }
            act.Quantity += umbel;
        }

        /// <summary>
        /// sníží množství akční karty typu decreaseActCard
        /// </summary>
        /// <param name="decreaseActCard">u které akční karty se má snížit počet</param>
        /// <param name="umbel">o kolik se má množství snížit</param>
        public void DecreaseQuantity(Game.actionCards decreaseActCard, int umbel)
        {
            ActionCardStruct act = ActionCards.Find(x => x.ActionCardType == decreaseActCard);
            if (act == null || act.Quantity < umbel) { throw new CantDeleteActionCardException("Can't delete action card"); }
            act.Quantity -= umbel;
        }

        /// <summary>
        /// nastaví množství akční karty na zadanou hodnotu
        /// </summary>
        /// <param name="setActCard">o kterou akční kartu se jedná</param>
        /// <param name="setValue">na kolik se má nastavit množství</param>
        public void SetQuantity(Game.actionCards setActCard, int setValue)
        {
            ActionCardStruct act = ActionCards.Find(x => x.ActionCardType == setActCard);
            if (act == null || setValue < 0) { throw new CantDeleteActionCardException("Can't delete action card"); }
            act.Quantity = setValue;
        }

        public void AddActionCard(Game.actionCards addedActCard)
        {
            RaiseQuantity(addedActCard, 1);
        }

        public void DeleteActionCard(Game.actionCards deletedActCard)
        {
            DecreaseQuantity(deletedActCard, 1);
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

        /// <summary>
        /// vybere náhodnou akční kartu
        /// </summary>
        /// <returns>vrátí noActionCard, pokud je balíček prázdný</returns>
        public Game.actionCards PickRandomActionCard()
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
            return Game.actionCards.noActionCard;
        }

        public class ActionCardStruct
        {
            public int Quantity { get; set; }
            public Game.actionCards ActionCardType { get; set; }

            public ActionCardStruct(Game.actionCards actionCardType, int quantity)
            {
                ActionCardType = actionCardType;
                Quantity = quantity;
            }
        }

        public object Clone()
        {
            ActionCardCollection clone = new ActionCardCollection();

            foreach (var curAct in ActionCards)
            {
                clone.ActionCards.Find(x => x.ActionCardType == curAct.ActionCardType).Quantity = curAct.Quantity;
            }

            return clone;
        }
    }
}
