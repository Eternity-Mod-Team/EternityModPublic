using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EternityMod.Dusts
{
    public class EtherealCurseDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.velocity.X = 0f;
            dust.velocity.Y -= 0.85f;
        }

        public override bool Update(Dust dust)
        {
            dust.rotation += 0.1f * (dust.dustIndex % 2 == 0 ? -1 : 1);
            dust.scale -= 0.05f;
            if (dust.scale < 0.25f)
                dust.active = false;
            Lighting.AddLight(dust.position, Color.Green.ToVector3());
            return false;
        }
    }
}
