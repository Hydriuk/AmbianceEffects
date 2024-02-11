using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UHighlight.RocketMod.Commands.Test;
using UHighlight.RocketMod.Commands;
using UnityEngine;
using Steamworks;

namespace AmbianceEffects.RocketMod.Commands
{
    internal class AmbianceZoneCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "ambiancezone";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string>() { "ambz" };

        public List<string> Permissions => new List<string>() { "ambiancezone.admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer uPlayer = (UnturnedPlayer)caller;

            string[] subCommand = command.Skip(1).ToArray();

            switch (command[0])
            {
                case "addeffect":
                case "add":
                    AddEffectCommand.Execute(uPlayer, subCommand);
                    break;

                case "createzone":
                case "create":
                case "c":
                    CreateZoneCommand.Execute(uPlayer, subCommand);
                    break;

                case "deletezone":
                case "delete":
                case "d":
                    DeleteZoneCommand.Execute(uPlayer, subCommand);
                    break;

                case "listeffects":
                case "le":
                    ListEffectsCommand.Execute(uPlayer, subCommand);
                    break;

                case "listzones":
                case "lz":
                    ListZonesCommand.Execute(uPlayer, subCommand);
                    break;

                case "removeeffects":
                case "remove":
                case "r":
                    RemoveEffectCommand.Execute(uPlayer, subCommand);
                    break;

                case "validatezone":
                case "validate":
                case "v":
                    ValidateZoneCommand.Execute(uPlayer, subCommand);
                    break;

                default:
                    ChatManager.serverSendMessage("Wrong syntax : ambz parameter not known", Color.red, toPlayer: uPlayer.SteamPlayer());
                    return;
            }
        }
    }
}
