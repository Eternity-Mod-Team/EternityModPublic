using System.Linq;
using EternityMod.Content.Dusts;
using EternityMod.Content.NPCs.Bosses.Moomag;
using EternityMod.Content.NPCs.Bosses.Wyrmwood;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EternityMod.Common.Globals.NPCs
{
    public partial class EternityGlobalNPC : GlobalNPC
    {
        #region Variables

        /// <summary>
        /// The damage reduction of a specific NPC.
        /// </summary>
        public float DR { get; set; } = 0f;

        public int KillTime { get; set; } = 0;

        // AI variables.
        public const int TotalExtraAISlots = 100;
        public float[] ExtraAI = new float[TotalExtraAISlots];
        internal bool[] HasAssociatedAIBeenUsed = new bool[TotalExtraAISlots];
        internal int TotalAISlotsInUse => HasAssociatedAIBeenUsed.Count(slot => slot);
        public int AITimer = 0;

        // General behavior variables.
        public bool CurrentlyEnraged;
        public bool ShouldFallThroughPlatforms;
        public int? TotalPlayersAtStart;
        public bool DisableNaturalDespawning;
        public bool HasResetHP;
        public Rectangle Arena;

        // Debuff variables.
        public int EtherealCurse = 0;

        // Stat variables.
        public bool BossCanBeKnockedBack = false;
        public int KnockbackResistanceTimer = 0;
        public const int MinKnockbackResistance = 180;

        // Global whoAmI variables.
        public static int Wyrmwood = -1;
        public static int Moomag = -1;

        #endregion Variables

        #region Instance and Cloning

        public override bool InstancePerEntity => true;

        public override GlobalNPC Clone(NPC npc, NPC npcClone)
        {
            EternityGlobalNPC myClone = (EternityGlobalNPC)base.Clone(npc, npcClone);

            myClone.DR = DR;

            myClone.KillTime = KillTime;

            myClone.AITimer = AITimer;

            myClone.CurrentlyEnraged = CurrentlyEnraged;
            myClone.ShouldFallThroughPlatforms = ShouldFallThroughPlatforms;
            myClone.TotalPlayersAtStart = TotalPlayersAtStart;
            myClone.DisableNaturalDespawning = DisableNaturalDespawning;
            myClone.HasResetHP = HasResetHP;

            myClone.EtherealCurse = EtherealCurse;

            myClone.BossCanBeKnockedBack = BossCanBeKnockedBack;
            myClone.KnockbackResistanceTimer = KnockbackResistanceTimer;

            return myClone;
        }

        #endregion Instance and Cloning

        #region Set Defaults

        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPCID.Plantera] = 1;
        }

        public override void SetDefaults(NPC npc)
        {
            for (int i = 0; i < ExtraAI.Length; i++)
                ExtraAI[i] = 0f;

            CurrentlyEnraged = false;

            // Thanks Redigit.
            if (npc.type == NPCID.WallofFleshEye)
                npc.netAlways = true;
        }

        #endregion Set Defaults

        #region AI

        public override void PostAI(NPC npc)
        {
            for (int i = 0; i < ExtraAI.Length; i++)
            {
                if (ExtraAI[i] != 0f)
                    HasAssociatedAIBeenUsed[i] = true;
            }

            // Debuff decrements.
            if (EtherealCurse > 0)
                EtherealCurse--;
        }

        #endregion AI

        #region Life Regeneration

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (EtherealCurse > 0)
            {
                npc.lifeRegen -= 3;
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;

                if (damage > 3)
                    damage = 3;
            }
        }

        #endregion Life Regeneration

        #region Draw Effects

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (EtherealCurse > 0)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustType<EtherealCurseDust>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1f);
                    
                    Main.dust[dust].noGravity = true;
                    if (Main.rand.NextBool(4))
                        Main.dust[dust].noGravity = false;
                    
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }
            }
        }

        #endregion Draw Effects

        #region Reset Effects

        public override void ResetEffects(NPC npc)
        {
            static void ResetSavedIndex(ref int index, int type, int type2 = -1)
            {
                if (index >= 0)
                {
                    // If the index is not active, reset the index.
                    if (!Main.npc[index].active)
                        index = -1;

                    // Otherwise, if only this is not found...
                    else if (type2 == -1)
                    {
                        // If the index is not the correct type, reset the index.
                        if (Main.npc[index].type != type)
                            index = -1;
                    }
                    else
                    {
                        if (Main.npc[type].type != type && Main.npc[index].type != type2)
                            index = -1;
                    }
                }
            }

            ResetSavedIndex(ref Wyrmwood, NPCType<WyrmwoodHead>());
            ResetSavedIndex(ref Moomag, NPCType<MoomagHead>());
        }

        #endregion Reset Effects
    }
}
