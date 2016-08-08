using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Cards
{
    public class Card : IComparable<Card>
    {
        public CardRanks Rank { get; }
        public Suits Suit { get; }

        public Card (CardRanks rank, Suits suit)
        {
            if (!Enum.IsDefined(typeof(CardRanks), rank))
            {
                throw new ArgumentOutOfRangeException(String.Format("{0} is not a defined CardRank", rank));
            }
            if (!Enum.IsDefined(typeof(Suits), suit))
            {
                throw new ArgumentOutOfRangeException(String.Format("{0} is not a defined Suit", rank));
            }
            Rank = rank;
            Suit = suit;
        }

        public bool IsFaceCard()
        {
            return Rank >= CardRanks.J;
        }

        public int CompareTo(Card other)
        {
            return this.Rank - other.Rank;
        }

        

        public override string ToString()
        {
            return (Rank.ToString() + Suit.ToString()).Trim('_');
            
        }
    }
}
