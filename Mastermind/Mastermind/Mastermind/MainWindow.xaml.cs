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

            drawGameboards();
        }

        private void drawGameboards()
        {
            int size = 40;

            gameboard_mastermind.Children.Clear();
            gameboard_mastermind.RowDefinitions.Clear();
            gameboard_mastermind.ColumnDefinitions.Clear();
            gameboard_mastermind.Height = 13 * size;
            gameboard_mastermind.Width = 5 * size;

            gameboard_super_mastermind.Children.Clear();
            gameboard_super_mastermind.RowDefinitions.Clear();
            gameboard_super_mastermind.ColumnDefinitions.Clear();
            gameboard_super_mastermind.Height = 13 * size;
            gameboard_super_mastermind.Width = 6 * size;

            for (int i = 0; i < 13; i++)
            {
                RowDefinition newRow1 = new RowDefinition();
                newRow1.Height = new GridLength(size);
                gameboard_mastermind.RowDefinitions.Add(newRow1);
                
                RowDefinition newRow2 = new RowDefinition();
                newRow2.Height = new GridLength(size);
                gameboard_super_mastermind.RowDefinitions.Add(newRow2);
            }

            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(size);
                gameboard_mastermind.ColumnDefinitions.Add(newCol);
            }

            for (int i = 0; i < 6; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(size);
                gameboard_super_mastermind.ColumnDefinitions.Add(newCol);
            }
        }

        private void changeGameboard(object sender, EventArgs e)
        {
            ComboBoxItem senderBox = sender as ComboBoxItem;

            if (senderBox.Tag.ToString() == "1")
            {
                gameboard_mastermind.Visibility = Visibility.Visible;
                gameboard_super_mastermind.Visibility = Visibility.Collapsed;
            }
            else
            {
                gameboard_mastermind.Visibility = Visibility.Collapsed;
                gameboard_super_mastermind.Visibility = Visibility.Visible;
            }
        }
    }
}