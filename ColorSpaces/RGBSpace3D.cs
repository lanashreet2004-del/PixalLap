using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PixalLap.ColorSpaces
{
    public class RGBSpace3D
    {
        private readonly HelixViewport3D viewport;

        private SphereVisual3D selectedMarker;

        public RGBSpace3D(HelixViewport3D viewport)
        {
            this.viewport = viewport;
        }

        public void GenerateCube()
        {

            // محور Red
            var xAxis = new LinesVisual3D
            {
                Color = Colors.Red,
                Thickness = 3
            };

            xAxis.Points.Add(new Point3D(-5, -5, -5));
            xAxis.Points.Add(new Point3D(5, -5, -5));

            viewport.Children.Add(xAxis);


            // محور Green
            var yAxis = new LinesVisual3D
            {
                Color = Colors.Lime,
                Thickness = 3
            };

            yAxis.Points.Add(new Point3D(-5, -5, -5));
            yAxis.Points.Add(new Point3D(-5, 5, -5));

            viewport.Children.Add(yAxis);


            // محور Blue
            var zAxis = new LinesVisual3D
            {
                Color = Colors.Blue,
                Thickness = 3
            };

            zAxis.Points.Add(new Point3D(-5, -5, -5));
            zAxis.Points.Add(new Point3D(-5, -5, 5));

            viewport.Children.Add(zAxis);
            // توليد نقاط RGB
            for (int r = 0; r <= 255; r += 32)
            {
                for (int g = 0; g <= 255; g += 32)
                {
                    for (int b = 0; b <= 255; b += 32)
                    {
                        SphereVisual3D sphere = new SphereVisual3D
                        {
                            Center = new Point3D(
                                r / 32.0 - 4,
                                g / 32.0 - 4,
                                b / 32.0 - 4),

                            Radius = 0.15,

                            Fill = new SolidColorBrush(
                                Color.FromRgb(
                                    (byte)r,
                                    (byte)g,
                                    (byte)b))
                        };

                        viewport.Children.Add(sphere);
                    }
                }
            }
        }
        public void ShowSelectedColor(byte r, byte g, byte b)
        {
            // حذف الماركر القديم
            if (selectedMarker != null)
            {
                viewport.Children.Remove(selectedMarker);
            }

            // إنشاء ماركر جديد
            selectedMarker = new SphereVisual3D
            {
                Center = new Point3D(
                    r / 32.0 - 4,
                    g / 32.0 - 4,
                    b / 32.0 - 4),

                Radius = 0.4,

                Fill = Brushes.White
            };

            viewport.Children.Add(selectedMarker);
        }
    }
}