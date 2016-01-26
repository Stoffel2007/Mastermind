using System;
using System.Windows;
using System.Windows.Controls;

namespace Mastermind
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void drawGameboards()
        {
            RowDefinition mastermindow = new RowDefinition();
        }

        private void changeGameboard(object sender, EventArgs e)
        {
            if (gameboard_mastermind.Visibility == Visibility.Visible)
            {
                gameboard_mastermind.Visibility = Visibility.Collapsed;
                Visibility = Visibility.Visible;
            }
            else
            {
                gameboard_mastermind.Visibility = Visibility.Visible;
                gameboard_super_mastermind.Visibility = Visibility.Collapsed;
            }
        }
    }
}