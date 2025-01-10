//using System.IO;
//using EternityMod.Events;
//using EternityMod.Systems;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace EternityMod.NPCs.Bosses.Wyrmwood
//{
//    public class WyrmwoodHead : ModNPC
//    {
//        public enum WyrmwoodState
//        {

//        }

//        public override void SetStaticDefaults()
//        {
//            NPCID.Sets.MPAllowedEnemies[Type] = true;
//        }

//        public override void SetDefaults()
//        {
//            NPC.width = 100;
//            NPC.height = 120;

//            NPC.LifeMaxByMode(85000, 97500, 125000, 2425000);
//            NPC.damage = 100;
//            NPC.defense = 15;
//            NPC.knockBackResist = 0f;

//            NPC.aiStyle = -1;
//            AIType = -1;

//            NPC.value = Item.buyPrice(0, 40, 0, 0);
//            NPC.behindTiles = true;
//            NPC.chaseable = false;
//            NPC.noGravity = true;
//            NPC.noTileCollide = true;
//            NPC.netAlways = true;

//            NPC.HitSound = SoundID.NPCHit1;
//            NPC.DeathSound = SoundID.NPCDeath1;
//            Music = MusicID.Boss2;

//            if (BossRushEvent.Active)
//                NPC.scale *= 1.25f;
//            else if (DifficultyModeSystem.BloodbathMode)
//                NPC.scale *= 1.2f;
//            else if (DifficultyModeSystem.DeathMode)
//                NPC.scale *= 1.15f;
//            else if (Main.expertMode)
//                NPC.scale *= 1.1f;
//            if (Main.getGoodWorld)
//                NPC.scale *= 1.25f;
//        }

//        public override void SendExtraAI(BinaryWriter writer)
//        {
//            writer.Write(NPC.chaseable);
//            writer.Write(NPC.localAI[0]);
//            writer.Write(NPC.localAI[1]);
//            writer.Write(NPC.localAI[2]);
//            writer.Write(NPC.npcSlots);
//            for (int i = 0; i < 100; i++)
//                writer.Write(NPC.Eternity().ExtraAI[i]);
//        }

//        public override void ReceiveExtraAI(BinaryReader reader)
//        {
//            NPC.chaseable = reader.ReadBoolean();
//            NPC.localAI[0] = reader.ReadSingle();
//            NPC.localAI[1] = reader.ReadSingle();
//            NPC.localAI[2] = reader.ReadSingle();
//            NPC.npcSlots = reader.ReadSingle();
//            for (int i = 0; i < 100; i++)
//                NPC.Eternity().ExtraAI[i] = reader.ReadSingle();
//        }

//        public override void AI()
//        {
            
//        }

//        public override void BossLoot(ref string name, ref int potionType)
//        {
//            potionType = ItemID.GreaterHealingPotion;
//        }

//        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
//        {
//            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * balance * bossAdjustment);
//            NPC.damage = (int)(NPC.damage * 1.1f);
//        }

//        public override bool CheckActive() => false;
//    }
//}
