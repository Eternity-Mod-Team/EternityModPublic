//using System;
//using EternityMod.Graphics.Particles;
//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace EternityMod.Projectiles.Ranged
//{
//    public class EtherealFlamerFire : ModProjectile
//    {
//        public override string Texture => "EternityMod/Assets/Textures/Invisible";

//        public ref float Timer => ref Projectile.ai[0];

//        public override void SetDefaults()
//        {
//            Projectile.width = Projectile.height = 32;
//            Projectile.friendly = true;
//            Projectile.tileCollide = false;
//            Projectile.ignoreWater = true;
//            Projectile.DamageType = DamageClass.Ranged;
//            Projectile.penetrate = 3;
//            Projectile.MaxUpdates = 7;
//            Projectile.timeLeft = 175;
//            Projectile.usesIDStaticNPCImmunity = true;
//            Projectile.idStaticNPCHitCooldown = 8;
//        }

//        public override void AI()
//        {
//            Timer++;

//            Lighting.AddLight(Projectile.Center, Projectile.Opacity * 0.3f, Projectile.Opacity * 0.3f, Projectile.Opacity * 0.03f);
//        }

//        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
//        {
//            target.AddBuff(BuffID.OnFire3, 180);
//            target.AddBuff(BuffID.OnFire, 120);
//        }

//        public override void OnHitPlayer(Player target, Player.HurtInfo info)
//        {
//            if (info.PvP)
//            {
//                target.AddBuff(BuffID.OnFire3, 180);
//                target.AddBuff(BuffID.OnFire, 120);
//            }
//        }
//    }
//}
