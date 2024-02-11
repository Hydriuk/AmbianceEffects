using AmbianceEffects.API;
using AmbianceEffects.Models;
#if OPENMOD
using Microsoft.Extensions.DependencyInjection;
using OpenMod.API.Ioc;
#endif
using Hydriuk.UnturnedModules.Extensions;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmbianceEffects.Services
{
#if OPENMOD
    [PluginServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
#endif
    internal class EffectSpawner : IEffectSpawner
    {
        private readonly Dictionary<Player, List<AmbianceEffect>> _repeatingEffects = new Dictionary<Player, List<AmbianceEffect>>();

        public void Dispose()
        {
            
        }

        public void SpawnEffects(Player player, IEnumerable<AmbianceEffect> effects)
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
            }
        }

        public void StopRepeatingEffects()
        {

        }
    }
}
