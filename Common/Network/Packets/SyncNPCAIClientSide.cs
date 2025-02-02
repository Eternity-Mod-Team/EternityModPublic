using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace EternityMod.Common.Network.Packets
{
    public class SyncNPCAIClientSide : BaseEternityPacket
    {
        public override void Write(ModPacket packet, params object[] context)
        {
            var npc = Main.npc[(int)context[0]];
            packet.Write((int)context[0]);
            for (var i = 0; i < NPC.maxAI; i++)
                packet.Write(npc.ai[i]);

            new ExtraNPCDataPacket().Write(packet, new[] { Main.npc[(int)context[0]] });
        }

        public override void Read(BinaryReader reader)
        {
            var npc = Main.npc[reader.ReadInt32()];
            for (var i = 0; i < NPC.maxAI; i++)
                npc.ai[i] = reader.ReadSingle();

            new ExtraNPCDataPacket().Read(reader);
            npc.netUpdate = true;
        }
    }
}
