using System;
using EternityMod.Common.Systems;
using EternityMod.Common.Systems.Overriding;
using EternityMod.Content.Events;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

        public static void HideFromBestiary(this ModNPC n)
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(n.Type, value);
        }

        public static T BehaviorOverride<T>(this NPC npc) where T : NPCBehaviorOverride
        {
            var container = NPCBehaviorOverride.BehaviorOverrideSet[npc.type];
            if (container is not null && container.BehaviorOverride is T t)
                return t;

            return null;
        }

        public static string GetNPCNameFromID(int id)
        {
            if (id < NPCID.Count)
                return id.ToString();

            return NPCLoader.GetNPC(id).FullName;
        }

        public static string GetNPCFullNameFromID(int id)
        {
            if (id < NPCID.Count)
                return NPC.GetFullnameByID(id);

            return NPCLoader.GetNPC(id).DisplayName.Value;
        }

        public static int GetNPCIDFromName(string name)
        {
            if (int.TryParse(name, out int id))
                return id;

            string[] splitName = name.Split('/');
            if (ModContent.TryFind(splitName[0], splitName[1], out ModNPC modNpc))
                return modNpc.Type;

            return NPCID.None;
        }

        /// <summary>
        /// Returns whether there is a boss currently alive or not.
        /// </summary>
        public static Tuple<bool, int> IsThereABoss()
        {
            bool bossExists = false;
            int bossID = -1;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.boss)
                    bossExists = true;
                bossID = npc.type;
            }
            return Tuple.Create(bossExists, bossID);
        }
    }
}
