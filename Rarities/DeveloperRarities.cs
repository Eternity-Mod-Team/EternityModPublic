using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace EternityMod.Rarities
{
    public class VaemaRarity : ModRarity
    {
        public override Color RarityColor => EternityUtils.ColorShift(Color.DarkViolet, Color.Black, 2);
    }
}