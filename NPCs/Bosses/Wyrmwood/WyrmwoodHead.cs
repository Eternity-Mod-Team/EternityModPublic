//using System.Collections.Generic;
//using System.IO;
//using EternityMod.Events;
//using EternityMod.Systems;
//using Microsoft.Xna.Framework;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace EternityMod.NPCs.Bosses.Wyrmwood
//{
//    public class WyrmwoodHead : ModNPC
//    {
//        #region Variables and Enumerations

//        public enum WyrmwoodState
//        {
//            SummonAnimation,
            
//            // Phase one.
//            ChargeAndBreatheFire,
//            CircleAroundTarget,
//            ScarletExplosions,
            
//            // Phase two.
//            DeathraySpin,
//            OrbitingBloodOrbs,
//            ScarletPlasma,
            
//            // Desperation phase.
//            PhantasmalNightmare,
//            BrimstoneVortex,

//            DeathAnimation
//        }

//        public ref float State => ref NPC.ai[0];
//        public ref float Timer => ref NPC.ai[1];
//        public ref float Initialized => ref NPC.ai[2];

//        public float LifeRatio => NPC.life / (float)NPC.lifeMax;

//        public const float Phase2LifeRatio = 0.75f;
//        public const float Phase3LifeRatio = 0.35f;

//        public const int BodySegmentCount = 65;

//        public Player Target => Main.player[NPC.target];

//        #endregion

//        public override void SetStaticDefaults()
//        {
//            NPCID.Sets.MPAllowedEnemies[Type] = true;
//        }

//        public override void SetDefaults()
//        {
//            NPC.width = 100;
//            NPC.height = 120;

//            NPC.LifeMaxByMode(85000, 97500, 125000, 2425000);
//            NPC.damage = 170;
//            NPC.defense = 15;
//            NPC.knockBackResist = 0f;

//            NPC.aiStyle = -1;
//            AIType = -1;

//            NPC.value = Item.buyPrice(0, 35);
//            NPC.behindTiles = true;
//            NPC.chaseable = false;
//            NPC.noGravity = true;
//            NPC.noTileCollide = true;
//            NPC.netAlways = true;

//            NPC.HitSound = SoundID.NPCHit1;
//            NPC.DeathSound = SoundID.NPCDeath1;

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
//            // Set the global whoAmI index.
//            EternityGlobalNPC.Wyrmwood = NPC.whoAmI;
            
//            // Spawn the worm segments.
//            if (Initialized == 0f)
//            {
//                NPC.Center = Target.Center + Vector2.UnitY * 2400f;
//                SpawnSegments(NPC);
//                Initialized = 1f;
//                NPC.netUpdate = true;
//            }

//            // Find a target.
//            NPC.TargetClosest();

//            // Despawn if all targets are dead or if it's daytime.
//            if (!Target.active || Target.dead || Main.dayTime)
//            {
//                NPC.TargetClosest(false);

//                NPC.velocity.X *= 0.98f;
//                NPC.velocity.Y += 0.5f;

//                if (NPC.timeLeft > 180)
//                    NPC.timeLeft = 180;

//                if (!NPC.WithinRange(Target.Center, 2400f))
//                    NPC.active = false;

//                NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

//                return;
//            }

//            Timer++;

//            switch ((WyrmwoodState)(int)State)
//            {
//                case WyrmwoodState.SummonAnimation:

//                    break;
//                case WyrmwoodState.ChargeAndBreatheFire:
//                    break;
//            }
//        }

//        public void SummonAnimation()
//        {

//        }

//        public void ChargeAndBreatheFire()
//        {

//        }

//        public void SpawnSegments(NPC head)
//        {
//            if (Main.netMode == NetmodeID.MultiplayerClient)
//                return;

//            int previousSegmentIndex = head.whoAmI;
//            for (int i = 0; i < BodySegmentCount; i++)
//            {
//                int newSegment;
//                if (i is >= 0 and < (int)(BodySegmentCount - 1f))
//                    newSegment = NPC.NewNPC(head.GetSource_FromAI(), (int)head.Center.X, (int)head.Center.Y, NPCID.TheDestroyerBody, head.whoAmI);
//                else
//                    newSegment = NPC.NewNPC(head.GetSource_FromAI(), (int)head.Center.X, (int)head.Center.Y, NPCID.TheDestroyerTail, head.whoAmI);

//                Main.npc[newSegment].realLife = head.whoAmI;

//                Main.npc[newSegment].ai[1] = previousSegmentIndex;
//                Main.npc[previousSegmentIndex].ai[0] = newSegment;

//                Main.npc[newSegment].Eternity().ExtraAI[0] = i;
//                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, newSegment, 0f, 0f, 0f, 0);

//                previousSegmentIndex = newSegment;
//            }
//        }

//        public void SelectNextAttack()
//        {
//            WyrmwoodState oldState = (WyrmwoodState)(int)State;
//            List<WyrmwoodState> potentialAttacks =
//            [
//                WyrmwoodState.ChargeAndBreatheFire,
//                WyrmwoodState.CircleAroundTarget,
//                WyrmwoodState.ScarletExplosions,
//            ];

//            if (LifeRatio <= Phase2LifeRatio)
//            {
//                potentialAttacks.Add(WyrmwoodState.DeathraySpin);
//                potentialAttacks.Add(WyrmwoodState.OrbitingBloodOrbs);
//                potentialAttacks.Add(WyrmwoodState.ScarletPlasma);
//            }

//            for (int i = 0; i < 5; i++)
//                NPC.Eternity().ExtraAI[i] = 0f;

//            do
//                State = (int)Main.rand.Next(potentialAttacks);
//            while ((int)oldState == (int)State && potentialAttacks.Count >= 2);

//            if (oldState == WyrmwoodState.SummonAnimation)
//                State = (int)WyrmwoodState.ChargeAndBreatheFire;

//            Timer = 0f;
//            NPC.netUpdate = true;
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
