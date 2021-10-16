using System.Collections.Generic;
using System.Linq;
using Synapse.Api;
using Synapse.Command;
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

            var cams = Map.Get.Cameras;
            var scpCams = new List<Synapse.Api.Camera>();

            foreach (var scp in Team.SCP.GetPlayers())
                if (scp.RoleType != RoleType.Scp079)
                    scpCams.Add(cams.OrderBy(x => Vector3.Distance(x.GameObject.transform.position, scp.Position)).FirstOrDefault());

            if(scpCams.Count > 0)
            {
                var cam = cams[UnityEngine.Random.Range(0, cams.Count)];
                context.Player.Scp079Controller.Camera = cam;
                return new CommandResult
                {
                    Message = "You camera was switched near a other Scp",
                    State = CommandResultState.Ok
                };
            }
            else
            {
                return new CommandResult
                {
                    Message = "No Scp near a camera was found!",
                    State = CommandResultState.Error
                };
            }
        }
    }
}
