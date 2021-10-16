using Synapse.Command;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class ExplodeCommand : I079Command
    {
        public KeyCode Key => KeyCode.Alpha5;

        public int RequiredLevel => PluginExtensions.GetRequiredLevel(Name, 4);

        public float Energy => PluginExtensions.GetEnergy(Name, 150f);

        public float Exp => PluginExtensions.GetExp(Name, 20f);

        public string Name => "explode";

        public string Description => "Spawns a greande at your current camera";

        public float Cooldown => PluginExtensions.GetCooldown(Name, 15f);

        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.RoleID == 79)
                return new CommandResult
                {
                    Message = PluginClass.Translation.ActiveTranslation.NotAsRobot,
                    State = CommandResultState.Error
                };


            Synapse.Api.Map.Get.Explode(context.Player.Position, Synapse.Api.Enum.GrenadeType.Grenade, context.Player);

            return new CommandResult
            {
                Message = "Grenade spawned",
                State = CommandResultState.Ok
            };
        }
    }
}
