using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Cards
{
    /// <summary>
    /// Immutable Hand of exactly 5 cards, scored according to
    /// poker rules.
    /// </summary>
    /// <see cref="Hand"/>
    public class Hand : IComparable<Hand>
    {
        


        public Card HighestRankCard { get { return Score.HighestRankCard; } }
        public Card HighestOverallCard { get { return Cards.Last(); } }
        public HandRanks Rank { get { return Score.Rank; } }
        public Card this[int idx] { get { return Cards[idx]; } }

        private List<Card> Cards { get; } = new List<Card>();

        private PokerScore Score
        {
            get
            {
                if (_score == null)
                {
                    _score = ComputeScore();
                }
                return _score;
            }
        }

        private PokerScore _score = null;

        /// <summary>
        /// Create a hand with the given set of cards
        /// </summary>
        /// <param name="cards">An array of length 5, or 5 individual arguments, containing the cards
        /// to put into the hand</param>
        /// <exception cref="ArgumentException">Thrown if cards does not contain exactly 5 elements</exception>
        public Hand(params Card[] cards)
        {
            // Ensure valid arguments
            if (cards == null || cards.Length != 5)
            {
                String msg = (cards == null) ?
                    "No cards supplied" :
                    String.Format("A poker hand requires 5 cards, received {0}", cards.Length);
                throw new ArgumentException(msg);
            }

            // Add cards to hand
            foreach (Card card in cards)
            {
                Cards.Add(card);
            }

            // Sort cards
            Cards.Sort();
        }


        public int CompareTo(Hand other)
        {
            // Check whether one hand is simply better than the other
            int result = Score.CompareTo(other.Score);
            if (result == 0)
            {
                // Break ties by comparing each individual card, regardless
                // of whether the card contributes to the poker score.
                int idx = Cards.Count - 1;
                do
                {
                    result = this[idx].CompareTo(other[idx]);
                } while (result == 0 && --idx >= 0);
            }

            return result;
        }

        public override string ToString()
        {
            return ToString(false);
            
        }

        public String ToString(bool hidden)
        {
            String contents = hidden ? "Hand Hidden" : String.Join(" ", Cards.Select(x => x.ToString()));
            return String.Format("<{0}>", contents);
        }

        private PokerScore ComputeScore()
        {
            PokerScore result;

            // Highest rank is royal_flush. 
            // royal_flush = straight_flush + highest_card_is_ace
            // straight_flush = straight + flush
            // There's no sense in checking for royal_flush, then
            // separately checking again for the individual components.
            // Instead, check for the components first and save the results.
            bool isStraight = IsStraight();
            bool isFlush = IsFlush();
            bool isStraightFlush = isStraight && isFlush;

            if (isStraightFlush && HighestOverallCard.Rank == CardRanks.A)
            {
                // Royal flush. All 5 cards are used for the rank.
                result = new PokerScore(HandRanks.ROYAL_FLUSH, HighestOverallCard);
            }
            else if (isStraightFlush)
            {
                // Straight flush. All 5 cards are used for the rank.
                result = new PokerScore(HandRanks.STRAIGHT_FLUSH, HighestOverallCard);
            }
            else
            {
                var pair = GetMaxOfAKind();
                Card mostRepeatedCard = pair.Key;
                int maxOfAKind = pair.Value;

                if (maxOfAKind == 4)
                {
                    // Four of a kind. All the rank cards are equivalent to mostRepeatedCard.
                    result = new PokerScore(HandRanks.FOUR_OF_A_KIND, mostRepeatedCard);
                }
                else if (IsFullHouse(maxOfAKind, mostRepeatedCard))
                {
                    // Full house. All 5 cards are used for the rank.
                    result = new PokerScore(HandRanks.FULL_HOUSE, HighestOverallCard);
                }
                else if (isFlush)
                {
                    // Flush. All 5 cards are used for the rank.
                    result = new PokerScore(HandRanks.FLUSH, HighestOverallCard);
                }
                else if (isStraight)
                {
                    // Straight. All 5 cards are used for the rank.
                    result = new PokerScore(HandRanks.STRAIGHT, HighestOverallCard);
                }
                else if (maxOfAKind == 3)
                {
                    // Three of a kind. All the rank cards are equivalent to mostRepeatedCard.
                    result = new PokerScore(HandRanks.THREE_OF_A_KIND, mostRepeatedCard);
                }
                else if (maxOfAKind == 2)
                {
                    // Either two pairs, or a single pair.
                    if (IsTwoPair(maxOfAKind, mostRepeatedCard))
                    {
                        // Two pairs. mostRepeatedCard was the best repeated card, so it remains the
                        // best card used for the rank.
                        result = new PokerScore(HandRanks.TWO_PAIR, mostRepeatedCard);
                    }
                    else
                    {
                        // Pair. All the rank cards are equivalent to mostRepeatedCard.
                        result = new PokerScore(HandRanks.PAIR, mostRepeatedCard);
                    }
                }
                else
                {
                    // Just a single card.
                    result = new PokerScore(HandRanks.SINGLE_CARD, HighestOverallCard);
                }
            }

            return result;
        }

        private bool IsStraight()
        {
            // Cards are already sorted. Each one must have a rank
            // one higher than the previous.
            bool success = true;
            for (int i = 1; i < Cards.Count; i++)
            {
                if (Cards[i].Rank - Cards[i - 1].Rank != 1)
                {
                    success = false;
                    break;
                }
            }
            return success;
        }

        private bool IsFlush()
        {
            Suits firstSuit = Cards[0].Suit;
            return Cards.All(x => x.Suit == firstSuit);
        }

        private KeyValuePair<Card, int> GetMaxOfAKind()
        {
            Card bestCard = null;
            Card oldCard = null;
            int bestRun = 0;
            int currentRun = 0;

            foreach (Card currentCard in Cards)
            {
                if (oldCard != null && currentCard.CompareTo(oldCard) == 0)
                {
                    currentRun++;
                }
                else
                {
                    currentRun = 1;
                }

                // Cards are sorted by increasing rank, so if we tie the
                // best run then we must have a higher rank. Use >= operator,
                // not > operator.
                if (currentRun >= bestRun)
                {
                    bestRun = currentRun;
                    bestCard = currentCard;
                }

                oldCard = currentCard;
            }

            return new KeyValuePair<Card, int>(bestCard, bestRun);
        }

        private bool IsFullHouse(int maxOfAKind, Card repeatedCard)
        {
            // If there are three-of-a-kind, then either the three-of-a-kind are the 
            // first three cards, or they are the last three cards. Then either the 
            // first two or the last two must be both equal to each other and not 
            // equal to repeatedCard.
            bool result;
            if (maxOfAKind != 3)
            {
                // No way to get full house without 3-of-a-kind
                result = false;
            }
            else if (Cards[0].CompareTo(repeatedCard) == 0)
            {
                // First card is part of the three-of-a-kind. Test the last
                // two cards.
                result = (Cards[Cards.Count - 1].CompareTo(Cards[Cards.Count - 2]) == 0);
                
            }
            else
            {
                // First card is not part of the three-of-a-kind. Test the first
                // two cards.
                result = (Cards[0].CompareTo(Cards[1]) == 0);
            }

            return result;
        }

        private bool IsTwoPair(int maxOfAKind, Card repeatedCard)
        {
            // repeatedCard is the best possible set of repeated cards. If there
            // is another pair, it must be a lower value than repeatedCard, and it
            // must be somewhere in the first 3 cards. Also, the match we find must
            // be different from the existing repeatedCard.
            bool result = false;
            if (maxOfAKind != 2)
            {
                // We don't even have one pair, so we can't have two pairs!
                result = false;
            }
            else if (Cards[0].CompareTo(Cards[1]) * Cards[1].CompareTo(Cards[2]) == 0 && Cards[1].CompareTo(repeatedCard) != 0)
            {
                // If the product of the comparisons is zero, then at least one of
                // the comparisons must itself be zero. We already know we can't have
                // three cards with the same value (because maxOfAKind == 2), which
                // means exactly one of the comparisons is zero. We have a second pair.
                result = true;
            }
            // else do nothing, result remains false.

            return result;
        }

        

        private class PokerScore : IComparable<PokerScore>
        {
            public HandRanks Rank { get; }
            public Card HighestRankCard { get; }

            public PokerScore(HandRanks rank, Card highestRankCard)
            {
                Rank = rank;
                HighestRankCard = highestRankCard;
            }

            public int CompareTo(PokerScore other)
            {
                // Compare ranks first
                int cmp = Rank - other.Rank;
                if (cmp == 0)
                {
                    // If ranks are equal, compare the highest card used in
                    // the ranking.
                    cmp = HighestRankCard.CompareTo(other.HighestRankCard);
                }
                
                return cmp;
            }
        }
    }
}
