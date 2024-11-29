using Terraria.ID;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EternityMod.NPCs.Bosses.Moomag
{
    public class MoomagBodyAlt : ModNPC
    {
        public override LocalizedText DisplayName => EternityUtils.GetText("NPCs.MoomagHead.DisplayName");

        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
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

            NPC.behindTiles = true;
            NPC.chaseable = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.dontCountMe = true;
            NPC.netAlways = true;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            if (Main.getGoodWorld)
                NPC.scale *= 1.25f;
        }

        public override void AI()
        {
            if (!Main.npc.IndexInRange((int)NPC.ai[1]) || !Main.npc[(int)NPC.ai[1]].active)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.active = false;
                NPC.netUpdate = true;
                return;
            }

            NPC previousSegment = Main.npc[(int)NPC.ai[1]];
            NPC.target = previousSegment.target;
            NPC.defense = previousSegment.defense;
            NPC.dontTakeDamage = previousSegment.dontTakeDamage;

            Vector2 dir = previousSegment.Center - NPC.Center;
            if (previousSegment.rotation != NPC.rotation)
            {
                dir = dir.RotatedBy(MathHelper.WrapAngle(previousSegment.rotation - NPC.rotation) * 0.08f);
                dir = dir.MoveTowards((previousSegment.rotation - NPC.rotation).ToRotationVector2(), 1f);
            }

            NPC.rotation = dir.ToRotation() + MathHelper.PiOver2;
            NPC.Center = previousSegment.Center - dir.SafeNormalize(Vector2.Zero) * NPC.scale;
            NPC.spriteDirection = (dir.X > 0).ToDirectionInt();
        }
    }
}
