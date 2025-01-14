using EternityMod.Dusts;
using EternityMod.NPCs.Bosses.Moomag;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EternityMod.NPCs
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
        internal const int ExtraAIMod = 100;
        public float[] ExtraAI = new float[ExtraAIMod];
        internal bool[] UsedAI = new bool[ExtraAIMod];
        public int AITimer = 0;

        // General behavior variables.
        public bool CurrentlyEnraged;
        public bool ShouldFallThroughPlatforms;

        // Debuffs.
        public int eCurse = 0;

        // Stats.
        public bool bossCanBeKnockedBack = false;
        public const int knockbackResistanceMin = 180;
        public int knockbackResistanceTimer = 0;

        // Global whoAmI variables.
        public static int Wyrmwood = -1;
        public static int Moomag = -1;
        #endregion

        #region Instance and Cloning
        public override bool InstancePerEntity => true;

        public override GlobalNPC Clone(NPC npc, NPC npcClone)
        {
            EternityGlobalNPC myClone = (EternityGlobalNPC)base.Clone(npc, npcClone);

            myClone.DR = DR;

            myClone.KillTime = KillTime;

            myClone.ExtraAI = new float[ExtraAIMod];
            for (int i = 0; i < ExtraAIMod; ++i)
                myClone.ExtraAI[i] = ExtraAI[i];
            myClone.UsedAI = new bool[ExtraAIMod];
            myClone.AITimer = AITimer;

            myClone.CurrentlyEnraged = CurrentlyEnraged;
            myClone.ShouldFallThroughPlatforms = ShouldFallThroughPlatforms;

            myClone.eCurse = eCurse;

            myClone.bossCanBeKnockedBack = bossCanBeKnockedBack;
            myClone.knockbackResistanceTimer = knockbackResistanceTimer;

            return myClone;
        }
        #endregion

        #region Reset Effects
        public override void ResetEffects(NPC npc)
        {
            void ResetSavedIndex(ref int type, int type1, int type2 = -1)
            {
                if (type >= 0)
                {
                    if (!Main.npc[type].active)
                    {
                        type = -1;
                    }
                    else if (type2 == -1)
                    {
                        if (Main.npc[type].type != type1)
                            type = -1;
                    }
                    else
                    {
                        if (Main.npc[type].type != type1 && Main.npc[type].type != type2)
                            type = -1;
                    }
                }
            }

            //ResetSavedIndex(ref Wyrmwood, NPCType<WyrmwoodHead>());
            ResetSavedIndex(ref Moomag, NPCType<MoomagHead>());
        }
        #endregion

        #region Life Regen
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (eCurse > 0)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 3;
                if (damage > 3)
                    damage = 3;
            }
        }
        #endregion

        #region Set Defaults
        public override void SetStaticDefaults()
        {
            NPCID.Sets.TrailingMode[NPCID.Plantera] = 1;
        }

        public override void SetDefaults(NPC npc)
        {
            for (int i = 0; i < ExtraAIMod; i++)
                ExtraAI[i] = 0f;

            CurrentlyEnraged = false;
        }
        #endregion

        #region AI
        public override void PostAI(NPC npc)
        {
            for (int i = 0; i < ExtraAIMod; i++)
            {
                if (ExtraAI[i] != 0f)
                    UsedAI[i] = true;
            }
        }
        #endregion

        #region Draw Effects
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (eCurse > 0)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, DustType<EtherealCurseDust>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                        Main.dust[dust].noGravity = false;
                }
            }
        }
        #endregion
    }
}
