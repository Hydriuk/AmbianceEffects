using AmbianceEffects.DAL;
using AmbianceEffects.Services;
using Hydriuk.RocketModModules;
using Rocket.Core.Plugins;

namespace AmbianceEffects.RocketMod
{
    public class Plugin : RocketPlugin
    {
        public static Plugin Instance { get; private set; }

        private ServiceRegistrator _serviceRegistrator;

        [PluginService] internal AmbianceStore AmbianceStore { get; private set; }
        [PluginService] internal EffectSpawner EffectSpawner { get; private set; }
        [PluginService] internal AmbianceSpawner AmbianceSpawner { get; private set; }

        public Plugin()
        {
            Instance = this;
        }

        protected override void Load()
        {
            _serviceRegistrator = new ServiceRegistrator(this);
        }

        protected override void Unload()
        {
            _serviceRegistrator.Dispose();
            AmbianceStore.Dispose();
            EffectSpawner.Dispose();
            AmbianceSpawner.Dispose();
        }
    }
}