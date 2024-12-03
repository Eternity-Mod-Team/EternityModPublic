//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.Audio;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace EternityMod.Projectiles.Ranged
//{
//    public class EtherealFlamerHoldout : ModProjectile
//    {
//        public Player Owner => Main.player[Projectile.owner];
//        public ref float Time => ref Projectile.ai[0];

//        public override string Texture => "EternityMod/Items/Weapons/Ranged/EtherealFlamer";

//        public override void SetDefaults()
//        {
//            Projectile.width = 28;
//            Projectile.height = 30;
//            Projectile.friendly = true;
//            Projectile.DamageType = DamageClass.Ranged;
//            Projectile.tileCollide = false;
//            Projectile.ignoreWater = true;
//            Projectile.netImportant = true;
//            Projectile.timeLeft = 7200;
//            Projectile.penetrate = -1;
//        }

//        public override void AI()
//        {
//            Projectile.Center = Owner.MountedCenter;

//            if (!Owner.channel || Owner.dead || !Owner.active || Owner.noItems || Owner.CCed || heldItem is null)
//            {
//                Projectile.Kill();
//                return;
//            }

//            Time++;
//        }

//        public override void OnKill(int timeLeft)
//        {
//            SoundEngine.PlaySound(SoundID.Item34 with { Volume = 0.85f }, Projectile.Center);
//        }

//        public override bool? CanDamage() => false;
//    }
//}
