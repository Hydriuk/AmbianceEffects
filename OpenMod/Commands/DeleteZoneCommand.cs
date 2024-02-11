using AmbianceEffects.API;
using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;
using UHighlight.API;

namespace AmbianceEffects.OpenMod.Commands
{
    [CommandParent(typeof(AmbianceZoneCommand))]
    [Command("deletezone")]
    [CommandAlias("delete")]
    [CommandAlias("d")]
    [CommandSyntax("<zoneName>")]
    [CommandDescription("Deletes an ambiance zone")]
    internal class DeleteZoneCommand : UnturnedCommand
    {
        private readonly IHighlightCommands _highlightCommands;
        private readonly IAmbianceStore _ambianceStore;
        private readonly IAmbianceSpawner _ambianceSpawner;

        public DeleteZoneCommand(IServiceProvider serviceProvider, IHighlightCommands highlightCommands, IAmbianceStore ambianceStore, IAmbianceSpawner ambianceSpawner) : base(serviceProvider)
        {
            _highlightCommands = highlightCommands;
            _ambianceStore = ambianceStore;
            _ambianceSpawner = ambianceSpawner;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;

            if (Context.Parameters.Length < 1)
                throw new CommandWrongUsageException(Context);

            string zoneName = Context.Parameters[0];

            await _highlightCommands.ExecuteDelete(user.Player.Player, Constants.GROUP_NAME, zoneName);

            _ambianceStore.Delete(zoneName);

            _ambianceSpawner.ReloadZones();
        }
    }
}