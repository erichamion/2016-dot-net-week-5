using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Cards
{
    // This will likely only be used for unit tests
    public class CardEqualityComparer : IEqualityComparer<Card>
    {
        public bool Equals(Card x, Card y)
        {
            return x.Rank == y.Rank && x.Suit == y.Suit;
        }

        public int GetHashCode(Card card)
        {
            int result = 13;
            result = 7 * result + card.Rank.GetHashCode();
            result = 7 * result + card.Suit.GetHashCode();
            return result;
        }
    }
}
