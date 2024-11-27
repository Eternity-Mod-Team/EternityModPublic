using EternityMod.Events;
using EternityMod.Systems;
using Terraria;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        public static void LifeMaxByMode(this NPC npc, int normal, int? death = null, int? bloodbath = null, int? bossRush = null)
        {
            npc.lifeMax = normal;
            if (bossRush.HasValue && BossRushEvent.Active)
                npc.lifeMax = bossRush.Value;
            else if (bloodbath.HasValue && DifficultyModeSystem.BloodbathMode)
                npc.lifeMax = bloodbath.Value;
            else if (death.HasValue && DifficultyModeSystem.DeathMode)
                npc.lifeMax = death.Value;
        }
    }
}