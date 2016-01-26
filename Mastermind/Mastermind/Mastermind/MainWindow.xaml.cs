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
            
            gameboard_mastermind.Height = 13 * size;
            gameboard_mastermind.Width = 5 * size;
            
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

            // Mastermind
            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(size);
                gameboard_mastermind.ColumnDefinitions.Add(newCol);
            }

            // Super Mastermind
            for (int i = 0; i < 6; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(size);
                gameboard_super_mastermind.ColumnDefinitions.Add(newCol);
            }

            for (int i = 1; i < 13; i++)
            {
                // Mastermind
                Grid newGrid1 = new Grid();
                newGrid1.ShowGridLines = true;

                RowDefinition row1_1 = new RowDefinition();
                row1_1.Height = new GridLength(size / 2);
                newGrid1.RowDefinitions.Add(row1_1);

                RowDefinition row1_2 = new RowDefinition();
                row1_2.Height = new GridLength(size / 2);
                newGrid1.RowDefinitions.Add(row1_2);

                ColumnDefinition col1_1 = new ColumnDefinition();
                col1_1.Width = new GridLength(size / 2);
                newGrid1.ColumnDefinitions.Add(col1_1);

                ColumnDefinition col1_2 = new ColumnDefinition();
                col1_2.Width = new GridLength(size / 2);
                newGrid1.ColumnDefinitions.Add(col1_2);

                Grid.SetRow(newGrid1, i);
                Grid.SetColumn(newGrid1, 0);
                gameboard_mastermind.Children.Add(newGrid1);

                // Super Mastermind
                Grid newGrid2 = new Grid();
                newGrid2.ShowGridLines = true;

                RowDefinition row2_1 = new RowDefinition();
                row2_1.Height = new GridLength(size / 2);
                newGrid2.RowDefinitions.Add(row2_1);

                RowDefinition row2_2 = new RowDefinition();
                row2_2.Height = new GridLength(size / 2);
                newGrid2.RowDefinitions.Add(row2_2);

                ColumnDefinition col2_1 = new ColumnDefinition();
                col2_1.Width = new GridLength(size / 2);
                newGrid2.ColumnDefinitions.Add(col2_1);

                ColumnDefinition col2_2 = new ColumnDefinition();
                col2_2.Width = new GridLength(size / 2);
                newGrid2.ColumnDefinitions.Add(col2_2);

                Grid.SetRow(newGrid2, i);
                Grid.SetColumn(newGrid2, 0);
                gameboard_super_mastermind.Children.Add(newGrid2);
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