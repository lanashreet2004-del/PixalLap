using PixalLap.Models;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixalLap.Services
{
    public class YUVProcessingService
    {
        private ColorConversionService colorConversionService =
            new ColorConversionService();

        public BitmapSource AdjustYUV(
            BitmapSource image,
            double yFactor,
            double uFactor,
            double vFactor)
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

                YUVColor yuv =
                    colorConversionService.RGBToYUV(rgb);

                yuv.Y *= yFactor;
                yuv.U *= uFactor;
                yuv.V *= vFactor;

                RGBColor newRgb =
                    colorConversionService.YUVToRGB(yuv);

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