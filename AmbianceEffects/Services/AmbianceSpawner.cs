using AmbianceEffects.API;
using AmbianceEffects.Models;
#if OPENMOD
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
#endif
using Hydriuk.UnturnedModules.Adapters;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UHighlight.API;
using UHighlight.Components;
using UHighlight.Models;

namespace AmbianceEffects.Services
{
#if OPENMOD
    [PluginServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
#endif
    internal class AmbianceSpawner : IAmbianceSpawner
    {
        private readonly Dictionary<HighlightedZone, AmbianceZone> _zones = new Dictionary<HighlightedZone, AmbianceZone>();

        private readonly IAmbianceStore _ambianceStore;
        private readonly IServiceAdapter _serviceAdapter;
        private readonly IEffectSpawner _effectSpawner;

        private IHighlightSpawner? _highlightSpawner;

        public AmbianceSpawner(IServiceAdapter serviceAdapter, IAmbianceStore ambianceStore, IEffectSpawner effectSpawner)
        {
            _ambianceStore = ambianceStore;
            _serviceAdapter = serviceAdapter;
            _effectSpawner = effectSpawner;

            _ = TriggerInit();
        }

        public void Dispose()
        {
            UnloadZones();
        }

        public async Task TriggerInit()
        {
            _highlightSpawner = await _serviceAdapter.GetServiceAsync<IHighlightSpawner>();

            ReloadZones();
        }

        public void ReloadZones()
        {
            UnloadZones();
            LoadZones();
        }

        private void UnloadZones()
        {
            foreach (var zone in _zones.Keys)
            {
                zone.Dispose();
            }
            _zones.Clear();
            _effectSpawner.Dispose();
        }

        private void LoadZones()
        {
            foreach (var ambianceZone in _ambianceStore.GetAll())
            {
                HighlightedZone zone = ActivateZone(ambianceZone);

                _zones.Add(zone, ambianceZone);
            }
        }

        private HighlightedZone ActivateZone(AmbianceZone ambianceZone)
        {
            if (_highlightSpawner == null)
                throw new Exception("HighlightSpawner did not load");

            HighlightedZone zone = _highlightSpawner.BuildZone(Constants.GROUP_NAME, ambianceZone.Name);

            zone.PlayerEntered += OnPlayerEntered;
            zone.PlayerExited += OnPlayerExited;

            return zone;
        }

        private void OnPlayerEntered(object sender, Player player)
        {
            HighlightedZone zone = (HighlightedZone)sender;

            if (!_zones.TryGetValue(zone, out var ambianceZone))
                return;

            _effectSpawner.SpawnEffects(
                player,
                zone,
                ambianceZone.Effects.Where(effect => effect.Event == ZoneProperty.EEvent.Enter)
            );

            _effectSpawner.StopRepeatingEffects(
                player,
                zone,
                ambianceZone.Effects.Where(effect => effect.Event == ZoneProperty.EEvent.Exit)
            );
        }

        private void OnPlayerExited(object sender, Player player)
        {
            HighlightedZone zone = (HighlightedZone)sender;

            if (!_zones.TryGetValue(zone, out var ambianceZone))
                return;

            _effectSpawner.SpawnEffects(
                player,
                zone,
                ambianceZone.Effects.Where(effect => effect.Event == ZoneProperty.EEvent.Exit)
            );

            _effectSpawner.StopRepeatingEffects(
                player, 
                zone,
                ambianceZone.Effects.Where(effect => effect.Event == ZoneProperty.EEvent.Enter)
            );
        }
    }
}
