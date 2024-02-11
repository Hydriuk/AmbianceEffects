using AmbianceEffects.Models;
#if OPENMOD
using OpenMod.API.Ioc;
#endif
using SDG.Unturned;
using System;
using System.Collections.Generic;

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
        /// <param name="effects">List of effects to spawn</param>
        void SpawnEffects(Player player, IEnumerable<AmbianceEffect> effects);
    }
}
