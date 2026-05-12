using System.Windows;
using System.Windows.Controls;
using PixalLap.ColorSpaces;

namespace PixalLap.Views
{
    public partial class RGBSpaceView : UserControl
    {
        private RGBSpace3D rgbSpace;

        public RGBSpaceView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rgbSpace = new RGBSpace3D(view1);

            rgbSpace.GenerateCube();
        }

        public void UpdateSelectedColor(byte r, byte g, byte b)
        {
            rgbSpace.ShowSelectedColor(r, g, b);
        }

    }
}