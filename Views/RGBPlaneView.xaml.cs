using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Input;
using System;

namespace PixalLap.Views
{
    public partial class RGBPlaneView : UserControl
    {
        public event Action<byte, byte, byte> ColorSelected;
        public RGBPlaneView()
        {
            InitializeComponent();

            GeneratePlane();

        }

        private void GeneratePlane()
        {
            int width = 256;
            int height = 256;

            WriteableBitmap bitmap =
                new WriteableBitmap(
                    width,
                    height,
                    96,
                    96,
                    PixelFormats.Bgr32,
                    null);

            int stride = width * 4;
            byte[] pixels = new byte[height * stride];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + x * 4;

                    byte r = (byte)x;
                    byte g = (byte)y;
                    byte b = 128;

                    pixels[index] = b;
                    pixels[index + 1] = g;
                    pixels[index + 2] = r;
                    pixels[index + 3] = 255;
                }
            }

            bitmap.WritePixels(
                new System.Windows.Int32Rect(0, 0, width, height),
                pixels,
                stride,
                0);

            PlaneImage.Source = bitmap;
        }

        public void UpdateSelectedColor(byte r, byte g, byte b)
        {
            double x = r / 255.0 * PlaneImage.ActualWidth;
            double y = g / 255.0 * PlaneImage.ActualHeight;

            Marker.Margin = new System.Windows.Thickness(
                x - Marker.Width / 2,
                y - Marker.Height / 2,
                0,
                0);

            Marker.Visibility = System.Windows.Visibility.Visible;
        }

        private void PlaneImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(PlaneImage);

            double imageWidth = PlaneImage.ActualWidth;
            double imageHeight = PlaneImage.ActualHeight;

            if (imageWidth == 0 || imageHeight == 0)
                return;

            // RGB
            byte r = (byte)(position.X / imageWidth * 255);
            byte g = (byte)(position.Y / imageHeight * 255);
            byte b = 128;

            // تحريك الماركر
            Marker.Margin = new System.Windows.Thickness(
                position.X - Marker.Width / 2,
                position.Y - Marker.Height / 2,
                0,
                0);

            Marker.Visibility = System.Windows.Visibility.Visible;

            // إرسال اللون
            ColorSelected?.Invoke(r, g, b);
        }
    }
}