using System;
using PixalLap.Models;

namespace PixalLap.Services
{
    public class ColorConversionService
    {
        public HSVColor RGBToHSV(RGBColor rgb)
        {
            double r = rgb.R / 255.0;
            double g = rgb.G / 255.0;
            double b = rgb.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            double delta = max - min;

            HSVColor hsv = new HSVColor();

            // Hue
            if (delta == 0)
            {
                hsv.H = 0;
            }
            else if (max == r)
            {
                hsv.H = 60 * (((g - b) / delta) % 6);
            }
            else if (max == g)
            {
                hsv.H = 60 * (((b - r) / delta) + 2);
            }
            else
            {
                hsv.H = 60 * (((r - g) / delta) + 4);
            }

            if (hsv.H < 0)
            {
                hsv.H += 360;
            }

            // Saturation
            if (max == 0)
            {
                hsv.S = 0;
            }
            else
            {
                hsv.S = delta / max;
            }

            // Value
            hsv.V = max;

            return hsv;
        }

        public RGBColor HSVToRGB(HSVColor hsv)
        {
            double h = hsv.H;
            double s = hsv.S;
            double v = hsv.V;

            double c = v * s;

            double x = c * (1 - Math.Abs((h / 60.0 % 2) - 1));

            double m = v - c;

            double rPrime = 0;
            double gPrime = 0;
            double bPrime = 0;

            if (h >= 0 && h < 60)
            {
                rPrime = c;
                gPrime = x;
                bPrime = 0;
            }
            else if (h >= 60 && h < 120)
            {
                rPrime = x;
                gPrime = c;
                bPrime = 0;
            }
            else if (h >= 120 && h < 180)
            {
                rPrime = 0;
                gPrime = c;
                bPrime = x;
            }
            else if (h >= 180 && h < 240)
            {
                rPrime = 0;
                gPrime = x;
                bPrime = c;
            }
            else if (h >= 240 && h < 300)
            {
                rPrime = x;
                gPrime = 0;
                bPrime = c;
            }
            else
            {
                rPrime = c;
                gPrime = 0;
                bPrime = x;
            }

            RGBColor rgb = new RGBColor();

            rgb.R = (rPrime + m) * 255;
            rgb.G = (gPrime + m) * 255;
            rgb.B = (bPrime + m) * 255;

            return rgb;
        }

        public CMYKColor RGBToCMYK(RGBColor rgb)
        {
            double r = rgb.R / 255.0;
            double g = rgb.G / 255.0;
            double b = rgb.B / 255.0;

            double k = 1 - Math.Max(r, Math.Max(g, b));

            CMYKColor cmyk = new CMYKColor();

            cmyk.K = k;

            if (k == 1)
            {
                cmyk.C = 0;
                cmyk.M = 0;
                cmyk.Y = 0;
            }
            else
            {
                cmyk.C = (1 - r - k) / (1 - k);

                cmyk.M = (1 - g - k) / (1 - k);

                cmyk.Y = (1 - b - k) / (1 - k);
            }

            return cmyk;
        }

        public RGBColor CMYKToRGB(CMYKColor cmyk)
        {
            RGBColor rgb = new RGBColor();

            rgb.R = 255 * (1 - cmyk.C) * (1 - cmyk.K);

            rgb.G = 255 * (1 - cmyk.M) * (1 - cmyk.K);

            rgb.B = 255 * (1 - cmyk.Y) * (1 - cmyk.K);

            return rgb;
        }

        public YCbCrColor RGBToYCbCr(RGBColor rgb)
        {
            YCbCrColor ycbcr = new YCbCrColor();

            ycbcr.Y =
                (0.299 * rgb.R) +
                (0.587 * rgb.G) +
                (0.114 * rgb.B);

            ycbcr.Cb =
                128 -
                (0.168736 * rgb.R) -
                (0.331264 * rgb.G) +
                (0.5 * rgb.B);

            ycbcr.Cr =
                128 +
                (0.5 * rgb.R) -
                (0.418688 * rgb.G) -
                (0.081312 * rgb.B);

            return ycbcr;
        }

        public RGBColor YCbCrToRGB(YCbCrColor ycbcr)
        {
            RGBColor rgb = new RGBColor();

            rgb.R =
                ycbcr.Y +
                1.402 * (ycbcr.Cr - 128);

            rgb.G =
                ycbcr.Y -
                0.344136 * (ycbcr.Cb - 128) -
                0.714136 * (ycbcr.Cr - 128);

            rgb.B =
                ycbcr.Y +
                1.772 * (ycbcr.Cb - 128);

            rgb.R = ClampRGB(rgb.R);
            rgb.G = ClampRGB(rgb.G);
            rgb.B = ClampRGB(rgb.B);

            return rgb;
        }
        public YUVColor RGBToYUV(RGBColor rgb)
        {
            YUVColor yuv = new YUVColor();

            yuv.Y =
                (0.299 * rgb.R) +
                (0.587 * rgb.G) +
                (0.114 * rgb.B);

            yuv.U =
                (-0.14713 * rgb.R) -
                (0.28886 * rgb.G) +
                (0.436 * rgb.B);

            yuv.V =
                (0.615 * rgb.R) -
                (0.51499 * rgb.G) -
                (0.10001 * rgb.B);

            return yuv;
        }

        public RGBColor YUVToRGB(YUVColor yuv)
        {
            RGBColor rgb = new RGBColor();

            rgb.R =
                yuv.Y +
                (1.13983 * yuv.V);

            rgb.G =
                yuv.Y -
                (0.39465 * yuv.U) -
                (0.58060 * yuv.V);

            rgb.B =
                yuv.Y +
                (2.03211 * yuv.U);

            rgb.R = ClampRGB(rgb.R);
            rgb.G = ClampRGB(rgb.G);
            rgb.B = ClampRGB(rgb.B);

            return rgb;
        }

        private double ClampRGB(double value)
        {
            if (value < 0)
                return 0;

            if (value > 255)
                return 255;

            return value;
        }
    }
}