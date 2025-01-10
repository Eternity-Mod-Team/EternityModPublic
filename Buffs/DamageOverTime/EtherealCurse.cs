using Terraria;
using Terraria.ModLoader;

namespace EternityMod.Buffs.DamageOverTime
{
    public class EtherealCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) => player.Eternity().eCurse = true;

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.Eternity().eCurse < npc.buffTime[buffIndex])
                npc.Eternity().eCurse = npc.buffTime[buffIndex];
            npc.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}
