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

        public ColorPanel(ref Ellipse selected_circle)
        {
            InitializeComponent();

            this.selected_circle = selected_circle;

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
