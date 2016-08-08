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
        public String Description { get; }
        public bool IsDealer { get; }
        public bool IsHuman { get; }
        public bool IsActive { get; }

        public PlayerState(String name, String desc, bool isDealer, bool isHuman, bool isActive)
        {
            Name = name;
            Description = desc;
            IsDealer = isDealer;
            IsHuman = isHuman;
            IsActive = isActive;
        }
    }
}
