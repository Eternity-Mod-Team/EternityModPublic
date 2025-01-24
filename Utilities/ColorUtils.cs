using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using System.Text;
using Terraria;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        public const double Conversion = 1d / 255d;

        public static float FloatScale(this int rgbaValue) => (float)(rgbaValue * Conversion);

        public static int IntScale(this float rgbaValue) => (int)(rgbaValue / Conversion);

        public static Vector3 RGBIntToFloat(this Vector3 v) => new((float)(v.X * Conversion), (float)(v.Y * Conversion), (float)(v.Z * Conversion));

        public static Color RGBIntToFloat(this Color color) => new((float)(color.R * Conversion), (float)(color.G * Conversion), (float)(color.B * Conversion));

        public static Vector4 RGBAIntToFloat(this Vector4 v) => new((float)(v.X * Conversion), (float)(v.Y * Conversion), (float)(v.Z * Conversion), (float)(v.W * Conversion));

        public static Color RGBAIntToFloat(this Color color) => new((float)(color.R * Conversion), (float)(color.G * Conversion), (float)(color.B * Conversion), (float)(color.A * Conversion));

        public static Vector3 RGBFloatToInt(this Vector3 v) => new((float)(v.X / Conversion), (float)(v.Y / Conversion), (float)(v.Z / Conversion));

        public static Color RGBFloatToInt(this Color color) => new((float)(color.R / Conversion), (float)(color.G / Conversion), (float)(color.B / Conversion));

        public static Vector4 RGBAFloatToInt(this Vector4 v) => new((float)(v.X / Conversion), (float)(v.Y / Conversion), (float)(v.Z / Conversion), (float)(v.W / Conversion));

        public static Color RGBAFloatToInt(this Color color) => new((float)(color.R / Conversion), (float)(color.G / Conversion), (float)(color.B / Conversion), (float)(color.A / Conversion));

        public static Color HexToColor(string hex)
        {
            int r = int.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            int g = int.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            int b = int.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            int a = int.Parse(hex.Substring(6, 2), NumberStyles.AllowHexSpecifier);
            return new Color(r, g, b, a);
        }

        public static string ColorMessage(string msg, Color color)
        {
            StringBuilder sb;
            if (!msg.Contains('\n'))
            {
                sb = new StringBuilder(msg.Length + 12);
                sb.Append("[c/").Append(color.Hex3()).Append(':').Append(msg).Append(']');
            }
            else
            {
                sb = new StringBuilder();
                foreach (string newlineSlice in msg.Split('\n'))
                    sb.Append("[c/").Append(color.Hex3()).Append(':').Append(newlineSlice).Append(']').Append('\n');
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a color lerp that allows for smooth transitioning between two given colors
        /// </summary>
        /// <param name="firstColor">The first color you want it to switch between</param>
        /// <param name="secondColor">The second color you want it to switch between</param>
        /// <param name="seconds">How long you want it to take to swap between colors</param>
        public static Color ColorSwap(Color firstColor, Color secondColor, float seconds)
        {
            double timeMult = (double)(MathHelper.TwoPi / seconds);
            float colorMePurple = (float)((Math.Sin(timeMult * Main.GlobalTimeWrappedHourly) + 1) * 0.5f);
            return Color.Lerp(firstColor, secondColor, colorMePurple);
        }

        /// <summary>
        /// Returns a color lerp that supports multiple colors.
        /// </summary>
        /// <param name="increment">The 0-1 incremental value used when interpolating.</param>
        /// <param name="colors">The various colors to interpolate across.</param>
        /// <returns></returns>
        public static Color MultiColorLerp(float increment, params Color[] colors)
        {
            increment %= 0.999f;
            int currentColorIndex = (int)(increment * colors.Length);
            Color color = colors[currentColorIndex];
            Color nextColor = colors[(currentColorIndex + 1) % colors.Length];
            return Color.Lerp(color, nextColor, increment * colors.Length % 1f);
        }
    }
}
