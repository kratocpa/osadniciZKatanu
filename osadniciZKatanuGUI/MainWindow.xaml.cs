using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace osadniciZKatanuGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string picked_language = language_combo_box.SelectionBoxItem.ToString();

            int playerCount = 4;

            if (twoPlayerRadioButton.IsChecked == true)
            {
                playerCount = 2;
            }
            else if (threePlayerRadioButton.IsChecked == true)
            {
                playerCount = 3;
            }
            else if (fourPlayerRadioButton.IsChecked == true)
            {
                playerCount = 4;
            }

            bool randomGameBorder = false;
            switch (game_border_combo_box.SelectedIndex)
            {
                case 0: randomGameBorder = false;
                    break;
                case 1: randomGameBorder = true;
                    break;
            }

            var gameWindow = new GameWindow(playerCount, picked_language, randomGameBorder,
                redIsPlayerRadioButton.IsChecked == true,
                blueIsPlayerRadioButton.IsChecked == true,
                yellowIsPlayerRadioButton.IsChecked == true,
                whiteIsPlayerRadioButton.IsChecked == true,
                helpfullID.IsChecked == true,
                showMoves.IsChecked == true);

            gameWindow.Show();
            this.Close();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            redStackPanel.Visibility = Visibility.Visible;
            blueStackPanel.Visibility = Visibility.Visible;
            whiteStackPanel.Visibility = Visibility.Hidden;
            yellowStackPanel.Visibility = Visibility.Hidden;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            redStackPanel.Visibility = Visibility.Visible;
            blueStackPanel.Visibility = Visibility.Visible;
            whiteStackPanel.Visibility = Visibility.Visible;
            yellowStackPanel.Visibility = Visibility.Hidden;
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            redStackPanel.Visibility = Visibility.Visible;
            blueStackPanel.Visibility = Visibility.Visible;
            whiteStackPanel.Visibility = Visibility.Visible;
            yellowStackPanel.Visibility = Visibility.Visible;
        }

    }
}
