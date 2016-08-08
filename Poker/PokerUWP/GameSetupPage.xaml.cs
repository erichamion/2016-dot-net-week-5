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
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PokerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            Poker.Game.Game game = null;
            try
            {
                String playerName = GetPlayerName();
                int playerCount = GetPlayerCount();
                int ante = GetAnte();
                int walletSize = GetWalletSize(ante);

                game = new Poker.Game.Game(playerCount, playerName, ante, walletSize);
            }
            catch (Exception ex)
            {
                // I referred here for how to show a message box:
                // http://stackoverflow.com/questions/22909329/universal-apps-messagebox-the-name-messagebox-does-not-exist-in-the-current
                var dialog = new MessageDialog(ex.Message, "Error");
                await dialog.ShowAsync();
            }

            if (game != null)
            {
                // I used this reference for navigating between two pages:
                // https://msdn.microsoft.com/windows/uwp/layout/peer-to-peer-navigation-between-two-pages
                Frame.Navigate(typeof(GamePlayPage), game);
            }

        }

        private String GetPlayerName()
        {
            return nameTextBox.Text;
        }

        private int GetPlayerCount()
        {
            const int minPlayers = Poker.Game.Game.MIN_PLAYERS;
            const int maxPlayers = Poker.Game.Game.MAX_PLAYERS;

            int result;
            bool success1 = false;
            bool success2 = false;
            String errorReason = "";
            var text = playerCountTextBox.Text;
            success1 = int.TryParse(text, out result);

            if (success1)
            {
                success2 = (result >= minPlayers && result <= maxPlayers);
            }
            else
            {
                errorReason = String.Format("Not numeric");
            }

            if (success1 && !success2)
            {
                // Numeric, but out of range
                errorReason = String.Format("Must be in the range {0}-{1}", minPlayers, maxPlayers);
            }

            if (success1 && success2)
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException(string.Format("'{0}' is not a valid number of players: {1}", text, errorReason));
            }
        }

        private int GetAnte()
        {
            int result;
            bool success1 = false;
            bool success2 = false;
            String errorReason = "";
            var text = anteTextBox.Text;
            success1 = int.TryParse(text, out result);

            if (success1)
            {
                success2 = result >= 1;
            }
            else
            {
                errorReason = String.Format("Not numeric");
            }

            if (success1 && !success2)
            {
                // Numeric, but out of range
                errorReason = "Must be greater than or equal to 1";
            }

            if (success1 && success2)
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException(string.Format("'{0}' is not a valid ante amount: {1}", text, errorReason));
            }
        }

        private int GetWalletSize(int ante)
        {
            int result;
            bool success1 = false;
            bool success2 = false;
            double minSize = ante * Poker.Game.Game.MIN_WALLET_TO_ANTE_RATIO;
            String errorReason = "";
            var text = walletTextBox.Text;
            success1 = int.TryParse(text, out result);

            if (success1)
            {
                success2 = result >= minSize;
            }
            else
            {
                errorReason = String.Format("Not numeric");
            }

            if (success1 && !success2)
            {
                // Numeric, but out of range
                errorReason = String.Format("Must be at least {0} times the ante ({1})", 
                    Poker.Game.Game.MIN_WALLET_TO_ANTE_RATIO, ante);
            }

            if (success1 && success2)
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException(string.Format("'{0}' is not a valid wallet amount: {1}", text, errorReason));
            }
        }
    }
}
