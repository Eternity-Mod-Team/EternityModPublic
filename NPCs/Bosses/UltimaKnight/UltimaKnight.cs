//using Terraria.ID;
//using Terraria;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;
//using Terraria.GameContent.Events;
//using Terraria.Graphics.Effects;
//using EternityMod.Projectiles.Boss.UltimaKnight;

//namespace EternityMod.NPCs.Bosses.UltimaKnight
//{
//    public class UltimaKnight : ModNPC
//    {
//        public ref float State => ref NPC.ai[0];
//        public ref float GeneralTimer => ref NPC.ai[1];
//        public ref float AttackTimer => ref NPC.ai[2];
//        public ref float Phase2Check => ref NPC.ai[3];

//        public Player Target => Main.player[NPC.target];

//        public override void SetStaticDefaults()
//        {
//            Main.npcFrameCount[Type] = 1;

//            NPCID.Sets.TrailCacheLength[Type] = 10;
//            NPCID.Sets.TrailingMode[Type] = 0;
//            NPCID.Sets.MPAllowedEnemies[Type] = true;
//            NPCID.Sets.BossBestiaryPriority.Add(Type);
//        }

//        public override void SetDefaults()
//        {
//            NPC.width = 40;
//            NPC.height = 70;

//            NPC.LifeMaxByMode(2800000, 3000000, 3150000, 6535000);
//            NPC.damage = 300;
//            NPC.defense = 100;
//            NPC.knockBackResist = 0f;
//            NPC.npcSlots = 15f;

//            NPC.aiStyle = -1;
//            AIType = -1;

//            NPC.boss = true;
//            NPC.noGravity = true;
//            NPC.noTileCollide = true;
//            NPC.lavaImmune = true;
//            NPC.value = Item.buyPrice(2, 25);

//            NPC.HitSound = SoundID.NPCHit7;
//            NPC.DeathSound = SoundID.NPCDeath1;

//            Music = MusicID.LunarBoss;
//        }

//        public override void AI()
//        {
//            NPC.direction = NPC.spriteDirection = Target.Center.X - NPC.Center.X > 0f ? 1 : -1;

//            if (NPC.target < 0 || NPC.target >= 255 || Target.dead || !Target.active)
//                NPC.TargetClosest();

//            if (Target.dead)
//                NPC.active = false;

//            Main.StopRain();
//            for (int i = 0; i < Main.maxRain; i++)
//                Main.rain[i].active = false;

//            if (Main.netMode != NetmodeID.MultiplayerClient)
//            {
//                Sandstorm.StopSandstorm();

//                if (Main.netMode != NetmodeID.Server)
//                    Filters.Scene["Graveyard"].Deactivate();
//            }

//            if (NPC.HasPlayerTarget)
//                Target.wingTime = Target.wingTimeMax;

//            for (int i = 0; i < Main.maxPlayers; i++)
//            {
//                Player p = Main.player[i];
//                if (!p.active || p.dead)
//                    continue;

//                p.moonLeech = true;
//            }

//            float toPlayer = Utils.ToRotation(Target.Center - NPC.Center);

//            if (Phase2Check == 0f)
//            {
//                if (State == 0f)
//                {
//                    AttackTimer++;
//                    Movement(Target.Center + new Vector2((NPC.direction == 1) ? -400 : 400, 0f), 15f, 0.1f);
//                    for (int i = 0; i < 5; i++)
//                    {
//                        if (AttackTimer == 70 + 30 * i)
//                        {
//                            float rand = Utils.NextFloat(Main.rand, 0f, 3f);
//                            for (int j = 0; j < 6; j++)
//                            {
//                                float r = rand + j * MathHelper.Pi / 1.5f + toPlayer;
//                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<UltimaSphere>(), NPC.damage / 2, 1f, NPC.target, 1f, r);
//                            }
//                        }
//                    }
//                }
//            }

//            void ChangeAttackP1()
//            {
//                GeneralTimer = 0f;
//                AttackTimer = 0f;

//                if (State >= 4)
//                    State = 0;
//                else
//                    State++;
//            }

//            void ChangeAttackP2()
//            {
//                if (NPC.dontTakeDamage)
//                    NPC.dontTakeDamage = false;

//                GeneralTimer = 0f;
//                AttackTimer = 0f;

//                if (State >= 14)
//                    State = 5;
//                else
//                    State++;
//            }

//            void Movement(Vector2 pos, float vel, float v)
//            {
//                if (pos != NPC.Center)
//                {
//                    Vector2 toPos = Utils.SafeNormalize(pos - NPC.Center, Vector2.Zero);
//                    NPC.velocity = Vector2.Lerp(NPC.velocity, toPos * vel, v);
//                }
//                if (Vector2.Distance(Target.Center, NPC.Center) < 100f)
//                    NPC.velocity = NPC.velocity + Utils.SafeNormalize(NPC.Center - Target.Center, Vector2.Zero) * 5f;
//            }
//        }

//        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
//        {

//        }
//    }
//}
