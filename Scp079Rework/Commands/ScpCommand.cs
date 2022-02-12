using Synapse;
using Synapse.Api;
using Synapse.Command;
using System.Linq;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class ScpCommand : I079Command
    {
        public KeyCode Key => KeyCode.Alpha4;

        public int RequiredLevel => PluginExtensions.GetRequiredLevel(Name, 2);

        public float Energy => PluginExtensions.GetEnergy(Name, 50f);

        public float Exp => PluginExtensions.GetExp(Name, 1f);

        public string Name => "scp";

        public string Description => "brings you near a other Scp";

        public float Cooldown => PluginExtensions.GetCooldown(Name, 0f);

        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.RoleID == 79)
                return new CommandResult
                {
                    Message = PluginClass.Translation.ActiveTranslation.NotAsRobot,
                    State = CommandResultState.Error
                };

            var scps = Server.Get.GetPlayers(x => x.TeamID == (int)Team.SCP && x.RoleType != RoleType.Scp079);
            if(scps.Count > 0)
            {
                var scp = scps[Random.Range(0, scps.Count)];
                var cams = Map.Get.Cameras.OrderBy(x => Vector3.Distance(x.GameObject.transform.position, scp.Position)).ToList();
                cams = cams.Take(5).ToList();
                var cam = cams[Random.Range(0, cams.Count)];

                context.Player.Scp079Controller.Camera = cam;
                return new CommandResult
                {
                    Message = "You camera was switched near a other Scp",
                    State = CommandResultState.Ok
                };
            }

            return new CommandResult
            {
                Message = "No Scp near a camera was found!",
                State = CommandResultState.Error
            };
        }
    }
}
