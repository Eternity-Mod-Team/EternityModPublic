using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EternityMod.Content.Reworks.Bosses
{
    public class LunaticCultistOverride : GlobalNPC
    {
        #region Enumerations and Variables

        public override bool InstancePerEntity => true;

        public enum CultistFrameState
        {
            AbsorbEffect,
            Hover,
            RaiseArmsUp,
            HoldArmsOut,
            Laugh,
        }

        // Keep in mind that attacks in separate phases are just additions to phase one.
        public enum CultistState
        {
            SummonAnimation,

            // Phase one.
            PerformRitual,
            SolarBarrage,
            VortexMight,
            NebulaArcanum,
            StardustWill,
            FrostFury,

            // Phase two.
            EnterPhase2,
            AncientDoom,

            // Phase three.
            EnterPhase3,
            SpinningDeathray,
            PillarThrow,

            // Desperation phase.
            DesperationAttack,

            DeathAnimation
        }

        public const float Phase2LifeRatio = 0.65f;
        public const float Phase3LifeRatio = 0.3f;

        #endregion Enumerations and Variables

        #region AI

        public override bool PreAI(NPC npc)
        {
            CultistState state = (CultistState)(int)npc.ai[0];
            ref float timer = ref npc.ai[1];
            ref float phase = ref npc.ai[2];
            ref float transitionTimer = ref npc.ai[3];
            ref float frameType = ref npc.localAI[0];
            ref float desperationState = ref npc.Eternity().ExtraAI[0];
            ref float deathTimer = ref npc.Eternity().ExtraAI[1];

            float lifeRatio = npc.life / (float)npc.lifeMax;
            bool phase2 = lifeRatio <= Phase2LifeRatio;
            bool phase3 = lifeRatio <= Phase3LifeRatio;
            bool dying = desperationState == 2f;

            Player target = Main.player[npc.target];

            // Find the nearest target.
            npc.TargetClosest();

            // Despawn if all remaining targets are dead.
            if (!Main.player.IndexInRange(npc.target) || target.dead || !target.active)
            {
                npc.TargetClosest();
                if (!Main.player.IndexInRange(npc.target) || target.dead || !target.active)
                {
                    Despawn(npc);
                    return false;
                }
            }

            // Perform the desperation attack before death.
            if (desperationState == 1f)
                state = CultistState.DesperationAttack;

            // Perform the death animation after the desperation attack.
            if (dying)
            {
                DeathAnimation(npc, ref deathTimer);
                npc.dontTakeDamage = true;
                frameType = (int)CultistFrameState.Laugh;
                return false;
            }

            // Disable contact damage.
            npc.damage = 0;

            return false;
        }

        public static void Despawn(NPC npc)
        {
            npc.velocity = Vector2.Zero;
            npc.dontTakeDamage = true;

            if (npc.timeLeft > 25)
                npc.timeLeft = 25;

            npc.alpha = Utils.Clamp(npc.alpha + 40, 0, 255);
            if (npc.alpha >= 255)
            {
                npc.active = false;
                npc.netUpdate = true;
            }
        }

        public static void DeathAnimation(NPC npc, ref float deathTimer)
        {

        }

        #endregion AI
    }
}
