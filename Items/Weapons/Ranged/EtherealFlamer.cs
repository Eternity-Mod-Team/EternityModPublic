//using EternityMod.Projectiles.Ranged;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace EternityMod.Items.Weapons.Ranged
//{
//    public class EtherealFlamer : ModItem
//    {
//        public new string LocalizationCategory => "Items.Weapons.Ranged";

//        public override void SetDefaults()
//        {
//            Item.width = 28;
//            Item.height = 30;
//            Item.DamageType = DamageClass.Ranged;
//            Item.damage = 10;
//            Item.crit = 8;
//            Item.knockBack = 2.75f;
//            Item.useTime = 4;
//            Item.useAnimation = 4;
//            Item.useAmmo = ItemID.Gel;
//            Item.autoReuse = true;
//            Item.noMelee = true;
//            Item.channel = true;
//            Item.useStyle = ItemUseStyleID.Shoot;
//            Item.UseSound = SoundID.Item1;
//            Item.rare = ItemRarityID.Green;
//            Item.value = Item.sellPrice(0, 0, 15);
//            Item.shoot = ModContent.ProjectileType<EtherealFlamerHoldout>();
//            Item.shootSpeed = 4.5f;
//        }

//        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

//        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() >= 0.65f;
//    }
//}
