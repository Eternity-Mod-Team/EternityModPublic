using Terraria.ModLoader;

namespace EternityMod.Common.Globals.Players
{
    public partial class EternityPlayer : ModPlayer
    {
        // Buff variables.
        // ...

        // Debuff variables.
        public bool EtherealCurse = false;
        public bool DraconicInferno = false;

        internal void ResetVariables()
        {
            EtherealCurse = false;
            DraconicInferno = false;
        }

        public override void ResetEffects() => ResetVariables();
    }
}
