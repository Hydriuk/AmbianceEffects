using AmbianceEffects.API;
using AmbianceEffects.Models;
using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;
using UHighlight.API;

namespace AmbianceEffects.OpenMod.Commands
{
    [CommandParent(typeof(AmbianceZoneCommand))]
    [Command("validatezone")]
    [CommandAlias("validate")]
    [CommandAlias("v")]
    [CommandSyntax("<zoneName>")]
    [CommandDescription("Confirms the new ambiance zone creation")]
    [CommandActor(typeof(UnturnedUser))]
    internal class ValidateZoneCommand : UnturnedCommand
    {
        private readonly IHighlightCommands _highlightCommands;
        private readonly IAmbianceStore _ambianceStore;

        public ValidateZoneCommand(IServiceProvider serviceProvider, IHighlightCommands highlightCommands, IAmbianceStore ambianceStore) : base(serviceProvider)
        {
            _highlightCommands = highlightCommands;
            _ambianceStore = ambianceStore;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;

            if (Context.Parameters.Length < 1)
                throw new CommandWrongUsageException(Context);

            string zoneName = Context.Parameters[0];

            await _highlightCommands.ExecuteValidate(user.Player.Player, Constants.GROUP_NAME, zoneName);

            _ambianceStore.Create(new AmbianceZone(zoneName));
        }
    }
}