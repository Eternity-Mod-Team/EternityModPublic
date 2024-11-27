using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using Terraria;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        public const double conversion = 1d / 255d;

        public static float FloatScale(this int rgbaValue) => (float)(rgbaValue * conversion);

        public static int IntScale(this float rgbaValue) => (int)(rgbaValue / conversion);

        public static Vector3 RGBIntToFloat(this Vector3 v) => new((float)(v.X * conversion), (float)(v.Y * conversion), (float)(v.Z * conversion));

        public static Color RGBIntToFloat(this Color color) => new((float)(color.R * conversion), (float)(color.G * conversion), (float)(color.B * conversion));

        public static Vector4 RGBAIntToFloat(this Vector4 v) => new((float)(v.X * conversion), (float)(v.Y * conversion), (float)(v.Z * conversion), (float)(v.W * conversion));

        public static Color RGBAIntToFloat(this Color color) => new((float)(color.R * conversion), (float)(color.G * conversion), (float)(color.B * conversion), (float)(color.A * conversion));

        public static Vector3 RGBFloatToInt(this Vector3 v) => new((float)(v.X / conversion), (float)(v.Y / conversion), (float)(v.Z / conversion));

        public static Color RGBFloatToInt(this Color color) => new((float)(color.R / conversion), (float)(color.G / conversion), (float)(color.B / conversion));

        public static Vector4 RGBAFloatToInt(this Vector4 v) => new((float)(v.X / conversion), (float)(v.Y / conversion), (float)(v.Z / conversion), (float)(v.W / conversion));

        public static Color RGBAFloatToInt(this Color color) => new((float)(color.R / conversion), (float)(color.G / conversion), (float)(color.B / conversion), (float)(color.A / conversion));

        public static Color ColorShift(Color firstColor, Color secondColor, float seconds)
        {
            float amount = (float)((Math.Sin(Math.PI * Math.PI / seconds * Main.GlobalTimeWrappedHourly) + 1.0) * 0.5);
            return Color.Lerp(firstColor, secondColor, amount);
        }

        public static Color ColorShiftMultiple(Color[] colors, float seconds)
        {
            float fade = Main.GameUpdateCount % (int)(seconds * 60) / (seconds * 60f);
            int index = (int)(Main.GameUpdateCount / (seconds * 60f) % colors.Length);
            return Color.Lerp(colors[index], colors[(index + 1) % colors.Length], fade);
        }

        public static Color HexToColor(string hex)
        {
            int r = int.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            int g = int.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            int b = int.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            int a = int.Parse(hex.Substring(6, 2), NumberStyles.AllowHexSpecifier);
            return new Color(r, g, b, a);
        }

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