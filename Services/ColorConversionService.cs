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


        public XYZColor RGBToXYZ(RGBColor rgb)
        {
            double r = rgb.R / 255.0;
            double g = rgb.G / 255.0;
            double b = rgb.B / 255.0;

            // Gamma correction
            r = (r > 0.04045)
                ? Math.Pow((r + 0.055) / 1.055, 2.4)
                : r / 12.92;

            g = (g > 0.04045)
                ? Math.Pow((g + 0.055) / 1.055, 2.4)
                : g / 12.92;

            b = (b > 0.04045)
                ? Math.Pow((b + 0.055) / 1.055, 2.4)
                : b / 12.92;

            r *= 100;
            g *= 100;
            b *= 100;

            XYZColor xyz = new XYZColor();

            xyz.X =
                r * 0.4124 +
                g * 0.3576 +
                b * 0.1805;

            xyz.Y =
                r * 0.2126 +
                g * 0.7152 +
                b * 0.0722;

            xyz.Z =
                r * 0.0193 +
                g * 0.1192 +
                b * 0.9505;

            return xyz;
        }

        public LABColor XYZToLAB(XYZColor xyz)
        {
            double x = xyz.X / 95.047;
            double y = xyz.Y / 100.000;
            double z = xyz.Z / 108.883;

            x = PivotXYZ(x);
            y = PivotXYZ(y);
            z = PivotXYZ(z);

            LABColor lab = new LABColor();

            lab.L = (116 * y) - 16;

            lab.A = 500 * (x - y);

            lab.B = 200 * (y - z);

            return lab;
        }

        private double PivotXYZ(double value)
        {
            if (value > 0.008856)
            {
                return Math.Pow(value, 1.0 / 3.0);
            }

            return (7.787 * value) + (16.0 / 116.0);
        }


        public XYZColor LABToXYZ(LABColor lab)
        {
            double y =
                (lab.L + 16) / 116.0;

            double x =
                (lab.A / 500.0) + y;

            double z =
                y - (lab.B / 200.0);

            x = InversePivotXYZ(x);
            y = InversePivotXYZ(y);
            z = InversePivotXYZ(z);

            XYZColor xyz = new XYZColor();

            xyz.X = x * 95.047;
            xyz.Y = y * 100.000;
            xyz.Z = z * 108.883;

            return xyz;
        }

        public RGBColor XYZToRGB(XYZColor xyz)
        {
            double x = xyz.X / 100.0;
            double y = xyz.Y / 100.0;
            double z = xyz.Z / 100.0;

            double r =
                x * 3.2406 +
                y * -1.5372 +
                z * -0.4986;

            double g =
                x * -0.9689 +
                y * 1.8758 +
                z * 0.0415;

            double b =
                x * 0.0557 +
                y * -0.2040 +
                z * 1.0570;

            r = (r > 0.0031308)
                ? 1.055 * Math.Pow(r, 1 / 2.4) - 0.055
                : r * 12.92;

            g = (g > 0.0031308)
                ? 1.055 * Math.Pow(g, 1 / 2.4) - 0.055
                : g * 12.92;

            b = (b > 0.0031308)
                ? 1.055 * Math.Pow(b, 1 / 2.4) - 0.055
                : b * 12.92;

            RGBColor rgb = new RGBColor();

            rgb.R = ClampRGB(r * 255);
            rgb.G = ClampRGB(g * 255);
            rgb.B = ClampRGB(b * 255);

            return rgb;
        }

        private double InversePivotXYZ(double value)
        {
            double valueCubed =
                value * value * value;

            if (valueCubed > 0.008856)
            {
                return valueCubed;
            }

            return (value - 16.0 / 116.0) / 7.787;
        }
    }
}