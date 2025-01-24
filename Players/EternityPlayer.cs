using EternityMod.Events;
using EternityMod.Systems;
using Terraria;
using Terraria.ModLoader;

namespace EternityMod.Players
{
    public partial class EternityPlayer : ModPlayer
    {
        public bool InfiniteFlight = false;

        #region Pre-Update

        public override void PreUpdate()
        {
            // Some bosses, even specific boss attacks, grants the player infinite flight time.
            if (InfiniteFlight)
                Player.wingTime = Player.wingTimeMax;
        }

        #endregion Pre-Update

        #region Nurse Modifications

        public override bool ModifyNurseHeal(NPC nurse, ref int health, ref bool removeDebuffs, ref string chatText)
        {
            if ((DifficultyModeSystem.BloodbathMode || BossRushEvent.Active) && EternityUtils.IsThereABoss().Item1)
            {
                chatText = EternityUtils.GetTextValue("Vanilla.NurseChat.HealNotAllowed");
                return false;
            }

            return true;
        }

        #endregion
    }
}
