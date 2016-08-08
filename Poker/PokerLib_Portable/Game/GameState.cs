using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Game
{
    public enum GameState
    {
        NOT_STARTED,
        PRE_ANTE,
        POST_ANTE,
        PRE_SCORE,
        POST_SCORE,
        //BETWEEN_ROUNDS,
        OVER
    }
}
