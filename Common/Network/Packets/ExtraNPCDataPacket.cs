using System.IO;
using EternityMod.Systems.Overriding;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static EternityMod.Network.PacketManager;

namespace EternityMod.Network.Packets
{
    public class ExtraNPCDataPacket : BaseEternityPacket
    {
        // General NPC sync packets (which should be the only source of these packets) only are fired from the server, and the context is hard to get
        // back once it's used on the server.
        public override bool ResendFromServer => false;

        public override void Write(ModPacket packet, params object[] context)
        {
            // Don't send anything if the NPC is invalid.
            if (context.Length <= 0 || context[0] is not NPC npc || !npc.active)
                return;

            int totalSlotsInUse = npc.Eternity().TotalAISlotsInUse;
            packet.Write(npc.whoAmI);
            packet.Write(npc.realLife);
            packet.Write(totalSlotsInUse);
            packet.Write(npc.Eternity().TotalPlayersAtStart ?? 1);
            packet.Write(npc.Eternity().Arena.X);
            packet.Write(npc.Eternity().Arena.Y);
            packet.Write(npc.Eternity().Arena.Width);
            packet.Write(npc.Eternity().Arena.Height);

            for (var i = 0; i < npc.Eternity().ExtraAI.Length; i++)
            {
                if (!npc.Eternity().HasAssociatedAIBeenUsed[i])
                    continue;

                packet.Write(i);
                packet.Write(npc.Eternity().ExtraAI[i]);
            }

            if (EternityMod.CanUseCustomAIs)
                npc.BehaviorOverride<NPCBehaviorOverride>()?.SendExtraData(npc, packet);
        }

        public override void Read(BinaryReader reader)
        {
            var npcIndex = reader.ReadInt32();
            var realLife = reader.ReadInt32();
            var totalUniqueAIIndicesUsed = reader.ReadInt32();
            var totalPlayersAtStart = reader.ReadInt32();
            var indicesUsed = new int[totalUniqueAIIndicesUsed];
            var aiValues = new float[totalUniqueAIIndicesUsed];
            Rectangle arenaRectangle = new(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

            for (var i = 0; i < totalUniqueAIIndicesUsed; i++)
            {
                indicesUsed[i] = reader.ReadInt32();
                aiValues[i] = reader.ReadSingle();
            }

            NPCSyncInformation syncInformation = new()
            {
                NPCIndex = npcIndex,
                CachedRealLife = realLife,
                TotalUniqueIndicesUsed = totalUniqueAIIndicesUsed,
                TotalPlayersAtStart = totalPlayersAtStart,
                ExtraAIIndicesUsed = indicesUsed,
                ExtraAIValues = aiValues,
                ArenaRectangle = arenaRectangle
            };

            if (!syncInformation.TryToApplyToNPC())
                PendingNPCSyncs.Add(syncInformation);

            if (EternityMod.CanUseCustomAIs)
                Main.npc[npcIndex].BehaviorOverride<NPCBehaviorOverride>()?.ReceiveExtraData(Main.npc[npcIndex], reader);
        }
    }
}
