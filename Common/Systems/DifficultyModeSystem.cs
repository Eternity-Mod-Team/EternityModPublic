using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EternityMod.Common.Systems
{
    public class DifficultyModeSystem : ModSystem
    {
        public static bool DeathMode = false;
        public static bool BloodbathMode = false;
        public static bool NightmareMode = false;

        private static void ResetVariables()
        {
            DeathMode = false;
            BloodbathMode = false;
            NightmareMode = false;
        }

        public override void OnWorldLoad()
        {
            ResetVariables();
        }

        public override void OnWorldUnload()
        {
            ResetVariables();
        }

        public override void SaveWorldHeader(TagCompound tag)
        {
            if (DeathMode)
                tag["DeathMode"] = true;
            if (BloodbathMode)
                tag["BloodbathMode"] = true;
            if (NightmareMode)
                tag["NightmareMode"] = true;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            var downed = new List<string>();
            if (DeathMode)
                downed.Add("DeathMode");
            if (BloodbathMode)
                downed.Add("BloodbathMode");
            if (NightmareMode)
                downed.Add("NightmareMode");
            tag["downed"] = downed;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            DeathMode = downed.Contains("DeathMode");
            BloodbathMode = downed.Contains("BloodbathMode");
            NightmareMode = downed.Contains("NightmareMode");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new();
            flags[0] = DeathMode;
            flags[1] = BloodbathMode;
            flags[2] = NightmareMode;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            DeathMode = flags[0];
            BloodbathMode = flags[1];
            NightmareMode = flags[2];
        }
    }
}
