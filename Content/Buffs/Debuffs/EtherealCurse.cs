﻿using Terraria;
using Terraria.ModLoader;

namespace EternityMod.Content.Buffs.Debuffs
{
    public class EtherealCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) => player.Eternity().EtherealCurse = true;

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.Eternity().EtherealCurse < npc.buffTime[buffIndex])
                npc.Eternity().EtherealCurse = npc.buffTime[buffIndex];

            npc.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}
