using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poker.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Player.Tests
{
    [TestClass()]
    public class Player_Tests
    {
        [TestMethod()]
        public void TryBet_WithValidParameters_SucceedsAndSubtractsCash()
        {
            // Arrange variables
            int initialCash = 3;
            int betAmount1 = 2;
            int betAmount2 = 3;
            int expectedAmount1 = initialCash - betAmount1;
            int expectedAmount2 = initialCash - betAmount2;
            Player player1 = new Player("foo", initialCash);
            Player player2 = new Player("bar", initialCash);
            bool result1, result2;
            int remainingCash1, remainingCash2;

            // Act
            result1 = player1.TryBet(betAmount1);
            remainingCash1 = player1.Cash;
            result2 = player2.TryBet(betAmount2);
            remainingCash2 = player2.Cash;

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(expectedAmount1, remainingCash1);
            Assert.AreEqual(expectedAmount2, remainingCash2);
        }

        [TestMethod()]
        public void TryBet_GreaterThanBalance_FailsAndDoesNotSubtractCash()
        {
            // Arrange variables
            int initialCash = 3;
            int betAmount = 4;
            Player player = new Player("foo", initialCash);
            bool result;
            int remainingCash;

            // Act
            result = player.TryBet(betAmount);
            remainingCash = player.Cash;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(initialCash, remainingCash);
        }

        [TestMethod()]
        public void CollectWinnings_ValidParameters_AddsCash()
        {
            // Arrange variables
            int initialCash = 3;
            int awardAmount = 5;
            int expectedBalance = initialCash + awardAmount;
            Player player = new Player("foo", initialCash);
            int newBalance;

            // Act
            player.CollectWinnings(awardAmount);
            newBalance = player.Cash;

            // Assert
            Assert.AreEqual(expectedBalance, newBalance);
        }
    }
}