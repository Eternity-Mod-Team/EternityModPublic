using Terraria.ID;
using Terraria.ModLoader;

namespace EternityMod
{
    public static class EternitySets
    {
        public sealed class NPCs : ModSystem
        {
            public static bool[] Zombie { get; private set; }

            public override void PostSetupContent()
            {
                Zombie = NPCID.Sets.Factory.CreateBoolSet(false,
                    NPCID.Zombie,
                    NPCID.ArmedZombie,
                    NPCID.BaldZombie,
                    NPCID.PincushionZombie,
                    NPCID.ArmedZombiePincussion,
                    NPCID.SlimedZombie,
                    NPCID.ArmedZombieSlimed,
                    NPCID.SwampZombie,
                    NPCID.ArmedZombieSwamp,
                    NPCID.TwiggyZombie,
                    NPCID.ArmedZombieTwiggy,
                    NPCID.FemaleZombie,
                    NPCID.ArmedZombieCenx,
                    NPCID.ZombieRaincoat,
                    NPCID.ZombieEskimo,
                    NPCID.ArmedZombieEskimo,
                    NPCID.BigRainZombie,
                    NPCID.SmallRainZombie,
                    NPCID.BigFemaleZombie,
                    NPCID.SmallFemaleZombie,
                    NPCID.BigTwiggyZombie,
                    NPCID.SmallTwiggyZombie,
                    NPCID.BigSwampZombie,
                    NPCID.SmallSwampZombie,
                    NPCID.BigSlimedZombie,
                    NPCID.SmallSlimedZombie,
                    NPCID.BigPincushionZombie,
                    NPCID.SmallPincushionZombie,
                    NPCID.BigBaldZombie,
                    NPCID.SmallBaldZombie,
                    NPCID.BigZombie,
                    NPCID.SmallZombie,
                    NPCID.MaggotZombie
                    );
            }
        }

        public sealed class Projectiles : ModSystem
        {
            public static bool[] Tombstone { get; private set; }

            public override void PostSetupContent()
            {
                Tombstone = ProjectileID.Sets.Factory.CreateBoolSet(false,
                    ProjectileID.Tombstone,
                    ProjectileID.Gravestone,
                    ProjectileID.RichGravestone1,
                    ProjectileID.RichGravestone2,
                    ProjectileID.RichGravestone3,
                    ProjectileID.RichGravestone4,
                    ProjectileID.RichGravestone4,
                    ProjectileID.Headstone,
                    ProjectileID.Obelisk
                    );
            }
        }
    }
}
