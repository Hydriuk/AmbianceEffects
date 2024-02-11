using AmbianceEffects.Models;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using UHighlight.RocketMod;
using UnityEngine;

namespace AmbianceEffects.RocketMod.Commands
{
    internal class ValidateZoneCommand
    {
        public static async void Execute(UnturnedPlayer uPlayer, string[] command)
        {
            if (command.Length < 1)
            {
                ChatManager.serverSendMessage("Wrong syntax: <zoneName>", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            string zoneName = command[0];

            await Plugin.Instance.HighlightCommands.ExecuteValidate(uPlayer.Player, Constants.GROUP_NAME, zoneName);

            Plugin.Instance.AmbianceStore.Create(new AmbianceZone(zoneName));
        }
    }
}
