using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PokerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePlayPage : Page
    {
        // The compiler won't allow these to be const, but these are effectively
        // constant.
        private Windows.UI.Color BG_COLOR_DEFAULT { get; } = Windows.UI.Colors.White;
        private Windows.UI.Color TEXT_COLOR_DEFAULT { get; } = Windows.UI.Colors.Black;
        private Windows.UI.Color BG_COLOR_WINNER { get; } = Windows.UI.Colors.DarkGreen;
        private Windows.UI.Color TEXT_COLOR_WINNER { get; } = Windows.UI.Colors.White;
        private Windows.UI.Color BG_COLOR_INACTIVE { get; } = Windows.UI.Colors.Silver;
        private Windows.UI.Color TEXT_COLOR_INACTIVE { get; } = Windows.UI.Colors.DarkGray;


        private Poker.Game.Game _game;
        private List<PlayerView> _playerViews = null;

        public GamePlayPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // I referred to this reference for passing information between pages:
            // https://msdn.microsoft.com/windows/uwp/layout/peer-to-peer-navigation-between-two-pages

            // Throws new InvalidCastException if a Game was not supplied.
            _game = (Poker.Game.Game)e.Parameter;

            base.OnNavigatedTo(e);
            DoGameRound();
        }

        private void actionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_game.State != Poker.Game.GameState.OVER)
            {
                DoGameRound();
            }
            else
            {
                Frame.GoBack();
            }
        }

        private void DoGameRound()
        {
            List<Poker.Game.PlayerState> playerStates;
            String actionTaken;
            Poker.Game.GameState gameState = _game.AdvanceRound(out actionTaken, out playerStates);
            UpdateUi(gameState, actionTaken, playerStates);
        }

        private void UpdateUi(Poker.Game.GameState gameState, String actionTaken, List<Poker.Game.PlayerState> playerStates)
        {
            if (_playerViews == null)
            {
                InitializePlayerViews(playerStates.Count);
            }

            UpdateControlPanel(gameState, actionTaken);
            UpdatePlayerPanels(playerStates);
        }

        private void InitializePlayerViews(int numPlayers)
        {
            _playerViews = new List<PlayerView>();

            // Player 1, always the same (bottom)
            _playerViews.Add(new PlayerView(playerPanel1, dealerText1, playerNameText1, playerBalanceText1, playerHandText1));

            // Player 2, always the same (left)
            _playerViews.Add(new PlayerView(playerPanel2, dealerText2, playerNameText2, playerBalanceText2, playerHandText2));

            // Player 3 depends on the number of players. If there are only 3 players,
            // skip the top and go to the right.
            if (numPlayers > 3)
            {
                // (top)
                _playerViews.Add(new PlayerView(playerPanel3, dealerText3, playerNameText3, playerBalanceText3, playerHandText3));

                // There are three rows. Set min height accordingly.
                mainGrid.MinHeight = 3 * playerPanel1.MinHeight;
                
            }
            else
            {
                // Hide the top panel if we don't use it
                playerPanel3.Visibility = Visibility.Collapsed;

                // There are only 2 occupied rows. Set the first row to 0 height, and set 
                // overall min height accordingly.
                var firstRowDef = mainGrid.RowDefinitions[0];
                firstRowDef.Height = new GridLength(0);
                firstRowDef.MinHeight = 0;
                mainGrid.MinHeight = 2 * playerPanel1.MinHeight;
            }

            // Player 3 or 4 depending on how many players we have (right)
            _playerViews.Add(new PlayerView(playerPanel4, dealerText4, playerNameText4, playerBalanceText4, playerHandText4));

            
        }

        private void UpdateControlPanel(Poker.Game.GameState gameState, String actionTaken)
        {
            lastActionText.Text = actionTaken;
            actionButton.Content = Poker.Game.Game.GetNextActionString(gameState).ToUpper();
        }

        private void UpdatePlayerPanels(List<Poker.Game.PlayerState> playerStates)
        {
            for (int i = 0; i < playerStates.Count; i++)
            {
                bool isWinner = (_game.Winner ?? -1) == i;
                UpdateSinglePlayerView(_playerViews[i], playerStates[i], isWinner);
            }
        }

        private void UpdateSinglePlayerView(PlayerView view, Poker.Game.PlayerState playerState, bool isWinner)
        {
            Windows.UI.Color bg, fg;
            if (playerState.IsActive)
            {
                if (isWinner)
                {
                    bg = BG_COLOR_WINNER;
                    fg = TEXT_COLOR_WINNER;
                } else
                {
                    bg = BG_COLOR_DEFAULT;
                    fg = TEXT_COLOR_DEFAULT;
                }
            }
            else
            {
                bg = BG_COLOR_INACTIVE;
                fg = TEXT_COLOR_INACTIVE;
            }

            view.BackgroundColor = bg;
            view.TextColor = fg;

            view.DealerTextBlock.Text = playerState.IsDealer ? "Dealer:" : String.Empty;
            view.NameTextBlock.Text = playerState.Name;
            view.BalanceTextBlock.Text = playerState.BalanceString;
            view.HandTextBlock.Text = playerState.HandString;

        }

        
    }
    
}
