using Synapse.Api;
using Synapse.Command;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class RandomCommand : I079Command
    {
        public KeyCode Key => KeyCode.Alpha6;

        public int RequiredLevel => PluginExtensions.GetRequiredLevel(Name, 1);

        public float Energy => PluginExtensions.GetEnergy(Name, 10f);

        public float Exp => PluginExtensions.GetExp(Name, 3f);

        public string Name => "random";

        public string Description => "brings you to a random Camera on the map";

        public float Cooldown => PluginExtensions.GetCooldown(Name, 5f);

        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.RoleID == 79)
                return new CommandResult
                {
                    Message = PluginClass.Translation.ActiveTranslation.NotAsRobot,
                    State = CommandResultState.Error
                };

            var cams = Map.Get.Cameras;

            if (cams.Count > 0)
            {
                var cam = cams[UnityEngine.Random.Range(0, cams.Count)];
                context.Player.Scp079Controller.Camera = cam;
                return new CommandResult
                {
                    Message = "You camera was switched",
                    State = CommandResultState.Ok
                };
            }
            else
            {
                return new CommandResult
                {
                    Message = "No Camera was found!",
                    State = CommandResultState.Error
                };
            }
        }
    }
}
