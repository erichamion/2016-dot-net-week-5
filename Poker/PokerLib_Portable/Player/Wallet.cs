using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Player
{
    public class Wallet
    {
        public int Balance { get; private set; }

        public Wallet(int initialBalance)
        {
            Balance = initialBalance;
        }

        public void AddBalance(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Cannot add a negative amount to wallet");
            }
            Balance += amount;
        }

        public void Pay(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Cannot pay a negative amount");
            }

            if (!CanPay(amount))
            {
                throw new ArgumentException("Attempted to pay out more than the current balance");
            }

            Balance -= amount;
        }

        public bool CanPay(int amount)
        {
            return Balance >= amount;
        }

        public override string ToString()
        {
            return String.Format("{0:C}", Balance);
        }
    }
}
