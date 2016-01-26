using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mastermind
{
    public partial class MainWindow : Window
    {
        Ellipse[,] mastermind_circles, mastermind_circles_check, super_mastermind_circles, super_mastermind_circles_check;
        Ellipse[] mastermind_solution, super_mastermind_solution;
        double size;
        
        public MainWindow()
        {
            InitializeComponent();

            size = 40;

            drawGameboards();
            drawCircles();

            mastermind_circles = new Ellipse[12, 4];
            mastermind_circles_check = new Ellipse[12, 4];
            super_mastermind_circles = new Ellipse[12, 5];
            super_mastermind_circles_check = new Ellipse[12, 5];

            mastermind_solution = new Ellipse[4];
            super_mastermind_solution = new Ellipse[5];
        }

        private void drawGameboards()
        {
            gameboard_mastermind.Height = 13 * size;
            gameboard_mastermind.Width = 5 * size;
            
            gameboard_super_mastermind.Height = 13 * size;
            gameboard_super_mastermind.Width = 7.5 * size;

            // Both
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
            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(size/2);
                gameboard_super_mastermind.ColumnDefinitions.Add(newCol);
            }

            for (int i = 0; i < 5; i++)
            {

                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(size);
                gameboard_super_mastermind.ColumnDefinitions.Add(newCol);
            }

            // Mastermind
            for (int i = 1; i < 13; i++)
            {
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
            }
        }

        private void drawCircles()
        {
            for (int i = 0; i < 12; i++)
            {
                // Mastermind
                for (int j = 0; j < 4; j++)
                {
                    //mastermind_circles[i, j] = null;
                    Ellipse newCircle = new Ellipse();
                    newCircle.Width = size - 5;
                    newCircle.Height = size - 5;
                    newCircle.Stroke = Brushes.Black;
                    Grid.SetRow(newCircle, i + 1);
                    Grid.SetColumn(newCircle, j + 1);
                    gameboard_mastermind.Children.Add(newCircle);
                    //mastermind_circles[i, j] = newCircle;
                }

                // Super Mastermind
                for (int j = 0; j < 5; j++)
                {
                    Ellipse newCircle = new Ellipse();
                    newCircle.Width = size - 5;
                    newCircle.Height = size - 5;
                    newCircle.Stroke = Brushes.Black;
                    Grid.SetRow(newCircle, i + 1);
                    Grid.SetColumn(newCircle, j + 5);
                    gameboard_super_mastermind.Children.Add(newCircle);
                }
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