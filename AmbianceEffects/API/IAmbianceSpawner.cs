#if OPENMOD
using OpenMod.API.Ioc;
#endif
using System;
using System.Threading.Tasks;

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
        Task ReloadZones();
    }
}
