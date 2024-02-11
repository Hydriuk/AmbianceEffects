using AmbianceEffects.Models;
#if OPENMOD
using OpenMod.API.Ioc;
#endif
using System;
using System.Collections.Generic;

namespace AmbianceEffects.API
{
#if OPENMOD
    [Service]
#endif
    internal interface IAmbianceStore : IDisposable
    {
        /// <summary>
        /// Create a new ambiance zone
        /// </summary>
        /// <param name="ambiance">Zone to create</param>
        /// <returns>True if the zone was created. False otherwise</returns>
        bool Create(AmbianceZone ambiance);

        /// <summary>
        /// Update an existing ambiance zone
        /// </summary>
        /// <param name="ambiance">Zone to update</param>
        /// <returns>True if zone was update, False otherwise</returns>
        bool Update(AmbianceZone ambiance);

        /// <summary>
        /// Delete a zone
        /// </summary>
        /// <param name="name">Name of the zone to delete</param>
        void Delete(string name);

        /// <summary>
        /// Get an existing zone
        /// </summary>
        /// <param name="name">Name of the zone to get</param>
        /// <returns>An ambiance zone</returns>
        AmbianceZone Get(string name);

        /// <summary>
        /// Get all existing ambiance zones
        /// </summary>
        /// <returns>List of ambiance zones</returns>
        IEnumerable<AmbianceZone> GetAll();
    }
}
