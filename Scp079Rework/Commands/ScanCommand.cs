using Synapse.Command;
using Synapse;
using UnityEngine;
using Synapse.Api.Enum;

namespace Scp079Rework.Commands
{
    public class ScanCommand : I079Command
    {
        public KeyCode Key => KeyCode.Alpha4;

        public int RequiredLevel => 3;

        public float Energy => 100f;

        public float Exp => 5f;

        public string Name => "scan";

        public string Description => "Shows you where youre Targets are";

        public CommandResult Execute(CommandContext context)
        {
            var layout = "\nCurrently living creatures in\n  LCZ:\n[LCZ]\n  HCZ:\n[HCZ]\n  EZ:\n[EZ]\n  Surface:\n[SURFACE]\n  Pocket:\n[POCKET]";

            var lcz = "";
            var hcz = "";
            var ez = "";
            var surface = "";
            var pocket = "";
            foreach(var player in Server.Get.Players)
            {
                switch (player.Room.Zone)
                {
                    case ZoneType.LCZ:
                        lcz += $"    - {context.Player} : {context.Player.RoleType}";
                        break;

                    case ZoneType.HCZ:
                        hcz += $"    - {context.Player} : {context.Player.RoleType}";
                        break;

                    case ZoneType.Entrance:
                        ez += $"    - {context.Player} : {context.Player.RoleType}";
                        break;

                    case ZoneType.Surface:
                        surface += $"    - {context.Player} : {context.Player.RoleType}";
                        break;

                    case ZoneType.Pocket:
                        pocket += $"    - {context.Player} : {context.Player.RoleType}";
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
