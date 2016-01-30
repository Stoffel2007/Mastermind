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
        ComboBoxItem[] comboboxes;
        Grid[] grid_gameboard, grid_solution;
        int[] round_counter, num_rounds, num_circles, num_colors;
        int cell_size, game_mode;
        SolidColorBrush[] random_colors;
        SolidColorBrush background_color;
        bool[] game_is_blocked;
        Ellipse[][][] circles, circles_check;
        Ellipse[][] circles_solution;
        TextBlock[][] textblock_current_round;

        public MainWindow()
        {
            Top = 0;
            Left = 0;

            InitializeComponent();

            comboboxes = new ComboBoxItem[] { combobox_mastermind, combobox_super_mastermind, combobox_mastermind_light, combobox_travel_mastermind, combobox_travel_mastermind };
            grid_gameboard = new Grid[] { grid_gameboard_mastermind, grid_gameboard_super_mastermind, grid_gameboard_mastermind_light, grid_gameboard_travel_mastermind, grid_gameboard_mini_mastermind };
            grid_solution = new Grid[] { grid_solution_mastermind, grid_solution_super_mastermind, grid_solution_mastermind_light, grid_solution_travel_mastermind, grid_solution_mini_mastermind };
            round_counter = new int[] { 0, 0, 0, 0, 0 };
            num_rounds = new int[] { 12, 12, 8, 6, 8 };
            num_circles = new int[] { 4, 5, 3, 4, 4 };
            num_colors = new int[] { 6, 8, 4, 6, 6 };
            cell_size = 35;
            random_colors = new SolidColorBrush[] { Brushes.Red, Brushes.Yellow, Brushes.Green, Brushes.Blue, Brushes.Purple, Brushes.White, Brushes.Black, Brushes.Gray };
            background_color = Brushes.LightGreen;
            game_is_blocked = new bool[] { false, false, false, false, false };
            
            Background = background_color;
            int num_modes = round_counter.Length;

            circles = new Ellipse[num_modes][][];
            circles_check = new Ellipse[num_modes][][];
            circles_solution = new Ellipse[num_modes][];
            textblock_current_round = new TextBlock[num_modes][];

            for (int mode = 0; mode < num_modes; mode++)
            {
                InitializeJaggedArrays(mode);
                InitializeGameboards(mode);
                AdjustSolutionCircles(mode);
                AdjustGameboardCircles(mode);
            }

            HideElements();

            int tag = 0;
            foreach (ComboBoxItem item in combobox_game_mode.Items)
            {
                item.Tag = (tag - 1).ToString();
                tag++;
            }
        }

        // Game logic
        #region
        private void playRound(object sender, EventArgs e)
        {
            if (!game_is_blocked[game_mode])
            {
                int row = num_rounds[game_mode] - round_counter[game_mode] - 1;
                int num_right_color_and_position = 0;
                int num_right_color = 0;
                bool all_circles_filled = true;
                bool[] color_is_correct = new bool[num_circles[game_mode]];
                bool[] circle_is_checked = new bool[num_circles[game_mode]];

                for (int i = 0; i < num_circles[game_mode]; i++)
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

                for (int i = 0; i < num_circles[game_mode]; i++)
                {
                    if (!color_is_correct[i])
                    {
                        for (int j = 0; j < num_circles[game_mode]; j++)
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

                if (num_right_color_and_position == num_circles[game_mode])
                {
                    for (int i = 0; i < num_right_color_and_position; i++)
                        circles_check[game_mode][row][i].Fill = Brushes.Black;

                    grid_solution[game_mode].Visibility = Visibility.Visible;
                    sendMessage("YOU WON! :-)");
                    game_is_blocked[game_mode] = true;
                }
                else if (all_circles_filled)
                {
                    for (int i = 0; i < num_right_color_and_position; i++)
                        circles_check[game_mode][row][i].Fill = Brushes.Black;

                    for (int i = num_right_color_and_position; i < num_right_color_and_position + num_right_color; i++)
                        circles_check[game_mode][row][i].Fill = Brushes.White;

                    changeRound(round_counter[game_mode] + 1, game_mode);

                    if (round_counter[game_mode] == num_rounds[game_mode])
                        loss();
                }
                else
                    sendMessage("Not all circles are filled yet!");
            }
        }

        private void resign(object sender, EventArgs e)
        {
            if (!game_is_blocked[game_mode] && round_counter[game_mode] != 0 && MessageBox.Show("Are you sure?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                loss();
        }

        private void loss()
        {
            game_is_blocked[game_mode] = true;
            grid_solution[game_mode].Visibility = Visibility.Visible;
            sendMessage("YOU LOST! :-(");
        }

        private void generateRandomSolution()
        {
            Random rand = new Random();

            for (int i = 0; i < circles_solution[game_mode].Length; i++)
                circles_solution[game_mode][i].Fill = random_colors[rand.Next(num_colors[game_mode])];
        }
        #endregion

        // Gameboard functions
        #region
        private void changeGameboard(object sender, EventArgs e)
        {
            game_mode = Convert.ToInt32((sender as ComboBoxItem).Tag);

            for (int i = 0; i < grid_solution.Length; i++)
            {
                grid_gameboard[i].Visibility = Visibility.Collapsed;
                grid_solution[i].Visibility = Visibility.Collapsed;
                comboboxes[i].Visibility = Visibility.Visible;
            }

            grid_gameboard[game_mode].Visibility = Visibility.Visible;
            grid_solution[game_mode].Visibility = Visibility.Hidden;
            comboboxes[game_mode].Visibility = Visibility.Collapsed;

            stackpanel_buttons.Visibility = Visibility.Visible;
        }

        private void resetGameboard(object sender, EventArgs e)
        {
            if (round_counter[game_mode] != 0 && MessageBox.Show("Are you sure?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                game_is_blocked[game_mode] = false;
                changeRound(0, game_mode);
                for (int i = 0; i < circles[game_mode].Length; i++)
                {
                    for (int j = 0; j < circles[game_mode][i].Length; j++)
                    {
                        circles[game_mode][i][j].Fill = background_color;
                        circles_check[game_mode][i][j].Fill = background_color;
                    }
                }

                generateRandomSolution();
                grid_solution[game_mode].Visibility = Visibility.Hidden;

                MessageBox.Show("Game was reset");
            }
        }

        private void changeRound(int round, int mode)
        {
            round_counter[mode] = round;
            for (int i = 0; i < num_rounds[mode]; i++)
                textblock_current_round[mode][i].Text = (num_rounds[mode] - i).ToString();

            if (round != num_rounds[mode])
            {
                textblock_current_round[mode][num_rounds[mode] - round - 1].Text = "";
                textblock_current_round[mode][num_rounds[mode] - round - 1].Inlines.Add(new Bold(new Run((round + 1).ToString())));
            }
        }
        #endregion

        // Color panel funcions
        #region
        private void openColorPanel(object sender, EventArgs e)
        {
            Ellipse circle = sender as Ellipse;
            int[] coordinates = circle.Tag as int[];
            int x = coordinates[0];
            int y = coordinates[1];

            if (round_counter[game_mode] + x + 1 == num_rounds[game_mode] && !game_is_blocked[game_mode])
                setColorFromPanel(circles[game_mode][x][y]);
        }

        private void setColorFromPanel(Ellipse sender)
        {
            Point mousePosition = Mouse.GetPosition(this);

            ColorPanel colorPanel = new ColorPanel(ref sender, num_colors[game_mode]);
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
        #endregion

        // Initializing functions
        #region
        private void InitializeJaggedArrays(int mode)
        {
            circles[mode] = new Ellipse[num_rounds[mode]][];
            for (int j = 0; j < num_rounds[mode]; j++)
                circles[mode][j] = new Ellipse[num_circles[mode]];

            circles_check[mode] = new Ellipse[num_rounds[mode]][];
            for (int j = 0; j < num_rounds[mode]; j++)
                circles_check[mode][j] = new Ellipse[num_circles[mode]];

            circles_solution[mode] = new Ellipse[num_circles[mode]];

            textblock_current_round[mode] = new TextBlock[num_rounds[mode]];
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

                TextBlock text_round_num = new TextBlock();
                text_round_num.Text = (num_rounds[mode] - i).ToString();
                Grid.SetRow(text_round_num, i);
                Grid.SetColumn(text_round_num, 0);
                text_round_num.VerticalAlignment = VerticalAlignment.Center;
                text_round_num.HorizontalAlignment = HorizontalAlignment.Center;
                text_round_num.FontSize = 20;
                grid_gameboard[mode].Children.Add(text_round_num);
                textblock_current_round[mode][i] = text_round_num;
            }

            changeRound(0, mode);
        }

        private void AdjustSolutionCircles(int mode)
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

        private void AdjustGameboardCircles(int mode)
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
                    newCircle2.TouchDown += openColorPanel;
                    Grid.SetRow(newCircle2, i);
                    Grid.SetColumn(newCircle2, j + width + 1);
                    grid_gameboard[mode].Children.Add(newCircle2);
                    circles[mode][i][j] = newCircle2;
                }
            }
        }

        private void HideElements()
        {
            foreach (ComboBoxItem item in combobox_game_mode.Items)
                item.Selected += changeGameboard;

            foreach (UIElement element in stackpanel_grids.Children)
                element.Visibility = Visibility.Collapsed;
        }
        #endregion

        // Help functions
        #region
        private void sendMessage(string message)
        {
            MessageBox.Show(message);
        }
        #endregion
    }
}