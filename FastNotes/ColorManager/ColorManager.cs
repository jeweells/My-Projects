using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorManager
{
    public class HSV
    {
        int h;
        float s, v;
        public int H { get => h; set => h = (360 + value % 360) % 360; }
        public float S { get => s; set => s = (value > 1) ? 1 : (value < 0) ? 0 : value; }
        public float V { get => v; set => v = (value > 1) ? 1 : (value < 0) ? 0 : value; }

        public HSV(int h, float s, float v)
        {
            H = h; S = s; V = v;
        }
        public HSV(RGB color)
        {
            float Rp = color.R / 255f;
            float Gp = color.G / 255f;
            float Bp = color.B / 255f;
            float cmax = Math.Max(Rp, Math.Max(Gp, Bp));
            float cmin = Math.Min(Rp, Math.Min(Gp, Bp));
            float det = (cmax - cmin);
            H = (int)
                ((det == 0) ? 0 :
                (cmax == Rp) ? 60 * (((Gp - Bp) / det) % 6) :
                (cmax == Gp) ? 60 * (((Bp - Rp) / det) + 2) :
                                    60 * (((Rp - Gp) / det) + 4));
            s = (cmax == 0) ? 0 : det / cmax;
            v = cmax;
        }

        public HSV(sRGB color)
        {
            float Rp = color.R;
            float Gp = color.G;
            float Bp = color.B;
            float cmax = Math.Max(Rp, Math.Max(Gp, Bp));
            float cmin = Math.Min(Rp, Math.Min(Gp, Bp));
            float det = (cmax - cmin);
            H = (int)
                ((det == 0) ? 0 :
                (cmax == Rp) ? 60 * (((Gp - Bp) / det) % 6) :
                (cmax == Gp) ? 60 * (((Bp - Rp) / det) + 2) :
                                    60 * (((Rp - Gp) / det) + 4));
            s = (cmax == 0) ? 0 : det / cmax;
            v = cmax;
        }

    }

    public class sRGB
    {
        float r, g, b;
        public float R { get => r; set { r = (value < 0) ? 0 : (value > 1) ? 1 : value; } }
        public float G { get => g; set { g = (value < 0) ? 0 : (value > 1) ? 1 : value; } }
        public float B { get => b; set { b = (value < 0) ? 0 : (value > 1) ? 1 : value; } }
        public sRGB(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public sRGB(System.Drawing.Color color)
        {
            R = color.R/255f;
            G = color.G/255f;
            B = color.B/255f;
        }
        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb((int) (R * 255f), (int) (G * 255f), (int) (B * 255f));
        }

        public sRGB(HSL color)
        {
            float C = (1 - Math.Abs(2 * color.L - 1)) * color.S;
            float X = C * (1 - Math.Abs((color.H / 60f) % 2 - 1));
            float m = color.L - C / 2;
            float Rp = 0, Gp = 0, Bp = 0;
            if (0 <= color.H && color.H < 60)
            {
                Rp = C; Gp = X; Bp = 0;
            }
            else if (60 <= color.H && color.H < 120)
            {
                Rp = X; Gp = C; Bp = 0;
            }
            else if (120 <= color.H && color.H < 180)
            {
                Rp = 0; Gp = C; Bp = X;
            }
            else if (180 <= color.H && color.H < 240)
            {
                Rp = 0; Gp = X; Bp = C;
            }
            else if (240 <= color.H && color.H < 300)
            {
                Rp = X; Gp = 0; Bp = C;
            }
            else
            {
                Rp = C; Gp = 0; Bp = X;
            }
            R = Rp + m;
            G = Gp + m;
            B = Bp + m;
        }

        public sRGB(HSV color)
        {
            float C = color.V * color.S;
            float X = C * (1 - Math.Abs((color.H / 60f) % 2 - 1));
            float m = color.V - C;
            float Rp = 0, Gp = 0, Bp = 0;
            if (0 <= color.H && color.H < 60)
            {
                Rp = C; Gp = X; Bp = 0;
            }
            else if (60 <= color.H && color.H < 120)
            {
                Rp = X; Gp = C; Bp = 0;
            }
            else if (120 <= color.H && color.H < 180)
            {
                Rp = 0; Gp = C; Bp = X;
            }
            else if (180 <= color.H && color.H < 240)
            {
                Rp = 0; Gp = X; Bp = C;
            }
            else if (240 <= color.H && color.H < 300)
            {
                Rp = X; Gp = 0; Bp = C;
            }
            else if (300 <= color.H && color.H < 360)
            {
                Rp = C; Gp = 0; Bp = X;
            }
            R = Rp + m;
            G = Gp + m;
            B = Bp + m;
        }
        public sRGB(RGB rgb)
        {
            R = rgb.R / 255f;
            G = rgb.G / 255f;
            B = rgb.B / 255f;
        }
        public sRGB(sRGB rgb)
        {
            R = rgb.R;
            G = rgb.G;
            B = rgb.B;
        }

        public static implicit operator string(sRGB rgb)
        {
            return $"rgb({rgb.R}, {rgb.G}, {rgb.B})";
        }
    }


    public class RGB
    {
        public byte R, G, B;
        public RGB(RGB rgb)
        {
            R = rgb.R;
            G = rgb.G;
            B = rgb.B;
        }
        public RGB(sRGB rgb)
        {
            R = (byte) (rgb.R * 255);
            G = (byte) (rgb.G * 255);
            B = (byte) (rgb.B * 255);
        }
        public RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public RGB(System.Drawing.Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }
        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb(R, G, B);
        }

        public RGB(HSL color)
        {
            float C = (1 - Math.Abs(2*color.L - 1)) * color.S;
            float X = C * (1 - Math.Abs((color.H / 60f) % 2 - 1));
            float m = color.L - C/2;
            float Rp = 0, Gp = 0, Bp = 0;
            if (0 <= color.H && color.H < 60)
            {
                Rp = C; Gp = X; Bp = 0;
            }
            else if (60 <= color.H && color.H < 120)
            {
                Rp = X; Gp = C; Bp = 0;
            }
            else if (120 <= color.H && color.H < 180)
            {
                Rp = 0; Gp = C; Bp = X;
            }
            else if (180 <= color.H && color.H < 240)
            {
                Rp = 0; Gp = X; Bp = C;
            }
            else if (240 <= color.H && color.H < 300)
            {
                Rp = X; Gp = 0; Bp = C;
            }
            else
            {
                Rp = C; Gp = 0; Bp = X;
            }
            R = (byte)((Rp + m) * 255f);
            G = (byte)((Gp + m) * 255f);
            B = (byte)((Bp + m) * 255f);
        }

        public RGB(HSV color)
        {
            float C = color.V * color.S;
            float X = C * (1 - Math.Abs((color.H / 60f) % 2 - 1));
            float m = color.V - C;
            float Rp = 0, Gp = 0, Bp = 0;
            if (0 <= color.H && color.H < 60)
            {
                Rp = C; Gp = X; Bp = 0;
            }
            else if (60 <= color.H && color.H < 120)
            {
                Rp = X; Gp = C; Bp = 0;
            }
            else if (120 <= color.H && color.H < 180)
            {
                Rp = 0; Gp = C; Bp = X;
            }
            else if (180 <= color.H && color.H < 240)
            {
                Rp = 0; Gp = X; Bp = C;
            }
            else if (240 <= color.H && color.H < 300)
            {
                Rp = X; Gp = 0; Bp = C;
            }
            else if (300 <= color.H && color.H < 360)
            {
                Rp = C; Gp = 0; Bp = X;
            }
            R = (byte)((Rp + m) * 255f);
            G = (byte)((Gp + m) * 255f);
            B = (byte)((Bp + m) * 255f);
        }

        public static implicit operator string(RGB rgb)
        {
            return $"rgb({rgb.R}, {rgb.G}, {rgb.B})";
        }
    }
    public class HSL
    {
        int h;
        float s, l;
        public int H { get => h; set => h = (360 + value % 360) % 360; }
        public float S { get => s; set => s = (value > 1) ? 1 : (value < 0) ? 0 : value; }
        public float L { get => l; set => l = (value > 1) ? 1 : (value < 0) ? 0 : value; }

        public HSL(int h, float s, float l)
        {
            H = h; S = s; L = l;
        }
        public HSL(RGB color)
        {
            float Rp = color.R / 255f;
            float Gp = color.G / 255f;
            float Bp = color.B / 255f;
            float cmax = Math.Max(Rp, Math.Max(Gp, Bp));
            float cmin = Math.Min(Rp, Math.Min(Gp, Bp));
            float det = (cmax - cmin);
            H = (int)
                ((det == 0) ? 0 :
                (cmax == Rp) ? (60 * ((((Gp - Bp) / det)) % 6)) :
                (cmax == Gp) ? 60 * (((Bp - Rp) / det) + 2) :
                                    60 * (((Rp - Gp) / det) + 4));
            l = (cmax + cmin) / 2f;

            s = (cmax == 0 || l == 1) ? 0 : det / (1 - Math.Abs(2 * l - 1));
        }
        public HSL(sRGB color)
        {
            float Rp = color.R;
            float Gp = color.G;
            float Bp = color.B;
            float cmax = Math.Max(Rp, Math.Max(Gp, Bp));
            float cmin = Math.Min(Rp, Math.Min(Gp, Bp));
            float det = (cmax - cmin);
            H = (int)
                ((det == 0) ? 0 :
                (cmax == Rp) ? (60 * ((((Gp - Bp) / det)) % 6)) :
                (cmax == Gp) ? 60 * (((Bp - Rp) / det) + 2) :
                                    60 * (((Rp - Gp) / det) + 4));
            l = (cmax + cmin) / 2f;
            if (l == 1)
            {
                l = .99998f;
            }

            s = (cmax == 0) ? 0 : det / (1 - Math.Abs(2 * l - 1));
        }

    }

    public static class ColorManager
    {
        /// <summary>
        /// Contrast ratio for text and images
        /// </summary>
        public static readonly float CRLevelAA = 4.5f;
        /// <summary>
        /// Contrast ratio for large texts (above 18pt or bold above 14pt)
        /// </summary>
        public static readonly float CRLevelAALargeText = 3.0f;

        /// <summary>
        /// Contrast ratio for text and images
        /// </summary>
        public static readonly float CRLevelAAA = 7.0f;

        /// <summary>
        /// Contrast ratio for large texts (above 18pt or bold above 14pt)
        /// </summary>
        public static readonly float CRLevelAAALargeText = 4.5f;

        #region Public Members

        /// <summary>
        /// Gets a color that can be seen given a background.
        /// It just modifies the luminance of the color to fit the best possible into that desiredContrastRatio
        /// </summary>
        public static sRGB ChooseColorByContrastRatio(this sRGB start, float desiredContrastRatio)
        {
            var startShifted = start.Shift(); // Shifts the color to get the relative luminance
            float startRelativeLuminance = UnshiftedRelativeLuminance(startShifted); // Get the relative illuminance of our known color
            var startShiftedHSL = new HSL(startShifted); // Gets the shfited color in HSL form
            float endShiftedLuminance = startShiftedHSL.FindLuminance(desiredContrastRatio, startRelativeLuminance); // Finds the new luminance
            var endShiftedHSL = new HSL(startShiftedHSL.H, startShiftedHSL.S, endShiftedLuminance); // Create the new color with that luminance
            var endRGB = new sRGB(endShiftedHSL).Unshift(); // Convert it to RGB and unshift that color
            return endRGB; // This is the new color that is the most close
        }

        /// <summary>
        /// Gets a number that indicates how different are color1 and color2 (To see if the human eye can notice the difference)
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns>A smaller number means it's really hard to see the difference, 3.3 should be a stable reference</returns>
        public static float ContrastRatio(RGB color1, RGB color2)
        {
            float r1 = RelativeIuminance(color1);
            float r2 = RelativeIuminance(color2);

            //MessageBox.Show($"Rel of ({color1.ToString()}) is {r1.ToString()} \nRel of {color2.ToString()} is {r2}");
            return (Math.Max(r1, r2) + 0.05f) / (Math.Min(r1, r2) + 0.05f);
        }

        public static float ContrastRatio(sRGB color1, sRGB color2)
        {
            float r1 = RelativeIuminance(color1);
            float r2 = RelativeIuminance(color2);

            //MessageBox.Show($"Rel of ({color1.ToString()}) is {r1.ToString()} \nRel of {color2.ToString()} is {r2}");
            return (Math.Max(r1, r2) + 0.05f) / (Math.Min(r1, r2) + 0.05f);
        }

        /// <summary>
        /// Gets a relation of how light a color is
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static float RelativeIuminance(this RGB color)
        {
            float Rg = RelativeIuminanceHelper(color.R / 255f),
                Gg = RelativeIuminanceHelper(color.G / 255f),
                Bg = RelativeIuminanceHelper(color.B / 255f);
            return 0.2126f * Rg + 0.7152f * Gg + 0.0722f * Bg;
        }

        public static float RelativeIuminance(this sRGB color)
        {
            float Rg = RelativeIuminanceHelper(color.R),
                Gg = RelativeIuminanceHelper(color.G),
                Bg = RelativeIuminanceHelper(color.B);
            return 0.2126f * Rg + 0.7152f * Gg + 0.0722f * Bg;
        }

        #endregion


        #region Private Helpers

        /// <summary>
        /// Gets the relative luminance of a color without shifting the color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        static float UnshiftedRelativeLuminance(sRGB color)
        {
            return 0.2126f * color.R + 0.7152f * color.G + 0.0722f * color.B;
        }

        static float RelativeIuminanceHelper(float component)
        {

            return (component <= 0.03928) ? component / 12.92f :
                (float)Math.Pow((component + 0.055) / 1.055, 2.4f);
        }


        static float FindLuminance(this HSL shiftedTarget, float desiredContrastRatio, float startRelativeLuminance)
        {
            float a, b, c; // Constants
            int h = shiftedTarget.H;
            float rc = .2126f, gc = .7152f, bc = .0722f; // These are the constants by default when trying to find the relative luminance
            if (0 <= h && h < 60)
            {
                // r is multiplying C
                // g is multiplying X
                // b is multiplying 0
                a = rc;
                b = gc;
                c = bc;
            }// Find indexes in this order (C, X, 0) on the next tuples, indexes are (r,g,b)
            else if (60 <= h && h < 120)
            {
                // (X,C,0)
                // Index of C is g
                // Index of X is r
                // Index of 0 is b
                a = gc;
                b = rc;
                c = bc;
            }
            else if (120 <= h && h < 180)
            {
                // (0,C,X)
                // Index of C is g
                // Index of X is b
                // Index of 0 is r
                a = gc;
                b = bc;
                c = rc;
            }
            else if (180 <= h && h < 240)
            {
                // (0,X,C)
                // Index of C is b
                // Index of X is g
                // Index of 0 is r
                a = bc;
                b = gc;
                c = rc;
            }
            else if (240 <= h && h < 300)
            {
                // (X,0,C)
                // Index of C is b
                // Index of X is r
                // Index of 0 is g
                a = bc;
                b = rc;
                c = gc;
            }
            else
            { // (C,0,X)
              //    [r, g, b] = [rc, bc, gc];
              // Index of C is r
              // Index of X is b
              // Index of 0 is g
                a = rc;
                b = bc;
                c = gc;
            }

            float z = 1 - Math.Abs((h / 60f) % 2 - 1);
            float y_1 = (startRelativeLuminance + .05f) / desiredContrastRatio - .05f;
            float y_2 = (startRelativeLuminance + .05f) * desiredContrastRatio - .05f;
            float y_needed;

            if ((y_1 < 0 || y_1 > 1) && (y_2 < 0 || y_2 > 1))
            {
                // Both values are out of the boundaries
                // We get the closest value to the desired contrast
                float y1_clamp = y_1.Clamp(0, 1);
                float y2_clamp = y_2.Clamp(0, 1);

                float found_c1 = (y1_clamp + .05f) / (startRelativeLuminance + .05f);
                if (y1_clamp < startRelativeLuminance) found_c1 = 1 / found_c1;
                float found_c2 = (y2_clamp + .05f) / (startRelativeLuminance + .05f);
                if (y2_clamp < startRelativeLuminance) found_c2 = 1 / found_c2;

                float dist_c1 = Math.Abs(desiredContrastRatio - found_c1);
                float dist_c2 = Math.Abs(desiredContrastRatio - found_c2);

                y_needed = dist_c1 < dist_c2 ? y1_clamp : y2_clamp;
            }
            else
            {
                y_needed = (y_1 < 0 || y_1 > 1) ? y_2 : y_1;
            }
            float L; // Define our solution
            float s = shiftedTarget.S;

            float w_op1 = a * (s + 1) + b * (2 * s * z + 1 - s) + c * (1 - s);
            float L_op1 = y_needed / w_op1;
            // This L's must be 2L - 1 < 0
            if (0 <= L_op1 && L_op1 <= 1 && 2 * L_op1 - 1 < 0)
            {
                L = L_op1;
            }
            else
            {
                // This happens when 2L - 1 >= 0
                float w1_op2  = a * s + b * 2 * s * z - b * s - c * s;
                float w2_op2 = a * (1 - s) + b * (1 + s - 2 * s * z) + c * (1 + s);
                float L_op2 = (y_needed - w1_op2) / w2_op2;

                if (0 <= L_op2 && L_op2 <= 1 && 2 * L_op2 - 1 >= 0)
                {
                    L = L_op2;
                }
                else
                {
                    // L not found...
                    // Approximate
                    float L1 = L_op1.Clamp(0, 1);
                    float L2 = L_op2.Clamp(0, 1);
                    var rgb1 = new sRGB(new HSL(h, s, L1));
                    var rgb2 = new sRGB(new HSL(h, s, L2));

                    float lum_rgb_1 = UnshiftedRelativeLuminance(rgb1);
                    float lum_rgb_2 = UnshiftedRelativeLuminance(rgb2);

                    float cr1 = (lum_rgb_1 + .05f) / (startRelativeLuminance + .05f);
                    float cr2 = (lum_rgb_2 + .05f) / (startRelativeLuminance + .05f);

                    if (lum_rgb_1 < startRelativeLuminance) cr1 = 1 / cr1;
                    if (lum_rgb_2 < startRelativeLuminance) cr1 = 1 / cr2;

                    float dist1 = Math.Abs(cr1 - startRelativeLuminance);
                    float dist2 = Math.Abs(cr2 - startRelativeLuminance);
                    float min_dist = Math.Min(dist1, dist2);

                    L = min_dist == dist1 ? L1 : L2;

                }
            }
            return L;
        }

        static float Clamp(this float n, float min, float max)
        {
            return Math.Min(Math.Max(n, min), max);
        }

        /// <summary>
        /// Modifies the color to apply the standard 0.2126f * R + 0.7152f * G + 0.0722f * B
        /// formula to get the relative luminance
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        static sRGB Shift(this sRGB color)
        {
            float[] rgb = new float[] { color.R, color.G, color.B };
            for (int i = 0; i < 3; i++)
            {
                rgb[i] = rgb[i] <= .03928f ? rgb[i] / 12.92f : (float)Math.Pow((rgb[i] + .055) / 1.055f, 2.4f);
            }
            return new sRGB(rgb[0], rgb[1], rgb[2]);
        }


        /// <summary>
        /// Finds the color that was shifted
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        static sRGB Unshift(this sRGB color)
        {
            float[] rgb = new float[] { color.R, color.G, color.B };
            for (int i = 0; i < 3; i++)
            {
                rgb[i] = rgb[i] * 12.92f <= .03928f ? rgb[i] * 12.92f : ((float)Math.Pow(rgb[i], 1 / 2.4) * 1.055f - .055f);
            }
            return new sRGB(rgb[0], rgb[1], rgb[2]);
        }

        #endregion

    }
}
