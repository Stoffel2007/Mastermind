using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mastermind
{
    public partial class MainWindow : Window
    {
        Ellipse[,] mastermind_circles, mastermind_circles_check, super_mastermind_circles, super_mastermind_circles_check;
        Ellipse[] mastermind_circles_solution, super_mastermind_circles_solution;
        int cell_size, mastermind_round_counter, super_mastermind_round_counter, num_rounds;
        bool game_is_super_mastermind;
        
        public MainWindow()
        {
            Top = 10;
            Left = 10;

            InitializeComponent();

            cell_size = 40;
            mastermind_round_counter = 0;
            super_mastermind_round_counter = 0;
            num_rounds = 12;
            game_is_super_mastermind = false;

            mastermind_circles = new Ellipse[num_rounds, 4];
            mastermind_circles_check = new Ellipse[num_rounds, 4];
            mastermind_circles_solution = new Ellipse[4];

            InitializeCircles(ref gameboard_mastermind, ref mastermind_circles, ref mastermind_circles_check, ref mastermind_circles_solution);

            super_mastermind_circles = new Ellipse[num_rounds, 5];
            super_mastermind_circles_check = new Ellipse[num_rounds, 5];
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
                    Ellipse newCircle1 = new Ellipse();
                    newCircle1.Height = cell_size / 2 - 5;
                    newCircle1.Width = cell_size / 2 - 5;
                    newCircle1.Stroke = Brushes.Black;
                    newCircle1.Fill = Brushes.White;
                    newCircle1.Tag = new int[] { i, j };
                    Grid.SetRow(newCircle1, i);
                    Grid.SetColumn(newCircle1, j);
                    gameboard.Children.Add(newCircle1);
                    circles_check[i, j] = newCircle1;

                    Ellipse newCircle2 = new Ellipse();
                    newCircle2 = new Ellipse();
                    newCircle2.Height = cell_size - 5;
                    newCircle2.Width = cell_size - 5;
                    newCircle2.Stroke = Brushes.Black;
                    newCircle2.Fill = Brushes.White;
                    newCircle2.Tag = new int[] { i, j };
                    newCircle2.MouseLeftButtonDown += openColorPanel;
                    Grid.SetRow(newCircle2, i);
                    Grid.SetColumn(newCircle2, j + width);
                    gameboard.Children.Add(newCircle2);
                    circles[i, j] = newCircle2;
                }
            }
        }

        private void InitializeGameboards(ref Grid gameboard, ref Ellipse[,] circles, ref Ellipse[,] circles_check, ref Ellipse[] circles_solution)
        {
            int length = circles.GetLength(0);
            int width = circles.GetLength(1);

            gameboard.Height = length * (cell_size + 10);
            gameboard.Width = 1.5* width * cell_size;

            for (int i = 0; i < length; i++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = new GridLength(cell_size + 10);
                gameboard.RowDefinitions.Add(newRow);
            }

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(cell_size / 2);
                gameboard.ColumnDefinitions.Add(newCol);
            }

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(cell_size);
                gameboard.ColumnDefinitions.Add(newCol);
            }
        }

        private void openColorPanel(object sender, EventArgs e)
        {
            Ellipse circle = sender as Ellipse;
            int[] coordinates = circle.Tag as int[];
            int x = coordinates[0];
            int y = coordinates[1];

            if (!game_is_super_mastermind && x + mastermind_round_counter == num_rounds - 1)
                setColorFromPanel(mastermind_circles[x, y], false);
            else if (x + super_mastermind_round_counter == num_rounds - 1)
                setColorFromPanel(super_mastermind_circles[x, y], true);
        }

        private void setColorFromPanel(Ellipse sender, bool is_super_mastermind)
        {
            Point mousePosition = Mouse.GetPosition(this);

            ColorPanel colorPanel = new ColorPanel(ref sender, is_super_mastermind);
            colorPanel.Owner = this;

            colorPanel.Top = mousePosition.Y - 120;
            colorPanel.Left = mousePosition.X + 25;
            if (WindowState != WindowState.Maximized)
            {
                colorPanel.Top += Top;
                colorPanel.Left += Left;
            }

            colorPanel.ShowDialog();
        }

        private void changeGameboard(object sender, EventArgs e)
        {
            ComboBoxItem senderBox = sender as ComboBoxItem;

            if (senderBox.Tag.ToString() == "1")
            {
                gameboard_mastermind.Visibility = Visibility.Visible;
                gameboard_super_mastermind.Visibility = Visibility.Collapsed;
                combobox_super_mastermind.Visibility = Visibility.Visible;
                game_is_super_mastermind = false;
            }
            else
            {
                gameboard_mastermind.Visibility = Visibility.Collapsed;
                gameboard_super_mastermind.Visibility = Visibility.Visible;
                combobox_mastermind.Visibility = Visibility.Visible;
                game_is_super_mastermind = true;
            }

            senderBox.Visibility = Visibility.Collapsed;
        }
    }
}