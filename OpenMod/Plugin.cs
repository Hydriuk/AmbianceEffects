using AmbianceEffects.API;
using Hydriuk.OpenModModules;
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Eventing;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;
using OpenMod.Core.Events;
using OpenMod.Unturned.Plugins;
using System;
using System.Threading.Tasks;
using UHighlight.OpenMod;

[assembly: PluginMetadata("AmbianceEffects.OpenMod", DisplayName = "AmbianceEffects", Author = "Hydriuk")]

namespace AmbianceEffects.OpenMod
{
    public class Plugin : OpenModUnturnedPlugin
    {
        public Plugin(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }

    public class ServiceConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(IOpenModServiceConfigurationContext openModStartupContext, IServiceCollection serviceCollection)
        {
            ServiceRegistrator.ConfigureServices<UHighlightPlugin>(openModStartupContext, serviceCollection);
        }
    }

    public class OpenModLoadedEvent : IEventListener<OpenModInitializedEvent>
    {
        private readonly IServiceProvider _serviceProvider;

        public OpenModLoadedEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task HandleEventAsync(object? sender, OpenModInitializedEvent @event)
        {
            // Load services after plugin has loaded
            _serviceProvider.GetRequiredService<IAmbianceSpawner>();

            return Task.CompletedTask;
        }
    }
}