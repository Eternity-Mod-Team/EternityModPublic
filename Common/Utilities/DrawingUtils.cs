﻿using System;
using System.Collections.Generic;
using System.Reflection;
using EternityMod.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        internal static Texture2D AuroraTexture
        {
            get
            {
                Main.instance.LoadProjectile(ProjectileID.HallowBossDeathAurora);
                return TextureAssets.Projectile[ProjectileID.HallowBossDeathAurora].Value;
            }
        }

        /// <summary>
        /// Draws a projectile as a series of afterimages. The first of these afterimages is centered on the center of the projectile's hitbox.<br />
        /// This function is guaranteed to draw the projectile itself, even if it has no afterimages and/or the Afterimages config option is turned off.
        /// </summary>
        /// <param name="proj">The projectile to be drawn.</param>
        /// <param name="mode">The type of afterimage drawing code to use. Vanilla Terraria has three options: 0, 1, and 2.</param>
        /// <param name="lightColor">The light color to use for the afterimages.</param>
        /// <param name="typeOneIncrement">If mode 1 is used, this controls the loop increment. Set it to more than 1 to skip afterimages.</param>
        /// <param name="texture">The texture to draw. Set to <b>null</b> to draw the projectile's own loaded texture.</param>
        /// <param name="drawCentered">If <b>false</b>, the afterimages will be centered on the projectile's position instead of its own center.</param>
        public static void DrawAfterimagesCentered(Projectile proj, int mode, Color lightColor, int typeOneIncrement = 1, Texture2D texture = null, bool drawCentered = true)
        {
            if (texture is null)
                texture = TextureAssets.Projectile[proj.type].Value;

            int frameHeight = texture.Height / Main.projFrames[proj.type];
            int frameY = frameHeight * proj.frame;
            float scale = proj.scale;
            float rotation = proj.rotation;

            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);
            Vector2 origin = rectangle.Size() / 2f;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (proj.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // If no afterimages are drawn due to an invalid mode being specified, ensure the projectile itself is drawn anyway.
            bool failedToDrawAfterimages = false;

            if (EternityClientConfig.Instance.Afterimages)
            {
                Vector2 centerOffset = drawCentered ? proj.Size / 2f : Vector2.Zero;
                Color alphaColor = proj.GetAlpha(lightColor);
                switch (mode)
                {
                    // Standard afterimages. No customizable features other than total afterimage count.
                    // Type 0 afterimages linearly scale down from 100% to 0% opacity. Their color and lighting is equal to the main projectile's.
                    case 0:
                        for (int i = 0; i < proj.oldPos.Length; ++i)
                        {
                            Vector2 drawPos = proj.oldPos[i] + centerOffset - Main.screenPosition + new Vector2(0f, proj.gfxOffY);
                            // DO NOT REMOVE THESE "UNNECESSARY" FLOAT CASTS. THIS WILL BREAK THE AFTERIMAGES.
                            Color color = alphaColor * ((float)(proj.oldPos.Length - i) / (float)proj.oldPos.Length);
                            Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(rectangle), color, rotation, origin, scale, spriteEffects, 0f);
                        }
                        break;

                    // Paladin's Hammer style afterimages. Can be optionally spaced out further by using the typeOneDistanceMultiplier variable.
                    // Type 1 afterimages linearly scale down from 66% to 0% opacity. They otherwise do not differ from type 0.
                    case 1:
                        // Safety check: the loop must increment
                        int increment = Math.Max(1, typeOneIncrement);
                        Color drawColor = alphaColor;
                        int afterimageCount = ProjectileID.Sets.TrailCacheLength[proj.type];
                        float afterimageColorCount = (float)afterimageCount * 1.5f;
                        int k = 0;
                        while (k < afterimageCount)
                        {
                            Vector2 drawPos = proj.oldPos[k] + centerOffset - Main.screenPosition + new Vector2(0f, proj.gfxOffY);
                            // DO NOT REMOVE THESE "UNNECESSARY" FLOAT CASTS EITHER.
                            if (k > 0)
                            {
                                float colorMult = (float)(afterimageCount - k);
                                drawColor *= colorMult / afterimageColorCount;
                            }
                            Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(rectangle), drawColor, rotation, origin, scale, spriteEffects, 0f);
                            k += increment;
                        }
                        break;

                    // Standard afterimages with rotation. No customizable features other than total afterimage count.
                    // Type 2 afterimages linearly scale down from 100% to 0% opacity. Their color and lighting is equal to the main projectile's.
                    case 2:
                        for (int i = 0; i < proj.oldPos.Length; ++i)
                        {
                            float afterimageRot = proj.oldRot[i];
                            SpriteEffects sfxForThisAfterimage = proj.oldSpriteDirection[i] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                            Vector2 drawPos = proj.oldPos[i] + centerOffset - Main.screenPosition + new Vector2(0f, proj.gfxOffY);
                            // DO NOT REMOVE THESE "UNNECESSARY" FLOAT CASTS. THIS WILL BREAK THE AFTERIMAGES.
                            Color color = alphaColor * ((float)(proj.oldPos.Length - i) / (float)proj.oldPos.Length);
                            Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(rectangle), color, afterimageRot, origin, scale, sfxForThisAfterimage, 0f);
                        }
                        break;

                    default:
                        failedToDrawAfterimages = true;
                        break;
                }
            }

            // Draw the projectile itself. Only do this if no afterimages are drawn because afterimage 0 is the projectile itself.
            if (!EternityClientConfig.Instance.Afterimages || ProjectileID.Sets.TrailCacheLength[proj.type] <= 0 || failedToDrawAfterimages)
            {
                Vector2 startPos = drawCentered ? proj.Center : proj.position;
                Main.spriteBatch.Draw(texture, startPos - Main.screenPosition + new Vector2(0f, proj.gfxOffY), rectangle, proj.GetAlpha(lightColor), rotation, origin, scale, spriteEffects, 0f);
            }
        }

        // Used for bullets. This lets you draw afterimages while keeping the hitbox at the front of the projectile.
        // This supports type 0 and type 2 afterimages. Vanilla bullets never have type 2 afterimages.
        public static void DrawAfterimagesFromEdge(Projectile proj, int mode, Color lightColor, Texture2D texture = null)
        {
            if (texture is null)
                texture = TextureAssets.Projectile[proj.type].Value;

            int frameHeight = texture.Height / Main.projFrames[proj.type];
            int frameY = frameHeight * proj.frame;
            float scale = proj.scale;
            float rotation = proj.rotation;

            Rectangle rectangle = new Rectangle(0, frameY, texture.Width, frameHeight);

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (proj.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, proj.height * 0.5f);

            switch (mode)
            {
                default: // If you specify an afterimage mode other than 0 or 2, you get nothing.
                    return;

                // Standard afterimages. No customizable features other than total afterimage count.
                // Type 0 afterimages linearly scale down from 100% to 0% opacity. Their color and lighting is equal to the main projectile's.
                case 0:
                    for (int i = 0; i < proj.oldPos.Length; ++i)
                    {
                        Vector2 drawPos = proj.oldPos[i] + drawOrigin - Main.screenPosition + new Vector2(0f, proj.gfxOffY);
                        // DO NOT REMOVE THESE "UNNECESSARY" FLOAT CASTS. THIS WILL BREAK THE AFTERIMAGES.
                        Color color = proj.GetAlpha(lightColor) * ((float)(proj.oldPos.Length - i) / (float)proj.oldPos.Length);
                        Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(rectangle), color, rotation, drawOrigin, scale, spriteEffects, 0f);
                    }
                    return;

                // Standard afterimages with rotation. No customizable features other than total afterimage count.
                // Type 2 afterimages linearly scale down from 100% to 0% opacity. Their color and lighting is equal to the main projectile's.
                case 2:
                    for (int i = 0; i < proj.oldPos.Length; ++i)
                    {
                        float afterimageRot = proj.oldRot[i];
                        SpriteEffects sfxForThisAfterimage = proj.oldSpriteDirection[i] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                        Vector2 drawPos = proj.oldPos[i] + drawOrigin - Main.screenPosition + new Vector2(0f, proj.gfxOffY);
                        // DO NOT REMOVE THESE "UNNECESSARY" FLOAT CASTS. THIS WILL BREAK THE AFTERIMAGES.
                        Color color = proj.GetAlpha(lightColor) * ((float)(proj.oldPos.Length - i) / (float)proj.oldPos.Length);
                        Main.spriteBatch.Draw(texture, drawPos, new Rectangle?(rectangle), color, afterimageRot, drawOrigin, scale, sfxForThisAfterimage, 0f);
                    }
                    return;
            }
        }

        // Cached for efficiency purposes.
        internal static readonly FieldInfo BeginCalled = typeof(SpriteBatch).GetField("beginCalled", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Determines if a <see cref="SpriteBatch"/> is in a lock due to a <see cref="SpriteBatch.Begin"/> call.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to check.</param>
        public static bool HasBeginBeenCalled(this SpriteBatch spriteBatch)
        {
            return (bool)BeginCalled.GetValue(spriteBatch);
        }

        public static Vector2 TileDrawOffset => Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);

        /// <summary>
        /// Helper for drawing a simple item with a customized scale in the inventory.
        /// <para>Various notes and tips:</para>
        /// <list type="bullet">Only apply this effect to items which get screwed over by effects like a flame animation
        /// <item>For consistency, try to keep the physical object inside the slot.</item>
        /// <item>Sprited effects like thunder, fire, ect can be fine when allowed outside to bleed outside a slot. <para>However these bleeds should only happen for a few frames, like Auric Tesla and Dyanmo Stem Cells.</para></item>
        /// <item>Exceptions to these rules are the various Tracers, The Community, and Quiver of Nihility.<para>Do not treat their upscales as them as a norm.</para></item>
        /// </list>
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="frame"></param>
        /// <param name="drawColor"></param>
        /// <param name="itemColor"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="wantedScale"></param>
        /// <param name="drawOffset"></param>
        public static void DrawInventoryCustomScale(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale, float wantedScale = 1f, Vector2 drawOffset = default)
        {
            wantedScale = Math.Max(scale, wantedScale * Main.inventoryScale);
            float scaleDifference = wantedScale - scale;
            position += drawOffset * wantedScale;
            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, wantedScale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a treasure bag in the world in the exact same way as how Terraria 1.4's bags are drawn.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="spriteBatch">The spritebatch.</param>
        /// <param name="rotation">The rotation of the item.</param>
        /// <param name="scale">The scale of the item.</param>
        /// <param name="whoAmI">Index reference for <see cref="Main.itemFrameCounter"/>.</param>
        public static bool DrawTreasureBagInWorld(Item item, SpriteBatch spriteBatch, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[item.type].Value;
            Rectangle frame = texture.Frame();

            // Use special item animations if applicable.
            if (Main.itemAnimations[item.type] != null)
                frame = Main.itemAnimations[item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);

            Vector2 frameOrigin = frame.Size() * 0.5f;
            Vector2 offset = new Vector2(item.width / 2 - frameOrigin.X, item.height - frame.Height);
            Vector2 drawPos = item.position - Main.screenPosition + frameOrigin + offset;

            float localTime = item.timeSinceItemSpawned / 240f + Main.GlobalTimeWrappedHourly * 0.04f;

            // Transform the global time value's incremental form into a unit-interval triangle wave.
            float time = Main.GlobalTimeWrappedHourly % 4f / 2f;
            if (time >= 1f)
                time = 2f - time;
            time = time * 0.5f + 0.5f;

            // Draw the outer pulse effect.
            for (int i = 0; i < 4; i++)
            {
                Vector2 pulseOffset = Vector2.UnitY.RotatedBy((i / 4f + localTime) * MathHelper.TwoPi) * time * 8f;
                spriteBatch.Draw(texture, drawPos + pulseOffset, frame, new Color(90, 70, 255, 50), rotation, frameOrigin, scale, 0, 0);
            }

            // Draw the inner pulse effect.
            for (int i = 0; i < 3; i++)
            {
                Vector2 pulseOffset = Vector2.UnitY.RotatedBy((i / 3f + localTime) * MathHelper.TwoPi) * time * 4f;
                spriteBatch.Draw(texture, drawPos + pulseOffset, frame, new Color(140, 120, 255, 77), rotation, frameOrigin, scale, 0, 0);
            }

            return true;
        }

        /// <summary>
        /// Copies the contents of one render target to another.
        /// </summary>
        public static void CopyContentsFrom(this RenderTarget2D to, RenderTarget2D from)
        {
            Main.instance.GraphicsDevice.SetRenderTarget(to);
            Main.instance.GraphicsDevice.Clear(Color.Transparent);

            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);
            Main.spriteBatch.Draw(from, Vector2.Zero, null, Color.White);
            Main.spriteBatch.End();

            Main.instance.GraphicsDevice.SetRenderTarget(from);
            Main.instance.GraphicsDevice.Clear(Color.Transparent);
            Main.instance.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// Calculates perspective matrices for usage by vertex shaders.
        /// </summary>
        /// <param name="viewMatrix">The view matrix.</param>
        /// <param name="projectionMatrix">The projection matrix.</param>
        public static void CalculatePerspectiveMatricies(out Matrix viewMatrix, out Matrix projectionMatrix)
        {
            Vector2 zoom = Main.GameViewMatrix.Zoom;
            Matrix zoomScaleMatrix = Matrix.CreateScale(zoom.X, zoom.Y, 1f);

            // Screen bounds.
            int width = Main.instance.GraphicsDevice.Viewport.Width;
            int height = Main.instance.GraphicsDevice.Viewport.Height;

            // Get a matrix that aims towards the Z axis (these calculations are relative to a 2D world).
            viewMatrix = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up);

            // Offset the matrix to the appropriate position.
            viewMatrix *= Matrix.CreateTranslation(0f, -height, 0f);

            // Flip the matrix around 180 degrees.
            viewMatrix *= Matrix.CreateRotationZ(MathHelper.Pi);

            // Account for the inverted gravity effect.
            if (Main.LocalPlayer.gravDir == -1f)
                viewMatrix *= Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, height, 0f);

            // And account for the current zoom.
            viewMatrix *= zoomScaleMatrix;

            projectionMatrix = Matrix.CreateOrthographicOffCenter(0f, width * zoom.X, 0f, height * zoom.Y, 0f, 1f) * zoomScaleMatrix;
        }

        /// <summary>
        /// Sets a <see cref="SpriteBatch"/>'s <see cref="BlendState"/> arbitrarily.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="blendState">The blend state to use.</param>
        public static void SetBlendState(this SpriteBatch spriteBatch, BlendState blendState)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, blendState, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static bool TryBegin(this SpriteBatch spriteBatch, SpriteSortMode sortMode,
            BlendState blendState,
            SamplerState samplerState,
            DepthStencilState depthStencilState,
            RasterizerState rasterizerState,
            Effect effect,
            Matrix transformMatrix)
        {
            if (spriteBatch.HasBeginBeenCalled())
                return false;
            else
            {
                spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
                return true;
            }
        }

        public static bool TryEnd(this SpriteBatch spriteBatch)
        {
            if (!spriteBatch.HasBeginBeenCalled())
                return false;
            else
            {
                spriteBatch.End();
                return true;
            }
        }

        // Cached for efficiency purposes.
        internal static readonly FieldInfo UImageFieldMisc0 = typeof(MiscShaderData).GetField("_uImage0", BindingFlags.NonPublic | BindingFlags.Instance);
        internal static readonly FieldInfo UImageFieldMisc1 = typeof(MiscShaderData).GetField("_uImage1", BindingFlags.NonPublic | BindingFlags.Instance);
        internal static readonly FieldInfo UImageFieldArmor = typeof(ArmorShaderData).GetField("_uImage", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Manually sets the texture of a <see cref="MiscShaderData"/> instance, since vanilla's implementation only supports strings that access vanilla textures.
        /// </summary>
        /// <param name="shader">The shader to bind the texture to.</param>
        /// <param name="texture">The texture to bind.</param>
        public static MiscShaderData SetShaderTexture(this MiscShaderData shader, Asset<Texture2D> texture, int index = 1)
        {
            switch (index)
            {
                case 0:
                    UImageFieldMisc0.SetValue(shader, texture);
                    break;
                case 1:
                    UImageFieldMisc1.SetValue(shader, texture);
                    break;
            }
            return shader;
        }

        /// <summary>
        /// Manually sets the texture of a <see cref="ArmorShaderData"/> instance, since vanilla's implementation only supports strings that access vanilla textures.
        /// </summary>
        /// <param name="shader">The shader to bind the texture to.</param>
        /// <param name="texture">The texture to bind.</param>
        public static ArmorShaderData SetShaderTextureArmor(this ArmorShaderData shader, Asset<Texture2D> texture)
        {
            UImageFieldArmor.SetValue(shader, texture);
            return shader;
        }

        public static void EnterShaderRegion(this SpriteBatch spriteBatch, BlendState newBlendState = null, Effect effect = null)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, newBlendState ?? BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, effect, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void ExitShaderRegion(this SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        /// <summary>
        /// Restarts a given <see cref="SpriteBatch"/> such that it enforces a rectangular area where pixels outside of said area are not drawn.<br></br>
        /// This is incredible convenient for UI sections where you need to ensure things only appear inside a box panel.<br></br>
        /// This method should be followed by a call to <see cref="ReleaseCutoffRegion(SpriteBatch, Matrix, SpriteSortMode)"/> once you're ready to flush the contents drawn under these conditions.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to enforce the cutoff region on.</param>
        /// <param name="cutoffRegion">The cutoff region. This should be in screen coordinates.</param>
        /// <param name="perspective">The perspective matrix that should be used across drawn contents.</param>
        /// <param name="sortMode">The sort mode that should be used across drawn contents. Use <see cref="SpriteSortMode.Immediate"/> if you additionally need to draw shaders.</param>
        /// <param name="newBlendState">The blend state that should be used across drawn contents. This defaults to <see cref="BlendState.AlphaBlend"/>.</param>
        public static void EnforceCutoffRegion(this SpriteBatch spriteBatch, Rectangle cutoffRegion, Matrix perspective, SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState newBlendState = null)
        {
            var rasterizer = Main.Rasterizer;
            rasterizer.ScissorTestEnable = true;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, newBlendState ?? BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, null, perspective);
            spriteBatch.GraphicsDevice.ScissorRectangle = cutoffRegion;
        }

        /// <summary>
        /// Flushes contents drawn under restrictions enforced by the <see cref="EnforceCutoffRegion(SpriteBatch, Rectangle, Matrix, SpriteSortMode, BlendState)"/> method and returns the <see cref="SpriteBatch"/> to a more typical state.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to flush the contents of.</param>
        /// <param name="perspective">The perspective matrix that was used before the cutoff region was enforced. Take care to ensure that this has the correct input.</param>
        /// <param name="sortMode">The sort mode that should be used across drawn contents. Use <see cref="SpriteSortMode.Immediate"/> if you additionally need to draw shaders.</param>
        public static void ReleaseCutoffRegion(this SpriteBatch spriteBatch, Matrix perspective, SpriteSortMode sortMode = SpriteSortMode.Deferred)
        {
            int width = spriteBatch.GraphicsDevice.Viewport.Width;
            int height = spriteBatch.GraphicsDevice.Viewport.Height;

            spriteBatch.End();
            spriteBatch.Begin(sortMode, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, perspective);
            spriteBatch.GraphicsDevice.ScissorRectangle = new(-1, -1, width + 2, height + 2);
        }

        public static IEnumerable<DrawData> DrawAuroras(Player player, float auroraCount, float opacity, Color color)
        {
            float time = Main.GlobalTimeWrappedHourly % 3f / 3f;
            Texture2D auroraTexture = AuroraTexture;
            for (int i = 0; i < auroraCount; i++)
            {
                float incrementOffsetAngle = MathHelper.TwoPi * i / auroraCount;
                float xOffset = (float)Math.Sin(time * MathHelper.TwoPi + incrementOffsetAngle * 2f) * 20f;
                float yOffset = (float)Math.Sin(time * MathHelper.TwoPi + incrementOffsetAngle * 2f + MathHelper.ToRadians(60f)) * 6f;
                float rotation = (float)Math.Sin(incrementOffsetAngle) * MathHelper.Pi / 12f;
                Vector2 offset = new Vector2(xOffset, yOffset - 14f);
                yield return new DrawData(auroraTexture,
                                 player.Top + offset - Main.screenPosition,
                                 null,
                                 color * opacity,
                                 rotation + MathHelper.PiOver2,
                                 auroraTexture.Size() * 0.5f,
                                 0.135f,
                                 SpriteEffects.None,
                                 1);
            }
        }

        /// <summary>
        /// Draws an item in the inventory with a new texture to replace a previous one.
        /// Useful in situations where for example, a different sprite is used for the "real" inventory sprite so it may appear when the player is using it.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="newTexture"></param>
        /// <param name="originalSize"></param>
        /// <param name="position"></param>
        /// <param name="drawColor"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        public static void DrawNewInventorySprite(this SpriteBatch spriteBatch, Texture2D newTexture, Vector2 originalSize, Vector2 position, Color drawColor, Vector2 origin, float scale, Vector2? offset = null)
        {
            Vector2 extraOffset;
            if (offset == null)
                extraOffset = Vector2.Zero;
            else
                extraOffset = offset.GetValueOrDefault();

            float largestDimensionOriginal = Math.Max(originalSize.X, originalSize.Y);
            float largestDimensionNew = Math.Max(newTexture.Width, newTexture.Height);

            //Scale the sprite so it will account for the dimension of the new sprite if it is larger than the old one (As in, we need to scale down the scale or else it will be too large)
            float scaleRatio = Math.Min(largestDimensionOriginal / largestDimensionNew, 1);

            //Offset the jellyfish sprite properly, since the fishing rod is larger than the jellyfish (Jellyfish width : 28px, Fishing rod width : 42)
            Vector2 positionOffset = Vector2.Zero;

            if (originalSize.X > newTexture.Width)
                positionOffset.X = (originalSize.X - newTexture.Width) / 2f;

            positionOffset *= scale;

            spriteBatch.Draw(newTexture, position + positionOffset + extraOffset, null, drawColor, 0f, origin, scale * scaleRatio, 0, 0);
        }

        public delegate void ChromaAberrationDelegate(Vector2 offset, Color colorMult);

        /// <summary>
        /// Draws a chromatic abberation effect.
        /// </summary>
        /// <param name="direction">The direction of the abberation</param>
        /// <param name="strength">The strenght of the abberation</param>
        /// <param name="action">The draw call itself.</param>
        public static void DrawChromaticAberration(Vector2 direction, float strength, ChromaAberrationDelegate drawCall)
        {
            for (int i = -1; i <= 1; i++)
            {
                Color aberrationColor = Color.White;
                switch (i)
                {
                    case -1:
                        aberrationColor = new Color(255, 0, 0, 0);
                        break;
                    case 0:
                        aberrationColor = new Color(0, 255, 0, 0);
                        break;
                    case 1:
                        aberrationColor = new Color(0, 0, 255, 0);
                        break;
                }

                Vector2 offset = direction.RotatedBy(MathHelper.PiOver2) * i;
                offset *= strength;

                drawCall.Invoke(offset, aberrationColor);
            }
        }

        /// <summary>
        /// Sets the current render target to the provided one.
        /// </summary>
        /// <param name="target">The render target to swap to</param>
        /// <param name="flushColor">The color to clear the screen with. Transparent by default</param>
        public static void SwapTo(this RenderTarget2D target, Color? flushColor = null)
        {
            // If we are in the menu, a server, or any of these are null, return.
            if (Main.gameMenu || Main.dedServ || target is null || Main.instance.GraphicsDevice is null || Main.spriteBatch is null)
                return;

            Main.instance.GraphicsDevice.SetRenderTarget(target);
            Main.instance.GraphicsDevice.Clear(flushColor ?? Color.Transparent);
        }
    }
}
