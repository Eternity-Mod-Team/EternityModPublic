using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace EternityMod.Configs
{
    public class EternityClientConfig : ModConfig
    {
        public static EternityClientConfig Instance;

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message) => true;

        [OnDeserialized]
        internal void ClampValues(StreamingContext context)
        {
            BossHealthBoost = Utils.Clamp(BossHealthBoost, MinBossHealthBoost, MaxBossHealthBoost);
            ParticleLimit = (int)Utils.Clamp(ParticleLimit, MinParticleLimit, MaxParticleLimit);
        }

        [Header("Graphics")]

        [DefaultValue(true)]
        public bool Afterimages { get; set; }

        [Range(MinParticleLimit, MaxParticleLimit)]
        [DefaultValue(5000)]
        public int ParticleLimit { get; set; }
        private const int MinParticleLimit = 500;
        private const int MaxParticleLimit = 10000;

        [DefaultValue(false)]
        public bool BossesStopWeather { get; set; }

        [Header("Gameplay")]
        [Range(MinBossHealthBoost, MaxBossHealthBoost)]
        [Increment(25f)]
        [DrawTicks]
        [DefaultValue(MinBossHealthBoost)]
        public float BossHealthBoost { get; set; }
        private const float MinBossHealthBoost = 0f;
        private const float MaxBossHealthBoost = 900f;
    }
}
