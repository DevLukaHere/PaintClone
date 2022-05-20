using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Paint
{
        public static class ColorConvert
        {
            public static Color ConvertCmykToRgb(float c, float m, float y, float k)
            {
                int r;
                int g;
                int b;

                r = Convert.ToInt32(255 * (1 - c) * (1 - k));
                g = Convert.ToInt32(255 * (1 - m) * (1 - k));
                b = Convert.ToInt32(255 * (1 - y) * (1 - k));

                return Color.FromArgb((byte)k, (byte)r, (byte)g, (byte)b);
            }

            public static CmykColor ConvertRgbToCmyk(int r, int g, int b)
            {
                float c;
                float m;
                float y;
                float k;
                float rf;
                float gf;
                float bf;

                rf = r / 255F;
                gf = g / 255F;
                bf = b / 255F;

                k = ClampCmyk(1 - Math.Max(Math.Max(rf, gf), bf));
                c = ClampCmyk((1 - rf - k) / (1 - k));
                m = ClampCmyk((1 - gf - k) / (1 - k));
                y = ClampCmyk((1 - bf - k) / (1 - k));

                return new CmykColor(c, m, y, k);
            }

            private static float ClampCmyk(float value)
            {
                if (value < 0 || float.IsNaN(value))
                {
                    value = 0;
                }

                return value;
            }
        }
    }

