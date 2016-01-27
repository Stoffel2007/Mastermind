using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mastermind
{
    public partial class MainWindow : Window
    {
        Ellipse[,] mastermind_circles, mastermind_circles_check, super_mastermind_circles, super_mastermind_circles_check;
        Ellipse[] mastermind_circles_solution, super_mastermind_circles_solution;
        int cell_size, mastermind_round_counter, super_mastermind_round_counter, num_rounds, num_circles_mastermind, num_circles_super_mastermind, num_colors_mastermind, num_colors_super_mastermind;
        bool game_is_super_mastermind;
        SolidColorBrush[] random_colors;
        SolidColorBrush background_color;
        
        public MainWindow()
        {
            Top = 0;
            Left = 0;

            InitializeComponent();

            cell_size = 35;
            mastermind_round_counter = 0;
            super_mastermind_round_counter = 0;
            num_rounds = 12;
            num_circles_mastermind = 4;
            num_circles_super_mastermind = 5;
            num_colors_mastermind = 6;
            num_colors_super_mastermind = 8;
            game_is_super_mastermind = false;
            random_colors = new SolidColorBrush[] { Brushes.Red, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Purple, Brushes.White, Brushes.Black, Brushes.Gray };
            background_color = Brushes.LightGreen;
            Background = background_color;

            mastermind_circles = new Ellipse[num_rounds, num_circles_mastermind];
            mastermind_circles_check = new Ellipse[num_rounds, num_circles_mastermind];

            InitializeCirclesInSolution(ref solution_mastermind, ref mastermind_circles_solution, num_circles_mastermind, num_colors_mastermind);
            InitializeCirclesInGrid(ref gameboard_mastermind, ref mastermind_circles, ref mastermind_circles_check);

            super_mastermind_circles = new Ellipse[num_rounds, num_circles_super_mastermind];
            super_mastermind_circles_check = new Ellipse[num_rounds, num_circles_super_mastermind];

            InitializeCirclesInSolution(ref solution_super_mastermind, ref super_mastermind_circles_solution, num_circles_super_mastermind, num_colors_super_mastermind);
            InitializeCirclesInGrid(ref gameboard_super_mastermind, ref super_mastermind_circles, ref super_mastermind_circles_check);
        }

        private void playRound(object sender, EventArgs e)
        {
            if (!game_is_super_mastermind)
                playRound(ref solution_mastermind, ref mastermind_circles, ref mastermind_circles_check, ref mastermind_circles_solution, ref mastermind_round_counter);
            else
                playRound(ref solution_super_mastermind, ref super_mastermind_circles, ref super_mastermind_circles_check, ref super_mastermind_circles_solution, ref super_mastermind_round_counter);
        }

        private void playRound(ref Grid grid_solution, ref Ellipse[,] circles, ref Ellipse[,] circles_check, ref Ellipse[] circles_solution, ref int round_counter)
        {
            int row = num_rounds - round_counter - 1;
            int num_right_color_and_position = 0;
            int num_right_color = 0;
            int num_circles = circles.GetLength(1);
            bool all_circles_filled = true;
            bool[] color_is_correct = new bool[num_circles];
            bool[] circle_is_checked = new bool[num_circles];

            for (int i = 0; i < num_circles; i++)
            {
                if (circles[row, i].Fill == circles_solution[i].Fill)
                {
                    num_right_color_and_position++;
                    color_is_correct[i] = true;
                    circle_is_checked[i] = true;
                }
                else if (circles[row, i].Fill == background_color)
                    all_circles_filled = false;
            }

            for (int i = 0; i < num_circles; i++)
            {
                if (!color_is_correct[i])
                {
                    for (int j = 0; j < num_circles; j++)
                    {
                        if (!circle_is_checked[j])
                        {
                            if (circles[row, i].Fill == circles_solution[j].Fill)
                            {
                                num_right_color++;
                                circle_is_checked[j] = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (num_right_color_and_position == num_circles)
            {
                grid_solution.Visibility = Visibility.Visible;
                MessageBox.Show("YOU WON! :-)");
                resetGameboard();
            }
            else if (all_circles_filled)
            {
                for (int i = 0; i < num_right_color_and_position; i++)
                    circles_check[row, i].Fill = Brushes.Black;

                for (int i = num_right_color_and_position; i < num_right_color_and_position + num_right_color; i++)
                    circles_check[row, i].Fill = Brushes.White;

                round_counter++;

                if (round_counter == 12)
                {
                    grid_solution.Visibility = Visibility.Visible;
                    MessageBox.Show("YOU LOST! :-(");
                    resetGameboard();
                }
            }
            else
                MessageBox.Show("Not all circles are filled yet!");
        }

        private void resetGameboard(object sender, EventArgs e)
        {
            if (!game_is_super_mastermind)
                solution_mastermind.Visibility = Visibility.Visible;
            else
                solution_super_mastermind.Visibility = Visibility.Visible;
            MessageBox.Show("Game was resetted");
            resetGameboard();
        }

        private void resetGameboard()
        {
            if (!game_is_super_mastermind)
            {
                mastermind_round_counter = 0;
                for (int i = 0; i < mastermind_circles.GetLength(0); i++)
                {
                    for (int j = 0; j < mastermind_circles.GetLength(1); j++)
                    {
                        mastermind_circles[i, j].Fill = background_color;
                        mastermind_circles_check[i, j].Fill = background_color;
                    }
                }
                generateRandomSolution(ref mastermind_circles_solution, 6);
                solution_mastermind.Visibility = Visibility.Hidden;
            }
            else
            {
                super_mastermind_round_counter = 0;
                for (int i = 0; i < super_mastermind_circles.GetLength(0); i++)
                {
                    for (int j = 0; j < super_mastermind_circles.GetLength(1); j++)
                    {
                        super_mastermind_circles[i, j].Fill = background_color;
                        super_mastermind_circles_check[i, j].Fill = background_color;
                    }
                }
                generateRandomSolution(ref super_mastermind_circles_solution, 8);
                solution_super_mastermind.Visibility = Visibility.Hidden;
            }
        }

        private void generateRandomSolution(ref Ellipse[] circles_solution, int num_colors)
        {
            Random rand = new Random();

            for (int i = 0; i < circles_solution.Length; i++)
                circles_solution[i].Fill = random_colors[rand.Next(num_colors)];
        }

        private void InitializeCirclesInSolution(ref Grid grid_solution, ref Ellipse[] circles, int num_circles, int num_colors)
        {
            Random rand = new Random();
            circles = new Ellipse[num_circles];
            grid_solution.Height = cell_size;
            grid_solution.Width = num_circles * cell_size;

            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(cell_size);
            grid_solution.RowDefinitions.Add(row);

            for (int i = 0; i < num_circles; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(cell_size);
                grid_solution.ColumnDefinitions.Add(newCol);

                circles[i] = new Ellipse();
                circles[i].Height = cell_size - 5;
                circles[i].Width = cell_size - 5;
                circles[i].Stroke = Brushes.Black;
                circles[i].Fill = random_colors[rand.Next(num_colors)];
                Grid.SetRow(circles[i], 0);
                Grid.SetColumn(circles[i], i);
                grid_solution.Children.Add(circles[i]);
            }
        }

        private void InitializeCirclesInGrid(ref Grid grid_gameboard, ref Ellipse[,] circles, ref Ellipse[,] circles_check)
        {
            InitializeGameboards(ref grid_gameboard, ref circles, ref circles_check);

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
                    newCircle1.Tag = new int[] { i, j };
                    Grid.SetRow(newCircle1, i);
                    Grid.SetColumn(newCircle1, j + 1);
                    grid_gameboard.Children.Add(newCircle1);
                    circles_check[i, j] = newCircle1;

                    Ellipse newCircle2 = new Ellipse();
                    newCircle2 = new Ellipse();
                    newCircle2.Height = cell_size - 5;
                    newCircle2.Width = cell_size - 5;
                    newCircle2.Stroke = Brushes.Black;
                    newCircle2.Fill = background_color;
                    newCircle2.Tag = new int[] { i, j };
                    newCircle2.MouseLeftButtonDown += openColorPanel;
                    Grid.SetRow(newCircle2, i);
                    Grid.SetColumn(newCircle2, j + width + 1);
                    grid_gameboard.Children.Add(newCircle2);
                    circles[i, j] = newCircle2;
                }
            }
        }

        private void InitializeGameboards(ref Grid grid_gameboard, ref Ellipse[,] circles, ref Ellipse[,] circles_check)
        {
            int length = circles.GetLength(0);
            int width = circles.GetLength(1);

            grid_gameboard.Height = length * (cell_size + 10);
            grid_gameboard.Width = 1.5 * width * cell_size + cell_size / 2 + 10;

            ColumnDefinition round_num_col = new ColumnDefinition();
            round_num_col.Width = new GridLength(cell_size / 2 + 10);
            grid_gameboard.ColumnDefinitions.Add(round_num_col);

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(cell_size / 2);
                grid_gameboard.ColumnDefinitions.Add(newCol);
            }

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(cell_size);
                grid_gameboard.ColumnDefinitions.Add(newCol);
            }

            for (int i = 0; i < length; i++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = new GridLength(cell_size + 10);
                grid_gameboard.RowDefinitions.Add(newRow);

                TextBlock round_num = new TextBlock();
                round_num.Inlines.Add(new Bold(new Run((12 - i).ToString())));
                Grid.SetRow(round_num, i);
                Grid.SetColumn(round_num, 0);
                round_num.VerticalAlignment = VerticalAlignment.Center;
                round_num.HorizontalAlignment = HorizontalAlignment.Center;
                round_num.FontSize = 20;
                grid_gameboard.Children.Add(round_num);
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
            else if (game_is_super_mastermind && x + super_mastermind_round_counter == num_rounds - 1)
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

            if (senderBox.Content.ToString() == "Mastermind")
            {
                gameboard_mastermind.Visibility = Visibility.Visible;
                gameboard_super_mastermind.Visibility = Visibility.Collapsed;
                solution_mastermind.Visibility = Visibility.Hidden;
                solution_super_mastermind.Visibility = Visibility.Collapsed;
                combobox_super_mastermind.Visibility = Visibility.Visible;
                game_is_super_mastermind = false;
            }
            else
            {
                gameboard_mastermind.Visibility = Visibility.Collapsed;
                gameboard_super_mastermind.Visibility = Visibility.Visible;
                solution_mastermind.Visibility = Visibility.Collapsed;
                solution_super_mastermind.Visibility = Visibility.Hidden;
                combobox_mastermind.Visibility = Visibility.Visible;
                game_is_super_mastermind = true;
            }

            senderBox.Visibility = Visibility.Collapsed;
            button_start.Visibility = Visibility.Visible;
            button_reset.Visibility = Visibility.Visible;
        }
    }
}