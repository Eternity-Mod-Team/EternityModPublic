//using EternityMod.Projectiles.Melee;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace EternityMod.Items.Weapons.Melee
//{
//    public class CryogenicBlade : ModItem
//    {
//        public new string LocalizationCategory => "Items.Weapons.Ranged";

//        public override void SetDefaults()
//        {
//            Item.width = 28;
//            Item.height = 30;
//            Item.DamageType = DamageClass.Melee;
//            Item.damage = 40;
//            Item.crit = 15;
//            Item.knockBack = 2f;
//            Item.useTime = 4;
//            Item.useAnimation = 4;
//            Item.useAmmo = ItemID.Gel;
//            Item.autoReuse = true;
//            Item.channel = true;
//            Item.useStyle = ItemUseStyleID.Swing;
//            Item.UseSound = SoundID.Item1;
//            Item.rare = ItemRarityID.Green;
//            Item.value = Item.sellPrice(0, 2, 50);
//            Item.shoot = ModContent.ProjectileType<CryogenicSwing>();
//            Item.shootSpeed = 4.5f;
//        }
//    }
//}
