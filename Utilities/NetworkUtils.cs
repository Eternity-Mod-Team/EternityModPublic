using Terraria.ID;
using Terraria;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        public static void SyncWorld()
        {
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData);
        }
    }
}
