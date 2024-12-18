using System.Diagnostics;
using EternityMod.Graphics.Particles;
using EternityMod.ModSupport;
using Terraria;
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
            if (!Main.dedServ)
            {
                GeneralParticleHandler.Load();
            }
        }

        public override void Unload()
        {
            Instance = null;

            if (!Main.dedServ)
            {
                GeneralParticleHandler.Unload();
            }
        }

        public override void PostSetupContent()
        {
            WeakReferenceSupport.Setup();
        }
    }
}
