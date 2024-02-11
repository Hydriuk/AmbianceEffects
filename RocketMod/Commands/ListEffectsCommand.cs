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
    internal class ListEffectsCommand
    {
        public static void Execute(UnturnedPlayer uPlayer, string[] command)
        {
            if (command.Length < 1)
            {
                ChatManager.serverSendMessage("Wrong syntax: <zoneName>", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            AmbianceZone zone = Plugin.Instance.AmbianceStore.Get(command[0]);

            if(zone == null)
            {
                ChatManager.serverSendMessage($"Could not find zone named {command[0]}", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            ChatManager.serverSendMessage(string.Join(", ", zone.Effects.Select(zone => zone.Name)), Color.green);
        }
    }
}
