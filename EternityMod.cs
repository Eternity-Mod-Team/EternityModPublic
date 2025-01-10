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

        // This is the mod that contains all of Eternity's music.
        internal Mod MusicMod = null;
        internal bool MusicAvailable => MusicMod is not null;

        public EternityMod()
        {
            Debug.Assert(Instance == null);
            Instance = this;
        }

        public override void Load()
        {
            MusicMod = null;
            ModLoader.TryGetMod("EternityModMusic", out MusicMod);

            if (!Main.dedServ)
            {
                GeneralParticleHandler.Load();
            }
        }

        public override void Unload()
        {
            Instance = null;

            MusicMod = null;

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
