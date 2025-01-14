using Terraria.ModLoader;

namespace EternityMod.Players
{
    public partial class EternityPlayer : ModPlayer
    {
        // Buffs
        // ...

        // Debuffs
        public bool eCurse = false;
        public bool dInferno = false;

        internal void ResetVariables()
        {
            eCurse = false;
            dInferno = false;
        }

        public override void ResetEffects() => ResetVariables();
    }
}
