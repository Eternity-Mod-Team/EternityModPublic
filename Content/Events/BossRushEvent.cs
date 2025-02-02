using Terraria.ModLoader;

namespace EternityMod.Content.Events
{
    public class BossRushEvent : ModSystem
    {
        public static bool Active = false;

        public override void OnModLoad()
        {
            Active = false;
        }

        public override void OnModUnload()
        {
            Active = false;
        }
    }
}
