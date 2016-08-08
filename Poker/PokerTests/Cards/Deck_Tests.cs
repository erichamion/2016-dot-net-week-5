using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Cards.Tests
{
    [TestClass()]
    public class Deck_Tests
    {
        [TestMethod()]
        public void NextCard_52Calls_DistinctResults()
        {
            // Arrange variables
            Deck deck = new Deck();
            CardEqualityComparer comparer = new CardEqualityComparer();
            int distinctCount;
            List<Card> cards;
            
            // Act
            cards = CallNextCard52Times(deck);
            distinctCount = cards.Distinct(comparer).Count();

            // Assert
            Assert.AreEqual(52, distinctCount);
        }

        [TestMethod()]
        public void NextCard_52Calls_ValidCards()
        {
            // Arrange variables
            Deck deck = new Deck();
            List<Card> cards;

            // Act
            cards = CallNextCard52Times(deck);
            
            // Assert
            Assert.IsTrue(cards.All(x => Enum.IsDefined(typeof(CardRanks), x.Rank) && Enum.IsDefined(typeof(Suits), x.Suit)));
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NextCard_53Calls_ThrowsInvalidOperationException()
        {
            // Arrange variables
            Deck deck = new Deck();
            List<Card> cards;
            Card dummy;

            // Act
            cards = CallNextCard52Times(deck);
            dummy = deck.NextCard;

            // Assert handled by ExpectedException;
        }

        [TestMethod()]
        public void Reshuffle_ValidCall_CanCallNextCard52MoreTimesWithDistinctCards()
        {
            // Arrange variables
            Deck deck = new Deck();
            CardEqualityComparer comparer = new CardEqualityComparer();
            int distinctCount;
            List<Card> cards;

            // Act
            cards = CallNextCardMultipleTimes(deck, 20);
            deck.Reshuffle();
            cards = CallNextCard52Times(deck);
            distinctCount = cards.Distinct(comparer).Count();

            // Assert
            Assert.AreEqual(52, distinctCount);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reshuffle_ThenCallNextCard53MoreTimes_ThrowsInvalidOperationException()
        {
            // Arrange variables
            Deck deck = new Deck();
            List<Card> cards;
            Card dummy;

            // Act
            cards = CallNextCardMultipleTimes(deck, 20);
            deck.Reshuffle();
            cards = CallNextCard52Times(deck);
            dummy = deck.NextCard;

            // Assert handled by ExpectedException;
        }

        [TestMethod()]
        public void Reshuffle_MultipleCalls_DifferentCardOrderEachCall()
        {
            // Arrange variables
            Deck deck = new Deck();
            CardEqualityComparer comparer = new CardEqualityComparer();
            List<Card>[] cardses = new List<Card>[4];

            // Act
            cardses[0] = CallNextCard52Times(deck);
            deck.Reshuffle();
            cardses[1] = CallNextCard52Times(deck);
            deck.Reshuffle();
            cardses[2] = CallNextCard52Times(deck);
            deck.Reshuffle();
            cardses[3] = CallNextCard52Times(deck);

            // Assert
            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    Assert.IsFalse(cardses[i].SequenceEqual(cardses[j], comparer));
                }
            }
        }


        private static List<Card> CallNextCard52Times(Deck deck)
        {
            return CallNextCardMultipleTimes(deck, 52);
        }

        private static List<Card> CallNextCardMultipleTimes(Deck deck, int n)
        {
            List<Card> cards = new List<Card>();

            // Act
            for (int i = 0; i < n; i++)
            {
                cards.Add(deck.NextCard);
            }
            return cards;
        }
    }
}