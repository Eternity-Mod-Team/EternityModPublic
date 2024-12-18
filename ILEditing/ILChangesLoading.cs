using Terraria;
using Terraria.ModLoader;

namespace EternityMod.ILEditing
{
    public partial class ILChanges : ModSystem
    {
        public override void OnModLoad()
        {
            IL_Main.DoDraw += AdditiveDrawing;
            On_Main.DrawInfernoRings += DrawForegroundParticles;
        }

        public override void OnModUnload()
        {
            
        }
    }
}
