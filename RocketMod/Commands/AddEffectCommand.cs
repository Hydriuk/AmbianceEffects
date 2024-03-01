using AmbianceEffects.Models;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using UHighlight.Models;
using UHighlight.RocketMod;
using UnityEngine;

namespace AmbianceEffects.RocketMod.Commands
{
    internal class AddEffectCommand
    {
        public static void Execute(UnturnedPlayer uPlayer, string[] command)
        {
            if (command.Length < 4)
            {
                ChatManager.serverSendMessage("Wrong syntax: <zoneName> <effectName> <effectGuid> { OnEnter | OnExit } [<radius>] [-owneronly] [-repeat <minRepeat> <maxRepeat>]", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            string zoneName = command[0];
            string effectName = command[1];

            if (!Guid.TryParse(command[2], out Guid effectGuid) || Assets.find(effectGuid) == null)
            {
                ChatManager.serverSendMessage("effectGuid is not a valid guid", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            if (!Enum.TryParse(command[3], out ZoneProperty.EEvent zoneEvent))
            {
                ChatManager.serverSendMessage($"Could not parse 4th argument. It must be {ZoneProperty.EEvent.OnEnter} or {ZoneProperty.EEvent.OnExit}", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            float radius = 256f;
            bool owneronly = false;
            float minRepeat = -1f;
            float maxRepeat = -1f;
            for (int i = 4; i < Math.Min(command.Length, 9); i++)
            {
                string parameter = command[i];

                if (parameter == "-owneronly")
                {
                    owneronly = true;
                }
                else if (parameter == "-repeat")
                {
                    if(!float.TryParse(command[++i], out minRepeat) || !float.TryParse(command[++i], out maxRepeat))
                    {
                        ChatManager.serverSendMessage("Could not parse repeat values", Color.red, toPlayer: uPlayer.SteamPlayer());
                        return;
                    }

                    if (minRepeat < 0 || maxRepeat <= 0)
                    {
                        ChatManager.serverSendMessage("minRepeat must be >= 0 and maxRepeat must be > 0", Color.red, toPlayer: uPlayer.SteamPlayer());
                        return;
                    }
                }
                else
                {
                    if (!float.TryParse(command[i], out radius))
                    {
                        ChatManager.serverSendMessage("Could not parse radius", Color.red, toPlayer: uPlayer.SteamPlayer());
                        return;
                    }
                }
            }

            AmbianceZone zone = Plugin.Instance.AmbianceStore.Get(zoneName);

            if (zone.Effects.Any(effect => effect.Name == effectName))
            {
                ChatManager.serverSendMessage($"Name {effectName} is already used", Color.red, toPlayer: uPlayer.SteamPlayer());
                return;
            }

            zone.Effects.Add(new AmbianceEffect()
            {
                Name = effectName,
                Position = uPlayer.Player.transform.position,
                EffectGUID = effectGuid,
                Event = zoneEvent,
                VisibiltyRadius = radius,
                OwnerOnly = owneronly,
                MinRepeat = minRepeat,
                MaxRepeat = maxRepeat
            });

            Plugin.Instance.AmbianceStore.Update(zone);

            Task.Run(Plugin.Instance.AmbianceSpawner.ReloadZones);
        }
    }
}
