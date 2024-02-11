using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;
using UHighlight.API;

namespace AmbianceEffects.OpenMod.Commands
{
    [CommandParent(typeof(AmbianceZoneCommand))]
    [Command("listzones")]
    [CommandAlias("lzones")]
    [CommandAlias("lz")]
    [CommandSyntax("")]
    [CommandDescription("List all ambiance zones")]
    internal class ListZonesCommand : UnturnedCommand
    {
        private readonly IHighlightCommands _highlightCommands;

        public ListZonesCommand(IServiceProvider serviceProvider, IHighlightCommands highlightCommands) : base(serviceProvider)
        {
            _highlightCommands = highlightCommands;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;

            await _highlightCommands.ExecuteVolumes(user.Player.Player, Constants.GROUP_NAME);
        }
    }
}