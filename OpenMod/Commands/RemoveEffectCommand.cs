using AmbianceEffects.API;
using AmbianceEffects.Models;
using Cysharp.Threading.Tasks;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using System.Linq;

namespace AmbianceEffects.OpenMod.Commands
{
    [CommandParent(typeof(AmbianceZoneCommand))]
    [Command("removeeffect")]
    [CommandAlias("remove")]
    [CommandAlias("r")]
    [CommandSyntax("<zoneName> <effectName>")]
    [CommandDescription("Removes an effect from an ambiance zone")]
    internal class RemoveEffectCommand : UnturnedCommand
    {
        private readonly IAmbianceStore _zoneStore;
        private readonly IAmbianceSpawner _ambianceSpawner;

        public RemoveEffectCommand(IServiceProvider serviceProvider, IAmbianceStore ambianceStore, IAmbianceSpawner ambianceSpawner) : base(serviceProvider)
        {
            _zoneStore = ambianceStore;
            _ambianceSpawner = ambianceSpawner;
        }

        protected override UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;

            if (Context.Parameters.Length < 2)
                throw new CommandWrongUsageException(Context);

            string zoneName = Context.Parameters[0];
            string effectName = Context.Parameters[1];

            AmbianceZone zone = _zoneStore.Get(zoneName)
                ?? throw new UserFriendlyException($"Zone {zoneName} not found");

            AmbianceEffect effect = zone.Effects.FirstOrDefault(effect => effect.Name == effectName)
                ?? throw new UserFriendlyException($"Effect {zoneName} not found in zone {zoneName}");

            _zoneStore.Update(zone);

            _ambianceSpawner.ReloadZones();

            return UniTask.CompletedTask;
        }
    }
}