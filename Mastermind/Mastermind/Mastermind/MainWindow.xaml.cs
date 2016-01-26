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
        Ellipse[] mastermind_circles_solution, super_mastermind_circles_solution;
        double size;
        
        public MainWindow()
        {
            InitializeComponent();

            size = 40;

            mastermind_circles = new Ellipse[12, 4];
            mastermind_circles_check = new Ellipse[12, 4];
            mastermind_circles_solution = new Ellipse[4];

            InitializeCircles(ref gameboard_mastermind, ref mastermind_circles, ref mastermind_circles_check, ref mastermind_circles_solution);

            super_mastermind_circles = new Ellipse[12, 5];
            super_mastermind_circles_check = new Ellipse[12, 5];
            super_mastermind_circles_solution = new Ellipse[4];

            InitializeCircles(ref gameboard_super_mastermind, ref super_mastermind_circles, ref super_mastermind_circles_check, ref mastermind_circles_solution);
        }

        private void InitializeCircles(ref Grid gameboard, ref Ellipse[,] circles, ref Ellipse[,] circles_check, ref Ellipse[] circles_solution)
        {
            InitializeGameboards(ref gameboard, ref circles, ref circles_check, ref circles_solution);

            int length = circles.GetLength(0);
            int width = circles.GetLength(1);

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    circles_check[i, j] = new Ellipse();
                    circles_check[i, j].Height = size / 2 - 5;
                    circles_check[i, j].Width = size / 2 - 5;
                    circles_check[i, j].Stroke = Brushes.Black;
                    circles_check[i, j].Fill = Brushes.AliceBlue;
                    circles_check[i, j].MouseLeftButtonDown += openColorPanel;
                    Grid.SetRow(circles_check[i, j], i);
                    Grid.SetColumn(circles_check[i, j], j);
                    gameboard.Children.Add(circles_check[i, j]);

                    circles[i, j] = new Ellipse();
                    circles[i, j].Height = size - 5;
                    circles[i, j].Width = size - 5;
                    circles[i, j].Stroke = Brushes.Black;
                    circles[i, j].Fill = Brushes.AliceBlue;
                    circles_check[i, j].MouseLeftButtonDown += openColorPanel;
                    Grid.SetRow(circles[i, j], i);
                    Grid.SetColumn(circles[i, j], j + width);
                    gameboard.Children.Add(circles[i, j]);
                }
            }
        }

        private void InitializeGameboards(ref Grid gameboard, ref Ellipse[,] circles, ref Ellipse[,] circles_check, ref Ellipse[] circles_solution)
        {
            int length = circles.GetLength(0);
            int width = circles.GetLength(1);

            gameboard.Height = size * length;
            gameboard.Width = 1.5 * size * width;

            for (int i = 0; i < length; i++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = new GridLength(size);
                gameboard.RowDefinitions.Add(newRow);
            }

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(size / 2);
                gameboard.ColumnDefinitions.Add(newCol);
            }

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(size);
                gameboard.ColumnDefinitions.Add(newCol);
            }
        }

        private void openColorPanel(object sender, EventArgs e)
        {
            Ellipse circle = sender as Ellipse;
            int[] coordinates = circle.Tag as int[];
            int x = coordinates[0];
            int y = coordinates[1];

            Ellipse colorPanel = new Ellipse();
            colorPanel.Height = size + 10;
            colorPanel.Width = size + 10;
            colorPanel.Stroke = Brushes.Blue;
            Grid.SetRow(colorPanel, x + 1);
            Grid.SetColumn(colorPanel, y + 1);
            gameboard_mastermind.Children.Add(colorPanel);
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