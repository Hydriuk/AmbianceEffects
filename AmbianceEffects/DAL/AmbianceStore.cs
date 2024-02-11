using AmbianceEffects.API;
using AmbianceEffects.Models;
#if OPENMOD
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
#endif
using Hydriuk.UnturnedModules.Adapters;
using LiteDB;
using System.Collections.Generic;
using System.IO;

namespace AmbianceEffects.DAL
{
#if OPENMOD
    [PluginServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
#endif
    internal class AmbianceStore : IAmbianceStore
    {
        private readonly LiteDatabase _database;
        private readonly ILiteCollection<AmbianceZone> _ambiances;

        public AmbianceStore(IEnvironmentAdapter environmentAdapter)
        {
            _database = new LiteDatabase(Path.Combine(environmentAdapter.Directory, "ambiances.db"));

            _ambiances = _database.GetCollection<AmbianceZone>();
            _ambiances.EnsureIndex(ambiance => ambiance.Name);
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public bool Create(AmbianceZone ambiance)
        {
            if (_ambiances.Exists(a => a.Name == ambiance.Name))
                return false;

            _ambiances.Insert(ambiance);

            return true;
        }

        public bool Update(AmbianceZone ambiance)
        {
            if (ambiance.Id == ObjectId.Empty)
                return false;

            _ambiances.Update(ambiance);

            return true;
        }

        public void Delete(string name)
        {
            _ambiances.DeleteMany(a => a.Name == name);
        }

        public AmbianceZone Get(string name)
        {
            return _ambiances.FindOne(a => a.Name == name);
        }

        public IEnumerable<AmbianceZone> GetAll()
        {
            return _ambiances.FindAll();
        }
    }
}
