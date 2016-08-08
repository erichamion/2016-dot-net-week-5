using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Game
{
    public class Game
    {
        public const int MIN_PLAYERS = 3;
        public const int MAX_PLAYERS = 4;
        public const double MIN_WALLET_TO_ANTE_RATIO = 3.0;

        public GameState State { get; private set; } = GameState.NOT_STARTED;
        public int? Winner { get; private set; } = null;

        private List<Player.Player> _players;
        private int _ante;
        private Pot _pot = new Pot();
        private Cards.Deck _deck;
        private int _dealer = -1;

        public static String GetNextActionString(GameState state)
        {
            switch (state)
            {
                case GameState.NOT_STARTED:
                    return "start the game";

                case GameState.PRE_ANTE:
                    return "submit antes";

                case GameState.POST_ANTE:
                    return "deal";

                case GameState.PRE_SCORE:
                    return "compare hands";

                case GameState.POST_SCORE:
                    return "start next round";

                case GameState.OVER:
                    return "exit or start a new game";

                default:
                    return "";
            }
        }


        public Game(int playerCount, String humanPlayerName, int ante, int walletSize) : 
            this(playerCount, humanPlayerName, ante, walletSize, new Random()) { }

        public Game(int playerCount, String humanPlayerName, int ante, int walletSize, Random random)
        {
            if (playerCount < MIN_PLAYERS || playerCount > MAX_PLAYERS)
            {
                throw new ArgumentOutOfRangeException("playerCount", playerCount, "Invalid number of players, must be greater than 2 and less than 5");
            }
            if (ante < 1)
            {
                throw new ArgumentOutOfRangeException("ante", ante, "Ante must be positive and nonzero");
            }
            if ((double) walletSize / ante < MIN_WALLET_TO_ANTE_RATIO)
            {
                throw new ArgumentException(String.Format("walletSize must be at least {0} times ante", MIN_WALLET_TO_ANTE_RATIO));
            }


            _ante = ante;

            _players = new List<Player.Player>(playerCount);
            Player.Player humanPlayer = new Player.Player(humanPlayerName, walletSize);
            _players.Add(humanPlayer);
            for (int i = 1; i < playerCount; i++)
            {
                String playerName = String.Format("Player {0}", i + 1);
                _players.Add(new Player.Player(playerName, walletSize));
            }

            _deck = new Cards.Deck(random);
        }

        /// <summary>
        /// Performs the next portion of a round.
        /// </summary>
        /// <param name="newPlayerStates">out. Contains the player states after the activity is performed.</param>
        /// <returns>The new state</returns>
        public GameState AdvanceRound(out String actionTaken, out List<PlayerState> newPlayerStates)
        {
            GameState newState;
            switch (State)
            {
                case GameState.NOT_STARTED:
                    newState = StartRound(out actionTaken, out newPlayerStates);
                    break;

                case GameState.PRE_ANTE:
                    newState = DoAnte(out actionTaken, out newPlayerStates);
                    break;

                case GameState.POST_ANTE:
                    newState = Deal(out actionTaken, out newPlayerStates);
                    break;

                case GameState.PRE_SCORE:
                    newState = ScorePlayers(out actionTaken, out newPlayerStates);
                    break;

                case GameState.POST_SCORE:
                    newState = StartRound(out actionTaken, out newPlayerStates);
                    break;

                case GameState.OVER:
                default:
                    throw new InvalidOperationException("Cannot continue a game after it is over. Start a new game instead.");
            }
            State = newState;
            return newState;
        }

        private GameState StartRound(out String actionString, out List<PlayerState> playerStates)
        {
            _deck.Reshuffle();
            _dealer = FindDealer(_dealer);
            Winner = null;

            playerStates = GetPlayerStatesNoHands();
            actionString = "Starting round";
            return GameState.PRE_ANTE;
        }

        private GameState DoAnte(out String actionString, out List<PlayerState> playerStates)
        {
            foreach (Player.Player player in _players.Where(x => x.IsActive))
            {
                // TODO: Consider making Player.TryBet return the amount of the
                // bet if successful, or 0 if unsuccessful. That would simplify
                // this to "_pot.Add(player.TryBet(_ante));"
                if (player.TryBet(_ante))
                {
                    _pot.Add(_ante);
                }
            }

            // player.TryBet deactivates any players who can't meet the ante.
            // How many active players remain?
            var activePlayers = _players.Where(x => x.IsActive);
            if (activePlayers.Count() > 1)
            {
                // Multiple players remaining, continue normally
                return SucceedAnte(out actionString, out playerStates);
            }
            else if (activePlayers.Count() == 1)
            {
                // Exactly one remaining player. That player is the winner.
                Player.Player winner = activePlayers.Single();

                // Return the winner's money from the pot
                winner.CollectWinnings(_pot.PayOut());

                return DoGameOver(winner.Name, out actionString, out playerStates);
            }
            else
            {
                // Zero remaining players should not be possible
                throw new InvalidOperationException("No active players remaining! Where did the money go?");
            }            
        }

        private GameState SucceedAnte(out String actionString, out List<PlayerState> playerStates)
        {
            actionString = String.Format("Players submitted ante of {0:C} each, resulting in a {1:C} pot", _ante, _pot.Size);
            playerStates = GetPlayerStatesNoHands();
            return GameState.POST_ANTE;
        }

        private GameState DoGameOver(String winnerName, out string actionString, out List<PlayerState> playerStates)
        {
            actionString = String.Format("Game over. {0} wins", winnerName);
            playerStates = GetPlayerStatesNoHands();
            return GameState.OVER;
        }

        private GameState Deal(out String actionString, out List<PlayerState> playerStates)
        {
            foreach (Player.Player player in _players.Where(x => x.IsActive))
            {
                Cards.Card[] cards = new Cards.Card[5];
                for (int i = 0; i < 5; i++)
                {
                    cards[i] = _deck.NextCard;
                }

                player.CreateHand(cards);
            }

            playerStates = GetPlayerStates(true, false);
            actionString = String.Format("Dealt hand to each player (pot holds {0:C})", _pot.Size);
            return GameState.PRE_SCORE;
        }

        private GameState ScorePlayers(out String actionString, out List<PlayerState> playerStates)
        {
            Player.Player winningPlayer = _players.Where(x => x.IsActive).OrderBy(x => x.Hand).Last();
            Winner = _players.IndexOf(winningPlayer);
            winningPlayer.CollectWinnings(_pot.PayOut());

            playerStates = GetPlayerStates(true, true);
            actionString = String.Format("{0} wins the round", winningPlayer.Name);
            return GameState.POST_SCORE;
        }

        private List<PlayerState> GetPlayerStatesNoHands()
        {
            return GetPlayerStates(false, false, false);
        }

        private List<PlayerState> GetPlayerStates(bool showHumanHand, bool showAllHands)
        {
            return GetPlayerStates(true, showHumanHand, showAllHands);
        }

        private List<PlayerState> GetPlayerStates(bool playersHaveHands, bool showHumanHand, bool showAllHands)
        {
            List<PlayerState> result = new List<PlayerState>(_players.Count);
            for (int i = 0; i < _players.Count; i++)
            {
                Player.Player player = _players[i];
                bool isHuman = (i == 0);

                String bal = String.Format("{0:C}", player.Cash);
                String handStr;
                if (playersHaveHands && player.IsActive)
                {
                    bool showHand = (isHuman && showHumanHand) || showAllHands;
                    handStr = player.Hand.ToString(!showHand);
                }
                else
                {
                    handStr = String.Empty;
                }

                result.Add(new PlayerState(player.Name, bal, handStr, i == _dealer, isHuman, player.IsActive));
            }
            return result;
        }

        private int FindDealer(int oldDealer)
        {
            int dealer = oldDealer;
            do
            {
                if (++dealer >= _players.Count)
                {
                    dealer = 0;
                }
            } while (!_players[dealer].IsActive);

            return dealer;
        }

        


        
    }
}
