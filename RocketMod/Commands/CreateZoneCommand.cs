using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UHighlight.RocketMod;
using UnityEngine;

namespace AmbianceEffects.RocketMod.Commands
{
    internal class CreateZoneCommand
    {
        public static void Execute(UnturnedPlayer uPlayer, string[] command)
        {
            string shape;
            if (command.Length > 0)
                shape = command[0];
            else
                shape = "s";

            switch (shape)
            {
                case "cube":
                case "c":
                    Plugin.Instance.HighlightCommands.ExecuteCreate(uPlayer.Player, "Cube", "Transparent", "Blue");
                    break;

                case "sphere":
                case "s":
                default:
                    Plugin.Instance.HighlightCommands.ExecuteCreate(uPlayer.Player, "Sphere", "Transparent", "Blue");
                    break;
            }
        }
    }
}
