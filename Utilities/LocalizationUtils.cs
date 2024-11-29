using Terraria.Localization;

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
    }
}
