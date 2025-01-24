using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        public static void DoInfiniteFlightCheck(this Player player, Color textColor)
        {
            if (player.dead || !player.active)
                return;

            bool textFlag = false;
            if (!textFlag)
            {
                CombatText.NewText(player.Hitbox, textColor, Language.GetTextValue("Mods.EternityMod.Status.InfiniteFlight"), true);
                SoundEngine.PlaySound(SoundID.Item35 with { Volume = 4f, Pitch = 0.3f }, player.Center);
                textFlag = true;
            }

            player.wingTime = player.wingTimeMax;
            //player.Eternity().InfiniteFlight = true;
        }
    }
}
