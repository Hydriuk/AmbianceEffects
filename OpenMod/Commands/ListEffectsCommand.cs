using AmbianceEffects.API;
using AmbianceEffects.Models;
using Cysharp.Threading.Tasks;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;
using System.Linq;

namespace AmbianceEffects.OpenMod.Commands
{
    [CommandParent(typeof(AmbianceZoneCommand))]
    [Command("listeffects")]
    [CommandAlias("leffects")]
    [CommandAlias("le")]
    [CommandSyntax("<zoneName>")]
    [CommandDescription("List all effects of an ambiance zones")]
    internal class ListEffectsCommand : UnturnedCommand
    {
        private readonly IAmbianceStore _ambianceStore;

        public ListEffectsCommand(IServiceProvider serviceProvider, IAmbianceStore ambianceStore) : base(serviceProvider)
        {
            _ambianceStore = ambianceStore;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;

            if (Context.Parameters.Count < 1)
                throw new CommandWrongUsageException(Context);

            AmbianceZone zone = _ambianceStore.Get(Context.Parameters[0])
                ?? throw new UserFriendlyException($"Could not find zone named {Context.Parameters[0]}");

            await user.PrintMessageAsync(string.Join(", ", zone.Effects.Select(zone => zone.Name)));
        }
    }
}