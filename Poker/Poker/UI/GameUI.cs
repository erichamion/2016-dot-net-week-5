using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.UI
{
    public class GameUI
    {
        private const ConsoleColor ACTIVE_PLAYER_COLOR = ConsoleColor.White;
        private const ConsoleColor INACTIVE_PLAYER_COLOR = ConsoleColor.DarkGray;

        private String _playerName = null;

        public void Start()
        {
            do
            {
                CreateAndRunGame();
                Console.Write("Continue ('y' for yes, anything else for no)?");
            } while (Console.ReadKey(true).KeyChar.ToString().ToLower().Equals("y"));
        }

        public void CreateAndRunGame()
        {
            Game.Game game = GetGameParametersAndCreateGame();

            Game.GameState gameState;            
            do
            {
                List<Game.PlayerState> playerStates;
                String actionTaken;
                gameState = game.AdvanceRound(out actionTaken, out playerStates);
                PrintGameState(game, gameState, actionTaken, playerStates);
                
            } while (gameState != Game.GameState.OVER);

        }

        private Game.Game GetGameParametersAndCreateGame()
        {
            Console.Clear();

            String playerName = GetPlayerName();
            int playerCount = GetPlayerCount();
            int ante = GetAnte();
            int walletSize = GetWalletSize(ante);
            
            Game.Game result = new Game.Game(playerCount, playerName, ante, walletSize);
            Console.Clear();
            return result;
        }

        private int GetPlayerCount()
        {
            const int minPlayers = Game.Game.MIN_PLAYERS;
            const int maxPlayers = Game.Game.MAX_PLAYERS;

            int result = minPlayers - 1;
            bool success = false;
            do
            {
                Console.Write("How many players ({0}-{1})?\n> ", minPlayers, maxPlayers);
                String response = Console.ReadLine();
                if (int.TryParse(response, out result))
                {
                    if (result >= minPlayers && result <= maxPlayers)
                    {
                        success = true;
                    }
                }

                if (!success)
                {
                    Console.WriteLine("Please enter an integer within the specified range.");
                }
            } while (!success);

            return result;
        }

        private String GetPlayerName()
        {
            if (_playerName != null)
            {
                Console.WriteLine("Welcome back, {0}!\nPress a key to get started", _playerName);
                Console.ReadKey(true);
            }
            else
            {
                Console.Write("What is your name?\n> ");
                _playerName = Console.ReadLine();
            }
            return _playerName;
        }

        private int GetAnte()
        {
            int result = 0;
            bool success = false;
            do
            {
                Console.Write("What size ante, in whole dollars?\n> ");
                String response = Console.ReadLine();
                if (int.TryParse(response, out result))
                {
                    if (result >= 1)
                    {
                        success = true;
                    }
                }

                if (!success)
                {
                    Console.WriteLine("Please enter an integer greater than or equal to 1.");
                }
            } while (!success);

            return result;
        }

        private int GetWalletSize(int ante)
        {
            int result = 0;
            double minWalletSize = ante * Game.Game.MIN_WALLET_TO_ANTE_RATIO;
            bool success = false;
            do
            {
                Console.Write("How much money should each player start with, in whole dollars?\n> ");
                String response = Console.ReadLine();
                if (int.TryParse(response, out result))
                {
                    if (result >= minWalletSize)
                    {
                        success = true;
                    }
                }

                if (!success)
                {
                    Console.WriteLine("Please enter an integer at least {0} times the ante size ({1}).", 
                        Game.Game.MIN_WALLET_TO_ANTE_RATIO, ante);
                }
            } while (!success);

            return result;
        }

        private void PrintGameState(Game.Game game, Game.GameState gameState, String actionTaken, List<Game.PlayerState> playerStates)
        {
            
            // Show the action that was just taken
            Console.WriteLine("{0}:\n", actionTaken);

            // Show the player states
            ShowPlayerStates(game, playerStates);

            // Show the next action
            Console.WriteLine();
            Console.WriteLine("Press a key to {0}...", Game.Game.GetNextActionString(gameState));
            Console.ReadKey(true);
            Console.Clear();
        }

        private static void ShowPlayerStates(Game.Game game, List<Game.PlayerState> playerStates)
        {
            // Save the current console text color in order to restore it later
            ConsoleColor oldColor = Console.ForegroundColor;

            // Show the player states
            for (int i = 0; i < playerStates.Count; i++)
            {
                Game.PlayerState playerState = playerStates[i];
                Console.ForegroundColor = playerState.IsActive ? ACTIVE_PLAYER_COLOR : INACTIVE_PLAYER_COLOR;
                String dealerIndicator = playerState.IsDealer ? "(D)" : "   ";
                String winnerIndicator;
                if (game.Winner != null)
                {
                    winnerIndicator = (i == game.Winner) ? "WINNER! " : "        ";
                }
                else
                {
                    winnerIndicator = String.Empty;
                }

                String outStr = String.Format("{0}. {1} {2} {3} ({4})",
                    i + 1, winnerIndicator, dealerIndicator, playerState.Name, playerState.BalanceString);
                if (!String.IsNullOrEmpty(playerState.HandString))
                {
                    outStr += String.Format(": {0}", playerState.HandString);
                }

                Console.WriteLine(outStr);
            }

            // Restore original console text color
            Console.ForegroundColor = oldColor;
        }
    }
}
