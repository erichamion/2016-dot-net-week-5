using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poker.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Game.Tests
{
    [TestClass()]
    public class Pot_Tests
    {
        [TestMethod()]
        public void Pot_Ctor_HasZeroAward()
        {
            // Arrange variables
            Pot pot;
            int expectedAward = 0;

            // Act
            pot = new Pot();

            // Assert
            Assert.AreEqual(expectedAward, pot.Size);
        }

        [TestMethod()]
        public void Add_ValidParameters_AddsToAward()
        {
            // Arrange variables
            Pot pot = new Pot();
            int firstAmount = 27;
            int secondAmount = 35;
            int expectedAward = firstAmount + secondAmount;

            // Act
            pot.Add(firstAmount);
            pot.Add(secondAmount);

            // Assert
            Assert.AreEqual(expectedAward, pot.Size);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Add_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange variables
            Pot pot = new Pot();
            int amount = -1;

            // Act
            pot.Add(amount);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        public void PayOut_ValidCall_ReturnsAwardAmount()
        {
            // Arrange variables
            Pot pot = new Pot();
            int expectedAward = 27;
            int actualAward;

            // Act
            pot.Add(expectedAward);
            actualAward = pot.PayOut();

            // Assert
            Assert.AreEqual(expectedAward, actualAward);
        }

        [TestMethod()]
        public void PayOut_ValidCall_ResetsAwardToZero()
        {
            // Arrange variables
            Pot pot = new Pot();
            int addAmount = 27;
            int newAward;

            // Act
            pot.Add(addAmount);
            pot.PayOut();
            newAward = pot.Size;

            // Assert
            Assert.AreEqual(0, newAward);
        }

        [TestMethod()]
        public void ToString_ValidCall_GivesCorrectString()
        {
            // Arrange variables
            Pot pot = new Pot();
            int addAmount = 27;
            String expectedString = "$27.00";
            String actualString;

            // Act
            pot.Add(addAmount);
            actualString = pot.ToString();

            // Assert
            Assert.IsTrue(actualString.Equals(expectedString));
        }
    }
}