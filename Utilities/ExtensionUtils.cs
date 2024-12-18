using EternityMod.NPCs;
using EternityMod.Players;
using Microsoft.Xna.Framework;
using Terraria;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        public static EternityGlobalNPC Eternity(this NPC npc) => npc.GetGlobalNPC<EternityGlobalNPC>();

        public static EternityPlayer Eternity(this Player player) => player.GetModPlayer<EternityPlayer>();

        public static Vector2 TurnRight(this Vector2 vec) => new Vector2(-vec.Y, vec.X);

        public static Vector2 TurnLeft(this Vector2 vec) => new Vector2(vec.Y, -vec.X);

        public static bool ZoneForest(this Player player)
        {
            return !player.ZoneJungle
                && !player.ZoneDungeon
                && !player.ZoneCorrupt
                && !player.ZoneCrimson
                && !player.ZoneHallow
                && !player.ZoneSnow
                && !player.ZoneUndergroundDesert
                && !player.ZoneGlowshroom
                && !player.ZoneMeteor
                && !player.ZoneBeach
                && !player.ZoneDesert
                && player.ZoneOverworldHeight;
        }
    }
}
