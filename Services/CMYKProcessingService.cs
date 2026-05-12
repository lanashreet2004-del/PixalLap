using PixalLap.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixalLap.Services
{
    public class CMYKProcessingService
    {
        private ColorConversionService colorConversionService =
            new ColorConversionService();

        public BitmapSource AdjustCMYK(
            BitmapSource image,
            double cyanFactor,
            double magentaFactor,
            double yellowFactor,
            double blackFactor)
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

                // RGB → CMYK
                CMYKColor cmyk =
                    colorConversionService.RGBToCMYK(rgb);

                // تعديل القيم
                cmyk.C *= cyanFactor;
                cmyk.M *= magentaFactor;
                cmyk.Y *= yellowFactor;
                cmyk.K *= blackFactor;

                // حماية
                cmyk.C = Clamp(cmyk.C);
                cmyk.M = Clamp(cmyk.M);
                cmyk.Y = Clamp(cmyk.Y);
                cmyk.K = Clamp(cmyk.K);

                // CMYK → RGB
                RGBColor newRgb =
                    colorConversionService.CMYKToRGB(cmyk);

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

        private double Clamp(double value)
        {
            if (value < 0)
                return 0;

            if (value > 1)
                return 1;

            return value;
        }
    }
}