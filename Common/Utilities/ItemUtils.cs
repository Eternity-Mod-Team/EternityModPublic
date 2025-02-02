using Terraria;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        public static Item ActiveItem(this Player player)
        {
            if (!Main.mouseItem.IsAir)
                return Main.mouseItem;
            return player.HeldItem;
        }
    }
}
