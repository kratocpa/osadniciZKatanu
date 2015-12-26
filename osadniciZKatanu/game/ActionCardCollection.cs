using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class ActionCardCollection : ActionCardCollectionDesc, ICloneable
    {
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

        public override object Clone()
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
