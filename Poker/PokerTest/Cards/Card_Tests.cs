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
    public class Card_Tests
    {
        [TestMethod()]
        public void Card_Ctor_WithValidArguments_HasCorrectValue()
        {
            // Arrange variables
            Card card1, card2, card3;
            CardRanks rank1 = CardRanks.A;
            CardRanks rank2 = CardRanks._10;
            CardRanks rank3 = CardRanks._4;
            Suits suit1 = Suits.C;
            Suits suit2 = Suits.D;
            Suits suit3 = Suits.H;

            // Act
            card1 = new Card(rank1, suit1);
            card2 = new Card(rank2, suit2);
            card3 = new Card(rank3, suit3);

            // Assert
            Assert.AreEqual(rank1, card1.Rank);
            Assert.AreEqual(rank2, card2.Rank);
            Assert.AreEqual(rank3, card3.Rank);
            Assert.AreEqual(suit1, card1.Suit);
            Assert.AreEqual(suit2, card2.Suit);
            Assert.AreEqual(suit3, card3.Suit);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Card_Ctor_WithRankLessThan2_ThrowsArgumentOutOfRange()
        {
            // Arrange variables
            Card card;
            CardRanks rank = CardRanks._2 - 1;
            Suits suit = Suits.C;
            
            // Act
            card = new Card(rank, suit);
            
            // Assert handled by ExpectedException
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Card_Ctor_WithRankGreaterThanAce_ThrowsArgumentOutOfRange()
        {
            // Arrange variables
            Card card;
            CardRanks rank = CardRanks.A + 1;
            Suits suit = Suits.C;

            // Act
            card = new Card(rank, suit);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Card_Ctor_WithNegativeSuit_ThrowsArgumentOutOfRange()
        {
            // Arrange variables
            Card card;
            CardRanks rank = CardRanks._4;
            Suits suit = (Suits)(-1);

            // Act
            card = new Card(rank, suit);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Card_Ctor_WithSuitGreaterThan3_ThrowsArgumentOutOfRange()
        {
            // Arrange variables
            Card card;
            CardRanks rank = CardRanks._4;
            Suits suit = (Suits)(4);

            // Act
            card = new Card(rank, suit);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        public void IsFaceCard_WithNumericRanks_IsFalse()
        {
            // Arrange Variables
            CardRanks[] ranks =
            {
                CardRanks._2,
                CardRanks._3,
                CardRanks._4,
                CardRanks._5,
                CardRanks._6,
                CardRanks._7,
                CardRanks._8,
                CardRanks._9,
                CardRanks._10
            };
            Suits suit = Suits.H;
            List<Card> cards = new List<Card>();

            // Act
            foreach (CardRanks rank in ranks)
            {
                cards.Add(new Card(rank, suit));
            }

            // Assert
            Assert.IsFalse(cards.Any(x => x.IsFaceCard()));
        }

        [TestMethod()]
        public void IsFaceCard_WithFaceRanks_IsTrue()
        {
            // Arrange Variables
            CardRanks[] ranks =
            {
                CardRanks.J,
                CardRanks.Q,
                CardRanks.K,
                CardRanks.A
            };
            Suits suit = Suits.S;
            List<Card> cards = new List<Card>();

            // Act
            foreach (CardRanks rank in ranks)
            {
                cards.Add(new Card(rank, suit));
            }

            // Assert
            Assert.IsTrue(cards.All(x => x.IsFaceCard()));
        }

        [TestMethod()]
        public void CompareTo_UnequalRanks_IsPositiveOrNegative()
        {
            // This test is completely exhaustive, but takes significant 
            // memory and time if combined with multiple similar tests.
            // If n = (total number of suits) * (total number of ranks) = 52, 
            // memory is O(n) and time is O(n^2).
            // A small sampling would be more efficient, but less complete.

            // Arrange variables
            List<Card> cards = new List<Card>();
            Array suits = Enum.GetValues(typeof(Suits));
            int numberOfSuits = suits.Length;
            Card highCard, lowCard;

            // Act
            for (int rankAsInt = (int)CardRanks._2; rankAsInt <= (int)CardRanks.A; rankAsInt++)
            {
                foreach (Suits suit in suits)
                {
                    cards.Add(new Card((CardRanks)rankAsInt, suit));
                }
            }

            // Assert
            for (int i = 0; i < cards.Count; i += numberOfSuits)
            {
                highCard = cards[i];
                for (int j = 0; j < i; j++)
                {
                    lowCard = cards[j];
                    Assert.IsTrue(highCard.CompareTo(lowCard) > 0);
                    Assert.IsTrue(lowCard.CompareTo(highCard) < 0);
                }
            }
        }

        [TestMethod()]
        public void CompareTo_EqualRanks_IsZero()
        {
            // This test is completely exhaustive, but takes significant 
            // memory and time if combined with multiple similar tests.
            // A small sampling would be more efficient, but less complete.

            // Arrange variables
            List<Card> cards = new List<Card>();
            Array suits = Enum.GetValues(typeof(Suits));
            int numberOfSuits = suits.Length;
            Card card1, card2;
            int rangeStart, rangeEnd;

            // Act
            for (int rankAsInt = (int)CardRanks._2; rankAsInt <= (int)CardRanks.A; rankAsInt++)
            {
                foreach (Suits suit in suits)
                {
                    cards.Add(new Card((CardRanks)rankAsInt, suit));
                }
            }

            // Assert
            for (rangeStart = 0; rangeStart < cards.Count; rangeStart += numberOfSuits)
            {
                rangeEnd = rangeStart + numberOfSuits;
                for (int i = rangeStart; i < rangeEnd; i++)
                {
                    card1 = cards[i];
                    for (int j = rangeStart; j < rangeEnd; j++)
                    {
                        card2 = cards[j];
                        Assert.AreEqual(0, card1.CompareTo(card2));
                    }
                }
            }
        }

        [TestMethod()]
        public void ToString_ValidCall_GivesCorrectString()
        {
            // Arrange variables
            Card card1, card2, card3;
            CardRanks rank1 = CardRanks.A;
            CardRanks rank2 = CardRanks._10;
            CardRanks rank3 = CardRanks._4;
            Suits suit1 = Suits.C;
            Suits suit2 = Suits.D;
            Suits suit3 = Suits.H;
            String expectedString1 = "AC";
            String expectedString2 = "10D";
            String expectedString3 = "4H";

            // Act
            card1 = new Card(rank1, suit1);
            card2 = new Card(rank2, suit2);
            card3 = new Card(rank3, suit3);

            // Assert
            Assert.IsTrue(card1.ToString().Equals(expectedString1));
            Assert.IsTrue(card2.ToString().Equals(expectedString2));
            Assert.IsTrue(card3.ToString().Equals(expectedString3));

        }
    }
}