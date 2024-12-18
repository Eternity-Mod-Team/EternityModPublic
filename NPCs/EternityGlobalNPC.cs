using EternityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EternityMod.NPCs
{
    public partial class EternityGlobalNPC : GlobalNPC
    {
        public float DR { get; set; } = 0f;

        public int KillTime { get; set; } = 0;

        // AI
        internal const int ExtraAIMod = 100;
        public float[] ExtraAI = new float[ExtraAIMod];
        internal bool[] UsedAI = new bool[ExtraAIMod];
        public bool CurrentlyEnraged;
        public int AITimer = 0;

        // Buffs and Debuffs
        public bool eCurse = false;

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
            myClone.CurrentlyEnraged = CurrentlyEnraged;
            myClone.AITimer = AITimer;

            myClone.eCurse = eCurse;

            return myClone;
        }

        public override void SetDefaults(NPC npc)
        {
            for (int i = 0; i < ExtraAIMod; i++)
                ExtraAI[i] = 0f;

            CurrentlyEnraged = false;
        }

        internal void ResetVariables()
        {
            eCurse = false;
        }

        public override void ResetEffects(NPC npc)
        {
            ResetVariables();
        }

        public override void PostAI(NPC npc)
        {
            for (int i = 0; i < ExtraAIMod; i++)
            {
                if (ExtraAI[i] != 0f)
                    UsedAI[i] = true;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (eCurse)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, ModContent.DustType<EtherealCurseDust>(), npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                        Main.dust[dust].noGravity = false;
                }
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (eCurse)
            {
                if (npc.lifeRegen > 0)
                    npc.lifeRegen = 0;
                npc.lifeRegen -= 3;
                if (damage > 3)
                    damage = 3;
            }
        }
    }
}
