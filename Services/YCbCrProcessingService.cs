using PixalLap.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixalLap.Services
{
    public class YCbCrProcessingService
    {
        private ColorConversionService colorConversionService =
            new ColorConversionService();

        public BitmapSource AdjustYCbCr(
            BitmapSource image,
            double yFactor,
            double cbFactor,
            double crFactor)
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

                // RGB → YCbCr
                YCbCrColor ycbcr =
                    colorConversionService.RGBToYCbCr(rgb);

                // تعديل القيم
                ycbcr.Y *= yFactor;

                ycbcr.Cb =
                    128 +
                    ((ycbcr.Cb - 128) * cbFactor);

                ycbcr.Cr =
                    128 +
                    ((ycbcr.Cr - 128) * crFactor);

                // حماية
                ycbcr.Y = Clamp(ycbcr.Y);
                ycbcr.Cb = Clamp(ycbcr.Cb);
                ycbcr.Cr = Clamp(ycbcr.Cr);

                // YCbCr → RGB
                RGBColor newRgb =
                    colorConversionService.YCbCrToRGB(ycbcr);

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

            if (value > 255)
                return 255;

            return value;
        }
    }
}