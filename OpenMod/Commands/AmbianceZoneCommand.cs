using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using System;

namespace AmbianceEffects.OpenMod.Commands
{
    [Command("ambiancezone")]
    [CommandAlias("ambz")]
    internal class AmbianceZoneCommand : UnturnedCommand
    {
        public AmbianceZoneCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override UniTask OnExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}