using Terraria.ModLoader;

namespace EternityMod
{
	public class EternityMod : Mod
	{
		internal static EternityMod Instance;

        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            Instance = null;
        }
    }
}