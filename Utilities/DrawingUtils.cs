using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        internal static readonly FieldInfo BeginCalled = typeof(SpriteBatch).GetField("beginCalled", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool HasBeginBeenCalled(this SpriteBatch spriteBatch)
        {
            return (bool)BeginCalled.GetValue(spriteBatch);
        }

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

        public static void SwapTo(this RenderTarget2D target, Color? flushColor = null)
        {
            if (Main.gameMenu || Main.dedServ || target is null || Main.instance.GraphicsDevice is null || Main.spriteBatch is null)
                return;

            Main.instance.GraphicsDevice.SetRenderTarget(target);
            Main.instance.GraphicsDevice.Clear(flushColor ?? Color.Transparent);
        }
    }
}
