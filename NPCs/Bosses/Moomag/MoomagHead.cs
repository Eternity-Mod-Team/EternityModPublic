using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace EternityMod.NPCs.Bosses.Moomag
{
    [AutoloadBossHead]
    public class MoomagHead : ModNPC
    {
        public ref float State => ref NPC.ai[0];
        public ref float Timer => ref NPC.ai[1];

        private const int BodySegmentCount = 60;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 108;
            NPC.height = 100;

            NPC.LifeMaxByMode(85000, 97500, 125000, 2425000);
            NPC.damage = 100;
            NPC.defense = 15;
            NPC.knockBackResist = 0f;

            NPC.aiStyle = -1;
            AIType = -1;

            NPC.value = Item.buyPrice(0, 40, 0, 0);
            NPC.behindTiles = true;
            NPC.chaseable = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            if (Main.getGoodWorld)
                NPC.scale *= 1.25f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type], quickUnlock: true);
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                new BossBestiaryInfoElement(),
                new FlavorTextBestiaryInfoElement("Mods.EternityMod.Bestiary.Moomag")
            });
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
            writer.Write(NPC.localAI[1]);
            writer.Write(NPC.localAI[2]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
            NPC.localAI[1] = reader.ReadSingle();
            NPC.localAI[2] = reader.ReadSingle();
        }

        public override void AI()
        {
            float lifeRatio = NPC.life / (float)NPC.lifeMax;
            bool phase2 = lifeRatio <= 0.75f;
            bool phase3 = lifeRatio <= 0.5f;
            bool phase4 = lifeRatio <= 0.25f;
            Player target = Main.player[NPC.target];

            bool initialized = false;
            if (!initialized)
            {
                NPC.Center = target.Center + Vector2.UnitY * 2500f;
                CreateSegments();
                initialized = true;
                NPC.netUpdate = true;
            }

            NPC.TargetClosest();
            if (NPC.target < 0 || NPC.target >= Main.maxPlayers || target.dead || !target.active || !NPC.WithinRange(target.Center, 4500f))
            {
                NPC.velocity.X *= 0.98f;

                if (NPC.timeLeft > 45)
                    NPC.timeLeft = 45;

                if (!NPC.WithinRange(target.Center, 1200f))
                    NPC.active = false;

                return;
            }

            switch ((int)State)
            {
                case 0:
                    SummonAnimation();
                    break;
            }

            void SummonAnimation()
            {

            }

            void CreateSegments()
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    return;

                int previousSegmentIndex = NPC.whoAmI;
                for (int i = 0; i < BodySegmentCount; i++)
                {
                    int newSegment;
                    if (i is >= 0 and < (int)(BodySegmentCount - 1f))
                    {
                        if (i % 2 == 0)
                            newSegment = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MoomagBodyAlt>(), NPC.whoAmI);
                        else
                            newSegment = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MoomagBody>(), NPC.whoAmI);
                    }
                    else
                        newSegment = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MoomagTail>(), NPC.whoAmI);

                    Main.npc[newSegment].realLife = NPC.whoAmI;
                    Main.npc[newSegment].ai[1] = previousSegmentIndex;
                    Main.npc[previousSegmentIndex].ai[0] = newSegment;
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, newSegment);
                    previousSegmentIndex = newSegment;
                }
            }
        }
    }
}
