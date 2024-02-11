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
    internal class RemoveEffectCommand
    {
        public static void Execute(UnturnedPlayer uPlayer, string[] command)
        {
            if (command.Length < 2)
            {
                ChatManager.serverSendMessage("Wrong syntax: <zoneName> <effectName>", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            string zoneName = command[0];
            string effectName = command[1];

            AmbianceZone zone = Plugin.Instance.AmbianceStore.Get(zoneName);

            if(zone == null)
            {
                ChatManager.serverSendMessage($"Zone {zoneName} not found", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            AmbianceEffect effect = zone.Effects.FirstOrDefault(effect => effect.Name == effectName);

            if(effect == null)
            {
                ChatManager.serverSendMessage($"Effect {zoneName} not found in zone {zoneName}", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            Plugin.Instance.AmbianceStore.Update(zone);

            Plugin.Instance.AmbianceSpawner.ReloadZones();
        }
    }
}
