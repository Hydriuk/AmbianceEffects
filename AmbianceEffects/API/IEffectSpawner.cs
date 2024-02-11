using AmbianceEffects.Models;
#if OPENMOD
using OpenMod.API.Ioc;
#endif
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UHighlight.Components;

namespace AmbianceEffects.API
{
#if OPENMOD
    [Service]
#endif
    internal interface IEffectSpawner : IDisposable
    {
        /// <summary>
        /// Spawns a list of effects
        /// </summary>
        /// <param name="player">Player who triggered the spawn</param>
        /// <param name="zone">The zone the trigger came from</param>
        /// <param name="effects">List of effects to spawn</param>
        void SpawnEffects(Player player, HighlightedZone zone, IEnumerable<AmbianceEffect> effects);

        /// <summary>
        /// Stops repeating effects
        /// </summary>
        /// <param name="player">The player who triggered the effect</param>
        /// <param name="zone">The zone the trigger came from</param>
        /// <param name="effects">List of effects to stop</param>
        void StopRepeatingEffects(Player player, HighlightedZone zone, IEnumerable<AmbianceEffect> effects);

        /// <summary>
        /// Stops all repeating effects
        /// </summary>
        void ClearEffects();
    }
}
