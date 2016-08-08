using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Player
{
    public class Player
    {
        public Cards.Hand Hand { get; private set; } = null;
        public bool IsActive { get; private set; } = true;
        public String Name { get; }
        public int Cash { get { return _wallet.Balance; } }

        private Wallet _wallet;

        public Player(String name, int initialCash)
        {
            _wallet = new Wallet(initialCash);
            Name = name;
        }

        public void CreateHand(params Cards.Card[] cards)
        {
            Hand = new Cards.Hand(cards);
        }

        public bool TryBet(int amount)
        {
            if (_wallet.CanPay(amount))
            {
                _wallet.Pay(amount);
                return true;
            }
            else
            {
                IsActive = false;
                return false;
            }
        }

        public void CollectWinnings(int amount)
        {
            _wallet.AddBalance(amount);
        }

        public override string ToString()
        {
            return String.Format("{0}: {1:C}", Name, Cash);
        }

        public String ToString(bool showHand)
        {
            return showHand ? ToStringWithHand() : ToString();
        }

        private String ToStringWithHand()
        {
            return String.Format("{0} ({1:C}): {2}", Name, Cash, Hand);
        }

    }
}
