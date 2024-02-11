using AmbianceEffects.DAL;
using AmbianceEffects.Services;
using Hydriuk.RocketModModules;
using Hydriuk.RocketModModules.Adapters;
using Rocket.Core.Plugins;
using UHighlight.API;
using UHighlight.RocketMod.Adapters;

namespace AmbianceEffects.RocketMod
{
    public class Plugin : RocketPlugin
    {
        public static Plugin Instance { get; private set; }

        private ServiceRegistrator _serviceRegistrator;

        [PluginService] private EnvironmentAdapter _environmentAdapter;
        [PluginService] private ThreadAdapter _threadAdapter;
        [PluginService] private ServiceAdapter _serviceAdapter;

        [PluginService] internal AmbianceStore AmbianceStore { get; private set; }
        [PluginService] internal EffectSpawner EffectSpawner { get; private set; }
        [PluginService] internal AmbianceSpawner AmbianceSpawner { get; private set; }
        internal IHighlightCommands HighlightCommands { get; private set; }

        public Plugin()
        {
            Instance = this;
        }

        protected override void Load()
        {
            _serviceRegistrator = new ServiceRegistrator(this);

            HighlightCommands = new HighlightCommands();
        }

        protected override void Unload()
        {
            _serviceRegistrator.Dispose();
        }
    }
}