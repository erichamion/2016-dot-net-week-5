using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Game
{
    public class Pot
    {
        public int Size { get; private set; }

        public Pot()
        {
            Size = 0;
        }

        public void Add(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Cannot add a negative amount to the pot");
            }
            Size += amount;
        }

        public int PayOut()
        {
            int result = Size;
            Size = 0;
            return result;
        }

        public override string ToString()
        {
            return String.Format("{0:C}", Size);
        }
    }
}
