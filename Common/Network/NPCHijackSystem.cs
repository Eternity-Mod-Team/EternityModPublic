﻿using EternityMod.Common.Network.Packets;
using EternityMod.Common.Systems.Overriding;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EternityMod.Common.Network
{
    public class NPCSyncHijackSystem : ModSystem
    {
        public override bool HijackSendData(int whoAmI, int msgType, int remoteClient, int ignoreClient, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7)
        {
            if (msgType == MessageID.SyncNPC)
            {
                var npc = Main.npc[number];
                if (!npc.active || !NPCBehaviorOverride.Registered(npc.type))
                    return base.HijackSendData(whoAmI, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);

                // Sync extra general information about the NPC.
                PacketManager.SendPacket<ExtraNPCDataPacket>(Main.npc[number]);

                // Have the Twins send a specialized packet to ensure that the attack synchronizer is updated.
                //if (npc.type is NPCID.Retinazer or NPCID.Spazmatism)
                //    PacketManager.SendPacket<TwinsAttackSynchronizerPacket>();
            }
            return base.HijackSendData(whoAmI, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
        }
    }
}
