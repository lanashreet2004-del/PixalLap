using PixalLap.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixalLap.Services
{
    public class HSVProcessingService
    {
        private ColorConversionService colorConversionService =
            new ColorConversionService();

        public BitmapSource AdjustHSV(
            BitmapSource image,
            double hueOffset,
            double saturationFactor,
            double valueFactor)
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

                // RGB → HSV
                HSVColor hsv =
                    colorConversionService.RGBToHSV(rgb);

                // تعديل Hue
                hsv.H += hueOffset;

                if (hsv.H > 360)
                {
                    hsv.H -= 360;
                }

                if (hsv.H < 0)
                {
                    hsv.H += 360;
                }

                // تعديل Saturation
                hsv.S *= saturationFactor;

                if (hsv.S > 1)
                {
                    hsv.S = 1;
                }

                if (hsv.S < 0)
                {
                    hsv.S = 0;
                }

                // تعديل Value
                hsv.V *= valueFactor;

                if (hsv.V > 1)
                {
                    hsv.V = 1;
                }

                if (hsv.V < 0)
                {
                    hsv.V = 0;
                }

                // HSV → RGB
                RGBColor newRgb =
                    colorConversionService.HSVToRGB(hsv);

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
    }
}