using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PokerUWP
{
    class PlayerView
    {
        public RelativePanel Panel { get; }
        public TextBlock DealerTextBlock { get; }
        public TextBlock NameTextBlock { get; }
        public TextBlock BalanceTextBlock { get; }
        public TextBlock HandTextBlock { get; }

        public Windows.UI.Color BackgroundColor
        {
            set
            {
                // Based on
                // http://stackoverflow.com/questions/36077231/how-to-change-background-color-of-button-in-uwp-apps-in-c-sharp

                Panel.Background = new Windows.UI.Xaml.Media.SolidColorBrush(value);
            }
        }

        public Windows.UI.Color TextColor
        {
            set
            {
                DealerTextBlock.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(value);
                NameTextBlock.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(value);
                BalanceTextBlock.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(value);
                HandTextBlock.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(value);
            }
        }

        public PlayerView(RelativePanel panel, TextBlock dealerTextBlock, TextBlock nameTextBlock, TextBlock balanceTextBlock, TextBlock handTextBlock)
        {
            Panel = panel;
            DealerTextBlock = dealerTextBlock;
            NameTextBlock = nameTextBlock;
            BalanceTextBlock = balanceTextBlock;
            HandTextBlock = handTextBlock;
        }
    }
}
