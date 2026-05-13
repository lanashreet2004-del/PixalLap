using System.Windows;
using System.Windows.Controls;
using PixalLap.Views;
using System.Windows.Media;
using PixalLap.Helpers;

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
            // RGB Cube
            if (VisualizationComboBox.SelectedIndex == 0)
            {
                RGBSpaceView cubeView = new RGBSpaceView();

                cubeView.ColorSelected += (r, g, b) =>
                {
                    UpdateSelectedColor(r, g, b);
                };

                VisualizationContent.Content = cubeView;
            }

            // RGB Plane
            else if (VisualizationComboBox.SelectedIndex == 1)
            {
                RGBPlaneView planeView = new RGBPlaneView();

                planeView.ColorSelected += (r, g, b) =>
                {
                    UpdateSelectedColor(r, g, b);
                };

                VisualizationContent.Content = planeView;
            }

            // HSV Wheel
            else if (VisualizationComboBox.SelectedIndex == 2)
            {
                HSVWheelView hsvView = new HSVWheelView();

                hsvView.ColorSelected += (r, g, b) =>
                {
                    UpdateSelectedColor(r, g, b);
                };

                VisualizationContent.Content = hsvView;
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

            if (VisualizationContent.Content is HSVWheelView hsvView)
            {
                hsvView.UpdateSelectedColor(r, g, b);
            }
            // Preview
            SelectedColorPreview.Background =
                new SolidColorBrush(
                    Color.FromRgb(r, g, b));

            // RGB
            RgbText.Text = $"R: {r}\nG: {g}\nB: {b}";

            var hsv = Helpers.ColorConverter.RGBtoHSV(r, g, b);

            HsvText.Text =
                $"H : {hsv.H:F1}°\n" +
                $"S : {hsv.S:F1}%\n" +
                $"V : {hsv.V:F1}%";

        }
    }
}