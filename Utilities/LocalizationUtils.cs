using Terraria.Chat;
using Terraria.ID;
using Terraria;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        public static LocalizedText GetText(string key)
        {
            return Language.GetOrRegister("Mods.EternityMod." + key);
        }

        public static string GetTextValue(string key)
        {
            return Language.GetTextValue("Mods.EternityMod." + key);
        }

        public static void DisplayLocalizedText(string key, Color? textColor = null)
        {
            // An attempt to bypass the need for a separate method and runtime/compile-time parameter
            // constraints by using nulls for defaults.
            if (!textColor.HasValue)
                textColor = Color.White;

            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(Language.GetTextValue(key), textColor.Value);
            else if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), textColor.Value);
        }
    }
}
