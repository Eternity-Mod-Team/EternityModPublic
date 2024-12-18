using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EternityMod.Projectiles.Ranged
{
    public class EtherealFlamerHoldout : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public ref float Timer => ref Projectile.ai[0];

        public new string LocalizationCategory => "Projectiles.Ranged";

        public override string Texture => "EternityMod/Items/Weapons/Ranged/EtherealFlamer";

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.timeLeft = 7200;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Item heldItem = Owner.ActiveItem();
            Projectile.Center = Owner.MountedCenter;
            AdjustPlayerValues();

            if (!Owner.channel || Owner.dead || !Owner.active || Owner.noItems || Owner.CCed || heldItem is null)
            {
                Projectile.Kill();
                return;
            }

            if (Main.myPlayer == Projectile.owner && Timer % heldItem.useTime == heldItem.useTime - 1f && Owner.HasAmmo(heldItem))
            {
                Owner.PickAmmo(heldItem, out _, out float shootSpeed, out int fireDamage, out float knockback, out _);
                Vector2 fireSpawnPosition = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitY) * heldItem.width * 0.5f;
                Vector2 fireShootVelocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * shootSpeed;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), fireSpawnPosition, fireShootVelocity, ModContent.ProjectileType<EtherealFlamerFire>(), fireDamage, knockback, Projectile.owner);
            }

            Timer++;
        }

        public void AdjustPlayerValues()
        {
            Projectile.timeLeft = 2;
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.itemRotation = (Projectile.direction * Projectile.velocity).ToRotation();

            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.velocity = Projectile.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX * Owner.direction);
                if (Projectile.velocity != Projectile.oldVelocity)
                    Projectile.netUpdate = true;
                Projectile.spriteDirection = (Projectile.velocity.X > 0f).ToDirectionInt();
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.spriteDirection == -1)
                Projectile.rotation += MathHelper.Pi;
            Owner.ChangeDir(Projectile.spriteDirection);

            Projectile.Center += Projectile.velocity * 20f;

            float frontArmRotation = Projectile.rotation + Owner.direction * -0.4f;
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, frontArmRotation);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item34 with { Volume = 0.85f }, Projectile.Center);
        }

        public override bool? CanDamage() => false;
    }
}
