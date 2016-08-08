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
    public class Wallet_Tests
    {
        [TestMethod()]
        public void Wallet_Ctor_HasGivenBalance()
        {
            // Arrange variables
            Wallet wallet;
            int expectedBalance = 27;

            // Act
            wallet = new Wallet(expectedBalance);

            // Assert
            Assert.AreEqual(expectedBalance, wallet.Balance);
        }

        [TestMethod()]
        public void AddBalance_WithValidParameters_AddsBalance()
        {
            // Arrange variables
            int initialBalance = 37;
            int addAmount = 25;
            int expectedBalance = initialBalance + addAmount;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.AddBalance(addAmount);

            // Assert
            Assert.AreEqual(expectedBalance, wallet.Balance);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddBalance_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange variables
            int initialBalance = 37;
            int addAmount = -25;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.AddBalance(addAmount);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        public void Pay_WithValidParameters_SubtractsBalance()
        {
            // Arrange variables
            int initialBalance = 37;
            int subtractAmount = 25;
            int expectedBalance = initialBalance - subtractAmount;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.Pay(subtractAmount);

            // Assert
            Assert.AreEqual(expectedBalance, wallet.Balance);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Pay_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange variables
            int initialBalance = 37;
            int subtractAmount = -25;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.Pay(subtractAmount);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Pay_GreaterThanBalance_ThrowsArgumentException()
        {
            // Arrange variables
            int initialBalance = 37;
            int subtractAmount = 55;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.Pay(subtractAmount);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        public void CanPay_LessThanBalance_IsTrue()
        {
            // Arrange variables
            int initialBalance = 37;
            int payAmount = 37;
            Wallet wallet = new Wallet(initialBalance);
            bool canPay;

            // Act
            canPay = wallet.CanPay(payAmount);

            // Assert
            Assert.IsTrue(canPay);
        }

        [TestMethod()]
        public void CanPay_GreaterThanBalance_IsFalse()
        {
            // Arrange variables
            int initialBalance = 37;
            int payAmount = 38;
            Wallet wallet = new Wallet(initialBalance);
            bool canPay;

            // Act
            canPay = wallet.CanPay(payAmount);

            // Assert
            Assert.IsFalse(canPay);
        }

        [TestMethod()]
        public void ToString_ValidCall_GivesCorrectString()
        {
            // Arrange variables
            int initialBalance = 37;
            String expectedString = "$37.00";
            Wallet wallet = new Wallet(initialBalance);
            String resultString;

            // Act
            resultString = wallet.ToString();

            // Assert
            Assert.IsTrue(resultString.Equals(expectedString));
        }
    }
}