using Microsoft.Xna.Framework;
using Terraria;

namespace EternityMod
{
    public static partial class EternityUtils
    {
        public static Vector2 SafeDirectionTo(this Entity entity, Vector2 destination, Vector2 fallback = default) => (destination - entity.Center).SafeNormalize(fallback);
    }
}
