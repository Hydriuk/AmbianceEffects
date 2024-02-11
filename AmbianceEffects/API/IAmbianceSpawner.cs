#if OPENMOD
using OpenMod.API.Ioc;
#endif
using System;

namespace AmbianceEffects.API
{
#if OPENMOD
    [Service]
#endif
    internal interface IAmbianceSpawner : IDisposable
    {
        /// <summary>
        /// Reload all ambiance zones
        /// </summary>
        void ReloadZones();
    }
}
