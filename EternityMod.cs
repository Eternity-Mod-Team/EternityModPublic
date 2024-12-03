using System.Diagnostics;
using Terraria.ModLoader;

namespace EternityMod
{
    public class EternityMod : Mod
	{
		internal static EternityMod Instance;

        public EternityMod()
        {
            Debug.Assert(Instance == null);
            Instance = this;
        }

        public override void Load()
        {

        }

        public override void Unload()
        {
            Instance = null;
        }
    }
}
