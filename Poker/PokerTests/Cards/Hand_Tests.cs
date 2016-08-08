using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poker.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Cards.Tests
{
    [TestClass()]
    public class Hand_Tests
    {


        [TestMethod()]
        public void Hand_Ctor_WithValidArguments_ContainsCorrectCardsInAscendingOrder()
        {
            // Arrange variables
            Card[] cards =
            {
                new Card(CardRanks._10, Suits.C),
                new Card(CardRanks._3, Suits.D),
                new Card(CardRanks.A, Suits.H),
                new Card(CardRanks._6, Suits.S),
                new Card(CardRanks._2, Suits.C)
            };
            Hand hand;
            bool threwOutOfRange = false;

            // Act
            hand = new Hand(cards);
            try
            {
                Card c = hand[5];
            }
            catch (ArgumentOutOfRangeException)
            {
                threwOutOfRange = true;
            }

            // Assert
            Assert.IsTrue(threwOutOfRange);
            Assert.AreEqual(CardRanks._2, hand[0].Rank);
            Assert.AreEqual(Suits.C, hand[0].Suit);
            Assert.AreEqual(CardRanks._3, hand[1].Rank);
            Assert.AreEqual(Suits.D, hand[1].Suit);
            Assert.AreEqual(CardRanks._6, hand[2].Rank);
            Assert.AreEqual(Suits.S, hand[2].Suit);
            Assert.AreEqual(CardRanks._10, hand[3].Rank);
            Assert.AreEqual(Suits.C, hand[3].Suit);
            Assert.AreEqual(CardRanks.A, hand[4].Rank);
            Assert.AreEqual(Suits.H, hand[4].Suit);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Hand_Ctor_WithTooFewArguments_ThrowsArgumentException()
        {
            // Arrange variables
            Card[] cards =
            {
                new Card(CardRanks._10, Suits.C),
                new Card(CardRanks._3, Suits.D),
                new Card(CardRanks.A, Suits.H),
                new Card(CardRanks._6, Suits.S)
            };
            Hand hand;

            // Act
            hand = new Hand(cards);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Hand_Ctor_WithTooManyArguments_ThrowsArgumentException()
        {
            // Arrange variables
            Card[] cards =
            {
                new Card(CardRanks._10, Suits.C),
                new Card(CardRanks._3, Suits.D),
                new Card(CardRanks.A, Suits.H),
                new Card(CardRanks._6, Suits.S),
                new Card(CardRanks._2, Suits.C),
                new Card(CardRanks.A, Suits.C)
            };
            Hand hand;

            // Act
            hand = new Hand(cards);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        public void HighestOverallCard_Get_IsCorrect()
        {
            // Arrange variables
            Card[] cards =
            {
                new Card(CardRanks._10, Suits.C),
                new Card(CardRanks._3, Suits.D),
                new Card(CardRanks.A, Suits.H),
                new Card(CardRanks._6, Suits.S),
                new Card(CardRanks._2, Suits.C)
            };
            Hand hand;
            Card highestCard;

            // Act
            hand = new Hand(cards);
            highestCard = hand.HighestOverallCard;

            // Assert
            Assert.AreEqual(CardRanks.A, highestCard.Rank);
            Assert.AreEqual(Suits.H, highestCard.Suit);
        }

        [TestMethod()]
        public void Rank_Single_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._10, Suits.H),
                new Card(CardRanks._5, Suits.S),
                new Card(CardRanks._7, Suits.C)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.SINGLE_CARD, rank);
        }

        [TestMethod()]
        public void Rank_Pair_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._10, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks._7, Suits.C)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.PAIR, rank);
        }

        [TestMethod()]
        public void Rank_TwoPair_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._10, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks._2, Suits.C)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.TWO_PAIR, rank);
        }

        [TestMethod()]
        public void Rank_ThreeOfAKind_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._10, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks.Q, Suits.C)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.THREE_OF_A_KIND, rank);
        }

        [TestMethod()]
        public void Rank_Straight_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks._7, Suits.C),
                new Card(CardRanks._6, Suits.D),
                new Card(CardRanks._4, Suits.H),
                new Card(CardRanks._8, Suits.S),
                new Card(CardRanks._5, Suits.C)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.STRAIGHT, rank);
        }

        [TestMethod()]
        public void Rank_Flush_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.C),
                new Card(CardRanks._10, Suits.C),
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks.Q, Suits.C)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.FLUSH, rank);
        }

        [TestMethod()]
        public void Rank_FullHouseLowPair_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._2, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks.Q, Suits.D)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.FULL_HOUSE, rank);
        }

        [TestMethod()]
        public void Rank_FullHouseHighPair_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._2, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks._2, Suits.C)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.FULL_HOUSE, rank);
        }

        [TestMethod()]
        public void Rank_FourOfAKind_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks.Q, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks.Q, Suits.D)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.FOUR_OF_A_KIND, rank);
        }

        [TestMethod()]
        public void Rank_StraightFlush_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks._7, Suits.D),
                new Card(CardRanks._6, Suits.D),
                new Card(CardRanks._4, Suits.D),
                new Card(CardRanks._8, Suits.D),
                new Card(CardRanks._5, Suits.D)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.STRAIGHT_FLUSH, rank);
        }

        [TestMethod()]
        public void Rank_RoyalFlush_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.D),
                new Card(CardRanks.A, Suits.D),
                new Card(CardRanks._10, Suits.D),
                new Card(CardRanks.J, Suits.D),
                new Card(CardRanks.K, Suits.D)
                );
            HandRanks rank;

            // Act
            rank = hand.Rank;

            // Assert
            Assert.AreEqual(HandRanks.ROYAL_FLUSH, rank);
        }


        [TestMethod()]
        public void HighestRankCard_Single_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._10, Suits.H),
                new Card(CardRanks._5, Suits.S),
                new Card(CardRanks._7, Suits.C)
                );
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(CardRanks.Q, cardRank);
            Assert.AreEqual(Suits.C, cardSuit);
        }

        [TestMethod()]
        public void HighestRankCard_Pair_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._10, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks.A, Suits.C)
                );
            CardRanks expectedRank = CardRanks.Q;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.C, Suits.S });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;
            
            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        [TestMethod()]
        public void HighestRankCard_TwoPair_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks.K, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks._2, Suits.C)
                );
            CardRanks expectedRank = CardRanks.Q;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.C, Suits.S });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        [TestMethod()]
        public void HighestRankCard_ThreeOfAKind_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks.K, Suits.D),
                new Card(CardRanks._10, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks.Q, Suits.D)
                );
            CardRanks expectedRank = CardRanks.Q;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.C, Suits.S, Suits.D });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        [TestMethod()]
        public void HighestRankCard_Straight_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks._7, Suits.C),
                new Card(CardRanks._6, Suits.D),
                new Card(CardRanks._4, Suits.H),
                new Card(CardRanks._8, Suits.S),
                new Card(CardRanks._5, Suits.C)
                );
            CardRanks expectedRank = CardRanks._8;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.S });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        [TestMethod()]
        public void HighestRankCard_Flush_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks.A, Suits.C),
                new Card(CardRanks._10, Suits.C),
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks.Q, Suits.C)
                );
            CardRanks expectedRank = CardRanks.A;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.C });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        [TestMethod()]
        public void HighestRankCard_FullHouseLowPair_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._2, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks.Q, Suits.D)
                );
            CardRanks expectedRank = CardRanks.Q;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.C, Suits.S, Suits.D });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        [TestMethod()]
        public void HighestRankCard_FullHouseHighPair_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._2, Suits.H),
                new Card(CardRanks._2, Suits.S),
                new Card(CardRanks.Q, Suits.D)
                );
            CardRanks expectedRank = CardRanks.Q;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.C, Suits.D });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        [TestMethod()]
        public void HighestRankCard_FourOfAKind_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks.A, Suits.D),
                new Card(CardRanks.Q, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks.Q, Suits.D)
                );
            CardRanks expectedRank = CardRanks.Q;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.C, Suits.S, Suits.H, Suits.D });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        [TestMethod()]
        public void HighestRankCard_StraightFlush_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks._7, Suits.D),
                new Card(CardRanks._6, Suits.D),
                new Card(CardRanks._4, Suits.D),
                new Card(CardRanks._8, Suits.D),
                new Card(CardRanks._5, Suits.D)
                );
            CardRanks expectedRank = CardRanks._8;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.D });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        [TestMethod()]
        public void HighestRankCard_RoyalFlush_IsCorrect()
        {
            // Arrange variables
            Hand hand = new Hand(
                new Card(CardRanks.Q, Suits.D),
                new Card(CardRanks.A, Suits.D),
                new Card(CardRanks._10, Suits.D),
                new Card(CardRanks.J, Suits.D),
                new Card(CardRanks.K, Suits.D)
                );
            CardRanks expectedRank = CardRanks.A;
            List<Suits> expectedSuits = new List<Suits>(new Suits[] { Suits.D });
            Card bestCard;
            CardRanks cardRank;
            Suits cardSuit;

            // Act
            bestCard = hand.HighestRankCard;
            cardRank = bestCard.Rank;
            cardSuit = bestCard.Suit;

            // Assert
            Assert.AreEqual(expectedRank, cardRank);
            Assert.IsTrue(expectedSuits.Contains(cardSuit));
        }

        
        [TestMethod()]
        public void CompareScores_DifferentHandRanks_FavorsBetterHand()
        {
            // Arrange variables
            Hand hand1 = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._10, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks._2, Suits.C)
                );
            Hand hand2 = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._2, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks.Q, Suits.D)
                );
            int cmp1, cmp2;

            // Act
            cmp1 = hand1.CompareTo(hand2);
            cmp2 = hand2.CompareTo(hand1);

            // Assert
            Assert.IsTrue(cmp1 < 0);
            Assert.IsTrue(cmp2 > 0);
        }

        [TestMethod()]
        public void CompareScores_SameHandRanks_FavorsHighestRankedCard()
        {
            // Arrange variables
            Hand hand1 = new Hand(
                new Card(CardRanks.J, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._2, Suits.H),
                new Card(CardRanks.J, Suits.S),
                new Card(CardRanks.A, Suits.D)
                
                );
            Hand hand2 = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._4, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks._2, Suits.C)
                );
            int cmp1, cmp2;

            // Act
            cmp1 = hand1.CompareTo(hand2);
            cmp2 = hand2.CompareTo(hand1);

            // Assert
            Assert.IsTrue(cmp1 < 0);
            Assert.IsTrue(cmp2 > 0);
        }

        [TestMethod()]
        public void CompareScores_BothHandsArePairs_FavorsHighestRankedCard()
        {
            // Arrange variables
            Hand hand1 = new Hand(
                new Card(CardRanks.J, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._3, Suits.H),
                new Card(CardRanks._3, Suits.S),
                new Card(CardRanks.A, Suits.D)

                );
            Hand hand2 = new Hand(
                new Card(CardRanks._8, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._4, Suits.H),
                new Card(CardRanks._8, Suits.S),
                new Card(CardRanks._3, Suits.C)
                );
            int cmp1, cmp2;

            // Act
            cmp1 = hand1.CompareTo(hand2);
            cmp2 = hand2.CompareTo(hand1);

            // Assert
            Assert.IsTrue(cmp1 < 0);
            Assert.IsTrue(cmp2 > 0);
        }

        [TestMethod()]
        public void CompareScores_SameHandRanksAndRankedCards_FavorsHighestOverallCards()
        {
            // Arrange variables
            Hand hand1 = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._2, Suits.D),
                new Card(CardRanks._2, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks.A, Suits.D)

                );
            Hand hand2 = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._3, Suits.D),
                new Card(CardRanks.A, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks._3, Suits.C)
                );
            Hand hand3 = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._5, Suits.D),
                new Card(CardRanks._2, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks._6, Suits.D)

                );
            Hand hand4 = new Hand(
                new Card(CardRanks.Q, Suits.C),
                new Card(CardRanks._3, Suits.D),
                new Card(CardRanks.A, Suits.H),
                new Card(CardRanks.Q, Suits.S),
                new Card(CardRanks._4, Suits.C)
                );
            int cmp1, cmp2, cmp3, cmp4;

            // Act
            cmp1 = hand1.CompareTo(hand2);
            cmp2 = hand2.CompareTo(hand1);
            cmp3 = hand3.CompareTo(hand4);
            cmp4 = hand4.CompareTo(hand3);

            // Assert
            Assert.IsTrue(cmp1 < 0);
            Assert.IsTrue(cmp2 > 0);
            Assert.IsTrue(cmp3 < 0);
            Assert.IsTrue(cmp4 > 0);
        }
    }
}