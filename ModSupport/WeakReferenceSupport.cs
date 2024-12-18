using EternityMod.Graphics.Particles;
using Terraria;

namespace EternityMod.ModSupport
{
    internal class WeakReferenceSupport
    {
        public static void Setup()
        {
            if (!Main.dedServ)
            {
                GeneralParticleHandler.LoadModParticleInstances();
            }
        }
    }
}
