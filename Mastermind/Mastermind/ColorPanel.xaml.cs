using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Mastermind
{
    public partial class ColorPanel : Window
    {
        Ellipse selected_circle;
        Label[] color_labels;

        public ColorPanel(ref Ellipse selected_circle, int num_colors)
        {
            InitializeComponent();

            color_labels = new Label[] { label_red, label_yellow, label_green, label_blue, label_purple, label_white, label_black, label_gray };

            this.selected_circle = selected_circle;

            for (int i = num_colors; i < color_labels.Length; i++)
                color_labels[i].Visibility = Visibility.Collapsed;

            foreach (Label l in grid_colorpanel.Children)
            {
                l.BorderBrush = Brushes.Black;
                l.BorderThickness = new Thickness(1);
                l.MouseLeftButtonDown += getLabelColor;
                l.TouchDown += getLabelColor;
            }
        }

        private void getLabelColor(object sender, EventArgs e)
        {
            Label senderLabel = sender as Label;
            selected_circle.Fill = senderLabel.Background as SolidColorBrush;
            Close();
        }
    }
}
