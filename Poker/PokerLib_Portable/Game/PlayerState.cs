using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Game
{
    /// <summary>
    /// This is more closely tied to the Game class than the Player
    /// class. Describes the player's state in the game, without
    /// reference to Player internals
    /// </summary>
    public class PlayerState
    {
        public String Name { get; }
        public String BalanceString { get; }
        public String HandString { get; }
        public bool IsDealer { get; }
        public bool IsHuman { get; }
        public bool IsActive { get; }

        public PlayerState(String name, String balanceStr, String handStr, bool isDealer, bool isHuman, bool isActive)
        {
            Name = name;
            BalanceString = balanceStr;
            HandString = handStr;
            IsDealer = isDealer;
            IsHuman = isHuman;
            IsActive = isActive;
        }
    }
}
