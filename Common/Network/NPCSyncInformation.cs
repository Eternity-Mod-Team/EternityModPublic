﻿using Microsoft.Xna.Framework;
using Terraria;

namespace EternityMod.Common.Network
{
    // Sometimes packets may arrive a little too early (such as a few milliseconds before an NPC spawn packet creates the NPC this packet wants).
    // To account for such cases, NPC packets which cannot be properly accounted for are stored in a temporary cache until they are ready.
    // To ensure that the list remains clean, the information has a time limit before it's discarded as be unnecessary.
    public class NPCSyncInformation
    {
        public int TimeSinceReceived;
        public int NPCIndex = -1;
        public int CachedRealLife = -1;
        public int TotalUniqueIndicesUsed;
        public int TotalPlayersAtStart;

        public int[] ExtraAIIndicesUsed;
        public float[] ExtraAIValues;

        public Rectangle ArenaRectangle;

        public bool ShouldBeDiscarded => NPCIndex <= -1 || TimeSinceReceived >= LifetimeBeforeDiscardation;

        public const int LifetimeBeforeDiscardation = 600;

        public bool TryToApplyToNPC()
        {
            // Increment the life time counter with each try.
            TimeSinceReceived++;

            if (NPCIndex < 0)
                return false;

            // If the NPC is not active, it is possible that the packet which initialized the NPC has not been sent yet.
            // If so, wait until that happens.
            if (!Main.npc[NPCIndex].active)
                return false;

            if (CachedRealLife >= 0)
                Main.npc[NPCIndex].realLife = CachedRealLife;

            for (var i = 0; i < TotalUniqueIndicesUsed; i++)
            {
                Main.npc[NPCIndex].Eternity().HasAssociatedAIBeenUsed[ExtraAIIndicesUsed[i]] = true;
                Main.npc[NPCIndex].Eternity().ExtraAI[ExtraAIIndicesUsed[i]] = ExtraAIValues[i];
            }

            if (ArenaRectangle != default && Main.npc[NPCIndex].Eternity().Arena == default)
                Main.npc[NPCIndex].Eternity().Arena = ArenaRectangle;

            Main.npc[NPCIndex].Eternity().TotalPlayersAtStart = TotalPlayersAtStart;

            return true;
        }
    }
}
