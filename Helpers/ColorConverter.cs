using System;

namespace PixalLap.Helpers
{
    public static class ColorConverter
    {
        public static (double H, double S, double V)
            RGBtoHSV(byte r, byte g, byte b)
        {
            double rNorm = r / 255.0;
            double gNorm = g / 255.0;
            double bNorm = b / 255.0;

            double max = Math.Max(rNorm,
                         Math.Max(gNorm, bNorm));

            double min = Math.Min(rNorm,
                         Math.Min(gNorm, bNorm));

            double delta = max - min;

            double h = 0;

            if (delta != 0)
            {
                if (max == rNorm)
                {
                    h = 60 * (((gNorm - bNorm) / delta) % 6);
                }
                else if (max == gNorm)
                {
                    h = 60 * (((bNorm - rNorm) / delta) + 2);
                }
                else
                {
                    h = 60 * (((rNorm - gNorm) / delta) + 4);
                }
            }

            if (h < 0)
            {
                h += 360;
            }

            double s = max == 0 ? 0 : delta / max;

            double v = max;

            return (h, s * 100, v * 100);
        }
    }
}