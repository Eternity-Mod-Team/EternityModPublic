using EternityMod.Graphics.Particles;
using Terraria;
using Terraria.ModLoader;

namespace EternityMod.Common.Systems
{
    public class EntityUpdateSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if (!Main.dedServ)
                GeneralParticleHandler.Update();
        }
    }
}
