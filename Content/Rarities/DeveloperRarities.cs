using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace EternityMod.Content.Rarities
{
    public class VaemaRarity : ModRarity
    {
        public override Color RarityColor => EternityUtils.MultiColorLerp(0.5f, [Color.Black, Color.DarkViolet]);
    }
}
