using System;
using EternityMod.Common.DataStructures;
using EternityMod.Graphics.Particles;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent.Events;

namespace EternityMod.Common.ILEditing
{
    public partial class ILChanges
    {
        private static void AdditiveDrawing(ILContext il)
        {
            ILCursor cursor = new(il);
            if (!cursor.TryGotoNext(MoveType.After, i => i.MatchCall<MoonlordDeathDrama>("DrawWhite")))
                return;

            cursor.EmitDelegate<Action>(() =>
            {
                Main.spriteBatch.SetBlendState(BlendState.Additive);

                // Draw projectiles.
                foreach (Projectile p in Main.ActiveProjectiles)
                {
                    if (p.ModProjectile is IAdditiveDrawer d)
                        d.AdditiveDraw(Main.spriteBatch);
                }

                // Draw NPCs.
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.ModNPC is IAdditiveDrawer d)
                        d.AdditiveDraw(Main.spriteBatch);
                }

                Main.spriteBatch.SetBlendState(BlendState.AlphaBlend);
            });
        }

        private static void DrawForegroundParticles(On_Main.orig_DrawInfernoRings orig, Main self)
        {
            GeneralParticleHandler.DrawAllParticles(Main.spriteBatch);
            orig(self);
        }
    }
}
