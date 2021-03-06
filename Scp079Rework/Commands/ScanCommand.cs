﻿using Synapse.Command;
using Synapse;
using UnityEngine;
using Synapse.Api.Enum;

namespace Scp079Rework.Commands
{
    public class ScanCommand : I079Command
    {
        public KeyCode Key => KeyCode.None;

        public int RequiredLevel => PluginExtensions.GetRequiredLevel(Name, 3);

        public float Energy => PluginExtensions.GetEnergy(Name, 100f);

        public float Exp => PluginExtensions.GetExp(Name, 5f);

        public string Name => "scan";

        public string Description => "Shows you where youre Targets are";

        public float Cooldown => PluginExtensions.GetCooldown(Name, 0f);

        public CommandResult Execute(CommandContext context)
        {
            var layout = "\nCurrently living creatures in\n  LCZ:[LCZ]\n  HCZ:[HCZ]\n  EZ:[EZ]\n  Surface:[SURFACE]\n  Pocket:[POCKET]";

            var lcz = "";
            var hcz = "";
            var ez = "";
            var surface = "";
            var pocket = "";
            foreach(var player in Server.Get.Players)
            {
                if (player.RoleType == RoleType.Spectator || player.RoleType == RoleType.None || player.RoleID == (int)RoleType.Tutorial) continue;

                switch (player.Room?.Zone)
                {
                    case ZoneType.LCZ:
                        lcz += $"\n    - {player} : {player.RoleName}";
                        break;

                    case ZoneType.HCZ:
                        hcz += $"\n    - {player} : {player.RoleName}";
                        break;

                    case ZoneType.Entrance:
                        ez += $"\n    - {player} : {player.RoleName}";
                        break;

                    case ZoneType.Surface:
                        surface += $"\n    - {player} : {player.RoleName}";
                        break;

                    case ZoneType.Pocket:
                        pocket += $"\n    - {player} : {player.RoleName}";
                        break;
                }
            }

            layout = layout.Replace("[LCZ]", lcz);
            layout = layout.Replace("[HCZ]", hcz);
            layout = layout.Replace("[EZ]", ez);
            layout = layout.Replace("[SURFACE]", surface);
            layout = layout.Replace("[POCKET]", pocket);

            return new CommandResult
            {
                Message = layout,
                State = CommandResultState.Ok
            };
        }
    }
}
