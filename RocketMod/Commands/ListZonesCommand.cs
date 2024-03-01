using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using UHighlight.RocketMod;
using UHighlight.RocketMod.Adapters;
using UnityEngine;

namespace AmbianceEffects.RocketMod.Commands
{
    internal class ListZonesCommand
    {
        public static async void Execute(UnturnedPlayer uPlayer, string[] command)
        {
            HighlightCommands highlightCommands = new HighlightCommands();

            await highlightCommands.ExecuteVolumes(uPlayer.Player, Constants.GROUP_NAME);
        }
    }
}
