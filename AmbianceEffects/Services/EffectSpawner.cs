using AmbianceEffects.API;
using AmbianceEffects.Models;
using Hydriuk.UnturnedModules.Adapters;
#if OPENMOD
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
#endif
using Hydriuk.UnturnedModules.Extensions;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UHighlight.Components;
using Random = UnityEngine.Random;

namespace AmbianceEffects.Services
{
#if OPENMOD
    [PluginServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
#endif
    internal class EffectSpawner : IEffectSpawner
    {
        private readonly Dictionary<Player, List<RepeatingEffect>> _repeatingEffects = new Dictionary<Player, List<RepeatingEffect>>();

        private readonly IThreadAdapter _threadAdapter;

        public EffectSpawner(IThreadAdapter threadAdapter)
        {
            _threadAdapter = threadAdapter;
        }

        public void Dispose() => ClearEffects();
        public void ClearEffects()
        {
            foreach (var effects in _repeatingEffects.Values)
            {
                foreach (var effect in effects)
                {
                    effect.Timer?.Dispose();
                }
            }

            _repeatingEffects.Clear();
        }

        public void SpawnEffects(Player player, HighlightedZone zone, IEnumerable<AmbianceEffect> effects)
        {
            foreach (var effect in effects)
            {
                TriggerEffectParameters effectParams = new TriggerEffectParameters(effect.EffectGUID)
                {
                    position = effect.Position,
                    relevantDistance = effect.VisibiltyRadius
                };

                if (effect.OwnerOnly)
                    effectParams.SetRelevantPlayer(player.GetTransportConnection());

                if(effect.MinRepeat == -1 || effect.MaxRepeat == -1)
                {
                    EffectManager.triggerEffect(effectParams);
                }
                else
                {
                    // Create timer
                    var state = new RepeatingEffect(zone, effect, effectParams);
                    var timer = new Timer(OnTimerCallback, state, (int)Random.Range(effect.MinRepeat * 1000, effect.MaxRepeat * 1000), Timeout.Infinite);
                    state.SetTimer(timer);

                    // Save effect
                    if (!_repeatingEffects.TryGetValue(player, out var repeatEffects))
                    {
                        repeatEffects = new List<RepeatingEffect>() { state };
                        _repeatingEffects.Add(player, repeatEffects);
                    }
                    else
                    {
                        repeatEffects.Add(state);
                    }
                }
            }
        }

        public void StopRepeatingEffects(Player player, HighlightedZone zone, IEnumerable<AmbianceEffect> effects)
        {
            if (!_repeatingEffects.TryGetValue(player, out var repeatEffects))
                return;

            List<RepeatingEffect> effectsToCancel = repeatEffects
                .Where(effect => effect.Zone == zone && effects.Contains(effect.Effect))
                .ToList();

            foreach (var effect in effectsToCancel)
            {
                effect.Timer?.Dispose();

                repeatEffects.Remove(effect);
            }
        }

        private void OnTimerCallback(object state)
        {
            RepeatingEffect repeatingEffect = (RepeatingEffect)state;

            repeatingEffect.Timer?.Change((int)Random.Range(repeatingEffect.Effect.MinRepeat * 1000, repeatingEffect.Effect.MaxRepeat * 1000), Timeout.Infinite);

            _threadAdapter.RunOnMainThread(() =>
            {
                EffectManager.triggerEffect(repeatingEffect.EffectParams);
            });
        }

        private class RepeatingEffect 
        {
            public HighlightedZone Zone { get; private set; }
            public AmbianceEffect Effect { get; private set; }
            public TriggerEffectParameters EffectParams { get; private set; }
            public Timer? Timer { get; private set; }

            public RepeatingEffect(HighlightedZone zone, AmbianceEffect effect, TriggerEffectParameters effectParams)
            {
                Zone = zone;
                Effect = effect;
                EffectParams = effectParams;
            }

            public void SetTimer(Timer timer)
            {
                Timer = timer;
            }
        }
    }
}
