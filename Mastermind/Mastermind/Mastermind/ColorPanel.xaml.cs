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

        public ColorPanel(ref Ellipse selected_circle, bool is_super_mastermind)
        {
            InitializeComponent();

            this.selected_circle = selected_circle;

            if (!is_super_mastermind)
            {
                label_optional_color_1.Visibility = Visibility.Collapsed;
                label_optional_color_2.Visibility = Visibility.Collapsed;
            }

            foreach (Label l in grid_colorpanel.Children)
                l.MouseLeftButtonDown += getLabelColor;
        }

        private void getLabelColor(object sender, EventArgs e)
        {
            Label senderLabel = sender as Label;
            selected_circle.Fill = senderLabel.Background as SolidColorBrush;
            Close();
        }
    }
}
