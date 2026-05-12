using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixalLap.Services
{
    public class ColorProcessingService
    {
        public BitmapSource RemoveRedChannel(BitmapSource image)
        {
            WriteableBitmap writableBitmap = new WriteableBitmap(image);

            int width = writableBitmap.PixelWidth;
            int height = writableBitmap.PixelHeight;
            int stride = width * 4;

            byte[] pixels = new byte[height * stride];

            writableBitmap.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i + 2] = 0;
            }

            WriteableBitmap result = new WriteableBitmap(
                width,
                height,
                writableBitmap.DpiX,
                writableBitmap.DpiY,
                PixelFormats.Bgra32,
                null);

            result.WritePixels(
                new Int32Rect(0, 0, width, height),
                pixels,
                stride,
                0);

            return result;
        }

        public BitmapSource RemoveGreenChannel(BitmapSource image)
        {
            WriteableBitmap writableBitmap = new WriteableBitmap(image);

            int width = writableBitmap.PixelWidth;
            int height = writableBitmap.PixelHeight;
            int stride = width * 4;

            byte[] pixels = new byte[height * stride];

            writableBitmap.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i + 1] = 0;
            }

            WriteableBitmap result = new WriteableBitmap(
                width,
                height,
                writableBitmap.DpiX,
                writableBitmap.DpiY,
                PixelFormats.Bgra32,
                null);

            result.WritePixels(
                new Int32Rect(0, 0, width, height),
                pixels,
                stride,
                0);

            return result;
        }

        public BitmapSource RemoveBlueChannel(BitmapSource image)
        {
            WriteableBitmap writableBitmap = new WriteableBitmap(image);

            int width = writableBitmap.PixelWidth;
            int height = writableBitmap.PixelHeight;
            int stride = width * 4;

            byte[] pixels = new byte[height * stride];

            writableBitmap.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                pixels[i] = 0;
            }

            WriteableBitmap result = new WriteableBitmap(
                width,
                height,
                writableBitmap.DpiX,
                writableBitmap.DpiY,
                PixelFormats.Bgra32,
                null);

            result.WritePixels(
                new Int32Rect(0, 0, width, height),
                pixels,
                stride,
                0);

            return result;
        }

        public BitmapSource AdjustRGBChannels(
     BitmapSource image,
     double redFactor,
     double greenFactor,
     double blueFactor)
        {
            WriteableBitmap writableBitmap = new WriteableBitmap(image);

            int width = writableBitmap.PixelWidth;
            int height = writableBitmap.PixelHeight;
            int stride = width * 4;

            byte[] pixels = new byte[height * stride];

            writableBitmap.CopyPixels(pixels, stride, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                // Blue
                pixels[i] = (byte)Math.Max(
     0,
     Math.Min(255, pixels[i] * blueFactor));

                // Green
                pixels[i + 1] = (byte)Math.Max(
     0,
     Math.Min(255, pixels[i + 1] * greenFactor));

                // Red
                pixels[i + 2] = (byte)Math.Max(
     0,
     Math.Min(255, pixels[i + 2] * redFactor));
            }

            WriteableBitmap result = new WriteableBitmap(
                width,
                height,
                writableBitmap.DpiX,
                writableBitmap.DpiY,
                PixelFormats.Bgra32,
                null);

            result.WritePixels(
                new System.Windows.Int32Rect(0, 0, width, height),
                pixels,
                stride,
                0);

            return result;
        }

    }
}