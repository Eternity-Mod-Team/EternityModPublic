using System;
using EternityMod.Graphics.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EternityMod.Content.Projectiles.Ranged
{
    public class EtherealFlamerFire : ModProjectile
    {
        public new string LocalizationCategory => "Projectiles.Ranged";

        public override string Texture => "EternityMod/Textures/Invisible";

        public ref float Timer => ref Projectile.ai[0];

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 3;
            Projectile.MaxUpdates = 7;
            Projectile.timeLeft = 150;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 8;
        }

        public override void AI()
        {
            float lifetimeInterpolant = Timer / 150;
            float particleScale = MathHelper.Lerp(0.65f, 1.2f, MathF.Pow(lifetimeInterpolant, 0.55f));
            float opacity = Utils.GetLerpValue(0.96f, 0.75f, lifetimeInterpolant, true);

            Color fireColor = Color.Lerp(Color.DarkGreen, Color.Lime, 2);

            Lighting.AddLight(Projectile.Center, fireColor.ToVector3() * opacity);

            var particle = new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.1f + Main.rand.NextVector2Circular(0.6f, 0.6f), fireColor, 30, particleScale, opacity, 0.05f, true, 0f, true);
            GeneralParticleHandler.SpawnParticle(particle);

            Timer++;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 180);
            target.AddBuff(BuffID.OnFire, 60);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                target.AddBuff(BuffID.OnFire3, 180);
                target.AddBuff(BuffID.OnFire, 60);
            }
        }
    }
}
