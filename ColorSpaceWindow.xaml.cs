using System.Windows;
using System.Windows.Controls;
using PixalLap.Views;

namespace PixalLap
{
    public partial class ColorSpaceWindow : Window
    {
        public ColorSpaceWindow()
        {
            InitializeComponent();

            VisualizationComboBox.SelectedIndex = 0;
        }

        private void VisualizationComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (VisualizationComboBox.SelectedIndex == 0)
            {
                VisualizationContent.Content = new RGBSpaceView();
            }
            else
            {
                VisualizationContent.Content = new RGBPlaneView();
            }
        }
        public void UpdateSelectedColor(byte r, byte g, byte b)
        {
            if (VisualizationContent.Content is RGBSpaceView rgbView)
            {
                rgbView.UpdateSelectedColor(r, g, b);
            }

            if (VisualizationContent.Content is RGBPlaneView planeView)
            {
                planeView.UpdateSelectedColor(r, g, b);
            }
        }
    }
}