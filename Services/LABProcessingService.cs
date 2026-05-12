using PixalLap.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixalLap.Services
{
    public class LABProcessingService
    {
        private ColorConversionService colorConversionService =
            new ColorConversionService();

        public BitmapSource AdjustLAB(
            BitmapSource image,
            double lFactor,
            double aFactor,
            double bFactor)
        {
            WriteableBitmap writableBitmap =
                new WriteableBitmap(image);

            int width = writableBitmap.PixelWidth;
            int height = writableBitmap.PixelHeight;
            int stride = width * 4;

            byte[] pixels = new byte[height * stride];

            writableBitmap.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                RGBColor rgb = new RGBColor
                {
                    B = pixels[i],
                    G = pixels[i + 1],
                    R = pixels[i + 2]
                };

                // RGB → XYZ
                XYZColor xyz =
                    colorConversionService.RGBToXYZ(rgb);

                // XYZ → LAB
                LABColor lab =
                    colorConversionService.XYZToLAB(xyz);

                // تعديل القيم
                lab.L *= lFactor;

                lab.A *= aFactor;

                lab.B *= bFactor;

                // حماية
                lab.L = Clamp(lab.L, 0, 100);

                lab.A = Clamp(lab.A, -128, 127);

                lab.B = Clamp(lab.B, -128, 127);

                // LAB → XYZ
                XYZColor newXyz =
                    colorConversionService.LABToXYZ(lab);

                // XYZ → RGB
                RGBColor newRgb =
                    colorConversionService.XYZToRGB(newXyz);

                pixels[i] = (byte)newRgb.B;
                pixels[i + 1] = (byte)newRgb.G;
                pixels[i + 2] = (byte)newRgb.R;
            }

            WriteableBitmap result =
                new WriteableBitmap(
                    width,
                    height,
                    writableBitmap.DpiX,
                    writableBitmap.DpiY,
                    PixelFormats.Bgra32,
                    null);

            result.WritePixels(
                new System.Windows.Int32Rect(
                    0,
                    0,
                    width,
                    height),
                pixels,
                stride,
                0);

            return result;
        }

        private double Clamp(
            double value,
            double min,
            double max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
    }
}