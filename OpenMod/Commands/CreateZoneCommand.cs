using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;
using UHighlight.API;

namespace AmbianceEffects.OpenMod.Commands
{
    [CommandParent(typeof(AmbianceZoneCommand))]
    [Command("createzone")]
    [CommandAlias("create")]
    [CommandAlias("c")]
    [CommandSyntax("[cube | c | sphere | s]")]
    [CommandDescription("Starts a new ambiance zone creation")]
    [CommandActor(typeof(UnturnedUser))]
    internal class CreateZoneCommand : UnturnedCommand
    {
        private readonly IHighlightCommands _highlightCommands;

        public CreateZoneCommand(IServiceProvider serviceProvider, IHighlightCommands highlightCommands) : base(serviceProvider)
        {
            _highlightCommands = highlightCommands;
        }

        protected override async UniTask OnExecuteAsync()
        {
            UnturnedUser user = (UnturnedUser)Context.Actor;

            if (!Context.Parameters.TryGet(0, out string? shape))
                shape = "s";

            switch (shape)
            {
                case "cube":
                case "c":
                    await _highlightCommands.ExecuteCreate(user.Player.Player, "Cube", "Transparent", "Blue");
                    break;

                case "sphere":
                case "s":
                default:
                    await _highlightCommands.ExecuteCreate(user.Player.Player, "Sphere", "Transparent", "Blue");
                    break;
            }
        }
    }
}