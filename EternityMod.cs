using System.IO;
using EternityMod.Common.Network;
using EternityMod.Common.Systems;
using EternityMod.Common.Systems.Overriding;
using EternityMod.Graphics.Particles;
using EternityMod.Graphics.Primitives;
using EternityMod.ModSupport;
using Luminance.Core.ModCalls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EternityMod
{
    public class EternityMod : Mod
	{
		internal static EternityMod Instance;

        // This is the mod that contains all of Eternity's music.
        internal Mod MusicMod = null;
        internal bool MusicAvailable => MusicMod is not null;

        // Keep these in alphabetical order for better readability.
        internal Mod AncientsAwakened = null;
        internal Mod Calamity = null;
        internal Mod FargosMutant = null;
        internal Mod FargosSouls = null;
        internal Mod PhaseIndicator = null;
        internal Mod ShadowsOfAbaddon = null;
        internal Mod Spirit = null;
        internal Mod Thorium = null;
        internal Mod Wikithis = null;

        // If Death Mode is enabled at minimum, custom hostile NPCs will be modified in terms of their AIs.
        public static bool CanUseCustomAIs => DifficultyModeSystem.DeathMode;

        public override void Load()
        {
            Instance = this;

            ModLoader.TryGetMod("EternityModMusic", out MusicMod);
            ModLoader.TryGetMod("AAMod", out AncientsAwakened);
            ModLoader.TryGetMod("CalamityMod", out Calamity);
            ModLoader.TryGetMod("Fargowiltas", out FargosMutant);
            ModLoader.TryGetMod("FargowiltasSouls", out FargosSouls);
            ModLoader.TryGetMod("PhaseIndicator", out PhaseIndicator);
            ModLoader.TryGetMod("SacredTools", out ShadowsOfAbaddon);
            ModLoader.TryGetMod("SpiritMod", out Spirit);
            ModLoader.TryGetMod("ThoriumMod", out Thorium);
            ModLoader.TryGetMod("Wikithis", out Wikithis);

            // Buff the Drill Containment Unit.
            Mount.drillPickPower = 225;

            // Make graveyard biomes require more gravestones.
            SceneMetrics.GraveyardTileMax = 60;
            SceneMetrics.GraveyardTileMin = 40;
            SceneMetrics.GraveyardTileThreshold = 52;

            // Manually invoke the attribute constructors to get the marked methods cached.
            foreach (var type in typeof(EternityMod).Assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(EternityUtils.UniversalBindingFlags))
                    method.GetCustomAttributes(false);
            }

            if (Main.netMode != NetmodeID.Server)
            {
                GeneralParticleHandler.Load();
                PrimitiveRenderer.Initialize();
            }
        }

        public override void Unload()
        {
            Instance = null;

            MusicMod = null;
            AncientsAwakened = null;
            Calamity = null;
            FargosMutant = null;
            FargosSouls = null;
            ShadowsOfAbaddon = null;
            Spirit = null;
            Thorium = null;
            Wikithis = null;

            Mount.drillPickPower = 210;

            SceneMetrics.GraveyardTileMax = 36;
            SceneMetrics.GraveyardTileMin = 16;
            SceneMetrics.GraveyardTileThreshold = 28;

            if (Main.netMode != NetmodeID.Server)
                GeneralParticleHandler.Unload();
        }

        public override void PostSetupContent()
        {
            NPCBehaviorOverride.LoadAll();
            ProjectileBehaviorOverride.LoadAll();
            NPCBehaviorOverride.LoadPhaseIndicators();
            WeakReferenceSupport.Setup();
        }

        // Use Luminance's mod call manager for cross-compatibility for other mods.
        public override object Call(params object[] args) => ModCallManager.ProcessAllModCalls(this, args);

        public override void HandlePacket(BinaryReader reader, int whoAmI) => PacketManager.ReceivePacket(reader);
    }
}
