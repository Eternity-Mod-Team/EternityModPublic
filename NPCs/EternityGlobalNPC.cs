using Terraria;
using Terraria.ModLoader;

namespace EternityMod.NPCs
{
    public partial class EternityGlobalNPC : GlobalNPC
    {
        public float DR { get; set; } = 0f;

        public int KillTime { get; set; } = 0;

        internal const int ExtraAIMod = 100;
        public float[] ExtraAI = new float[ExtraAIMod];
        public int AITimer = 0;

        public override bool InstancePerEntity => true;

        public override GlobalNPC Clone(NPC npc, NPC npcClone)
        {
            EternityGlobalNPC myClone = (EternityGlobalNPC)base.Clone(npc, npcClone);

            myClone.DR = DR;

            myClone.KillTime = KillTime;

            myClone.ExtraAI = new float[ExtraAIMod];
            for (int i = 0; i < ExtraAIMod; ++i)
                myClone.ExtraAI[i] = ExtraAI[i];
            myClone.AITimer = AITimer;

            return myClone;
        }
    }
}