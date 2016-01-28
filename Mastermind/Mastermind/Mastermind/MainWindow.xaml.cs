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
        Ellipse[][][] circles, circles_check;
        Ellipse[][] circles_solution;
        ComboBoxItem[] comboboxes;
        Grid[] grid_gameboard, grid_solution;
        int[] round_counter, num_rounds, num_circles, num_colors;
        int cell_size, game_mode;
        SolidColorBrush[] random_colors;
        SolidColorBrush background_color;

        public MainWindow()
        {
            Top = 0;
            Left = 0;

            InitializeComponent();

            cell_size = 35;
            round_counter = new int[] { 0, 0 };
            num_rounds = new int[] { 12, 12 };
            num_circles = new int[] { 4, 5 };
            num_colors = new int[] { 6, 8 };
            game_mode = -1;
            random_colors = new SolidColorBrush[] { Brushes.Red, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Purple, Brushes.White, Brushes.Black, Brushes.Gray };
            background_color = Brushes.LightGreen;
            Background = background_color;

            grid_solution = new Grid[] { grid_solution_mastermind, grid_solution_super_mastermind };
            grid_gameboard = new Grid[] { grid_gameboard_mastermind, grid_gameboard_super_mastermind };

            comboboxes = new ComboBoxItem[] { combobox_mastermind, combobox_super_mastermind };

            int num_modes = round_counter.Length;

            circles = new Ellipse[num_modes][][];
            circles_check = new Ellipse[num_modes][][];
            circles_solution = new Ellipse[num_modes][];

            for (int mode = 0; mode < num_modes; mode++)
            {
                circles[mode] = new Ellipse[num_rounds[mode]][];
                for (int j = 0; j < num_rounds[mode]; j++)
                    circles[mode][j] = new Ellipse[num_circles[mode]];

                circles_check[mode] = new Ellipse[num_rounds[mode]][];
                for (int j = 0; j < num_rounds[mode]; j++)
                    circles_check[mode][j] = new Ellipse[num_circles[mode]];

                circles_solution[mode] = new Ellipse[num_circles[mode]];

                InitializeCirclesInSolution(mode);
                InitializeGameboards(mode);
                InitializeCirclesInGrid(mode);
            }
        }

        private void playRound(object sender, EventArgs e)
        {
            int row = num_rounds[game_mode] - round_counter[game_mode] - 1;
            int num_right_color_and_position = 0;
            int num_right_color = 0;
            int num_circles = circles[game_mode][0].Length;
            bool all_circles_filled = true;
            bool[] color_is_correct = new bool[num_circles];
            bool[] circle_is_checked = new bool[num_circles];

            for (int i = 0; i < num_circles; i++)
            {
                if (circles[game_mode][row][i].Fill == circles_solution[game_mode][i].Fill)
                {
                    num_right_color_and_position++;
                    color_is_correct[i] = true;
                    circle_is_checked[i] = true;
                }
                else if (circles[game_mode][row][i].Fill == background_color)
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
                            if (circles[game_mode][row][i].Fill == circles_solution[game_mode][j].Fill)
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
                grid_solution[game_mode].Visibility = Visibility.Visible;
                MessageBox.Show("YOU WON! :-)");
                resetGameboard();
            }
            else if (all_circles_filled)
            {
                for (int i = 0; i < num_right_color_and_position; i++)
                    circles_check[game_mode][row][i].Fill = Brushes.Black;

                for (int i = num_right_color_and_position; i < num_right_color_and_position + num_right_color; i++)
                    circles_check[game_mode][row][i].Fill = Brushes.White;

                round_counter[game_mode]++;

                if (round_counter[game_mode] == 12)
                {
                    grid_solution[game_mode].Visibility = Visibility.Visible;
                    MessageBox.Show("YOU LOST! :-(");
                    resetGameboard();
                }
            }
            else
                MessageBox.Show("Not all circles are filled yet!");
        }

        private void resetGameboard(object sender = null, EventArgs e = null)
        {
            grid_solution[game_mode].Visibility = Visibility.Visible;
            MessageBox.Show("Game was resetted");

            round_counter[game_mode] = 0;
            for (int i = 0; i < circles[game_mode].Length; i++)
            {
                for (int j = 0; j < circles[game_mode][i].Length; j++)
                {
                    circles[game_mode][i][j].Fill = background_color;
                    circles[game_mode][i][j].Fill = background_color;
                }
            }
            generateRandomSolution();
            grid_solution[game_mode].Visibility = Visibility.Hidden;
        }

        private void generateRandomSolution()
        {
            Random rand = new Random();

            for (int i = 0; i < circles_solution.Length; i++)
                circles_solution[game_mode][i].Fill = random_colors[rand.Next(num_colors[game_mode])];
        }

        private void InitializeCirclesInSolution(int mode)
        {
            Random rand = new Random();
            grid_solution[mode].Height = cell_size;
            grid_solution[mode].Width = num_circles[mode] * cell_size;

            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(cell_size);
            grid_solution[mode].RowDefinitions.Add(row);

            for (int j = 0; j < num_circles[mode]; j++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(cell_size);
                grid_solution[mode].ColumnDefinitions.Add(newCol);

                Ellipse newCircle = new Ellipse();
                newCircle.Height = cell_size - 5;
                newCircle.Width = cell_size - 5;
                newCircle.Stroke = Brushes.Black;
                newCircle.Fill = random_colors[rand.Next(num_colors[mode])];
                Grid.SetRow(newCircle, 0);
                Grid.SetColumn(newCircle, j);
                grid_solution[mode].Children.Add(newCircle);
                circles_solution[mode][j] = newCircle;
            }
        }

        private void InitializeCirclesInGrid(int mode)
        {
            int length = circles[mode].Length;
            int width = circles[mode][0].Length;

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
                    grid_gameboard[mode].Children.Add(newCircle1);
                    circles_check[mode][i][j] = newCircle1;

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
                    grid_gameboard[mode].Children.Add(newCircle2);
                    circles[mode][i][j] = newCircle2;
                }
            }
        }

        private void InitializeGameboards(int mode)
        {
            int length = circles[mode].Length;
            int width = circles[mode][0].Length;

            grid_gameboard[mode].Height = length * (cell_size + 10);
            grid_gameboard[mode].Width = 1.5 * width * cell_size + cell_size / 2 + 10;

            ColumnDefinition round_num_col = new ColumnDefinition();
            round_num_col.Width = new GridLength(cell_size / 2 + 10);
            grid_gameboard[mode].ColumnDefinitions.Add(round_num_col);

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(cell_size / 2);
                grid_gameboard[mode].ColumnDefinitions.Add(newCol);
            }

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition newCol = new ColumnDefinition();
                newCol.Width = new GridLength(cell_size);
                grid_gameboard[mode].ColumnDefinitions.Add(newCol);
            }

            for (int i = 0; i < length; i++)
            {
                RowDefinition newRow = new RowDefinition();
                newRow.Height = new GridLength(cell_size + 10);
                grid_gameboard[mode].RowDefinitions.Add(newRow);

                TextBlock round_num = new TextBlock();
                round_num.Inlines.Add(new Bold(new Run((12 - i).ToString())));
                Grid.SetRow(round_num, i);
                Grid.SetColumn(round_num, 0);
                round_num.VerticalAlignment = VerticalAlignment.Center;
                round_num.HorizontalAlignment = HorizontalAlignment.Center;
                round_num.FontSize = 20;
                grid_gameboard[mode].Children.Add(round_num);
            }
        }

        private void openColorPanel(object sender, EventArgs e)
        {
            Ellipse circle = sender as Ellipse;
            int[] coordinates = circle.Tag as int[];
            int x = coordinates[0];
            int y = coordinates[1];

            if (round_counter[game_mode] == num_rounds[game_mode] - x - 1)
                setColorFromPanel(circles[game_mode][x][y]);
        }

        private void setColorFromPanel(Ellipse sender)
        {
            Point mousePosition = Mouse.GetPosition(this);
            
            ColorPanel colorPanel = new ColorPanel(ref sender, game_mode == 1);
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
                game_mode = 0;
            else
                game_mode = 1;

            grid_gameboard[game_mode].Visibility = Visibility.Visible;
            grid_gameboard[- game_mode + 1].Visibility = Visibility.Collapsed;
            grid_solution[game_mode].Visibility = Visibility.Hidden;
            grid_solution[- game_mode + 1].Visibility = Visibility.Collapsed;
            comboboxes[- game_mode + 1].Visibility = Visibility.Visible;
            comboboxes[game_mode].Visibility = Visibility.Collapsed;
            button_start.Visibility = Visibility.Visible;
            button_reset.Visibility = Visibility.Visible;
        }
    }
}