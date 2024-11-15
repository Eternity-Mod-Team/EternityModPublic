using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace EternityMod.Rarities
{
    public class BloodbathRarity : ModRarity
    {
        public override Color RarityColor => EternityUtils.ColorShift(Color.DarkRed, Color.Crimson, 2);
    }
}