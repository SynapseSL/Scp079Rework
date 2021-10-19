using Synapse.Api.Enum;
using Synapse.Command;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class FlashCommand : I079Command
    {
        public KeyCode Key => KeyCode.Alpha3;

        public int RequiredLevel => PluginExtensions.GetRequiredLevel(Name, 1);

        public float Energy => PluginExtensions.GetEnergy(Name, 50f);

        public string Name => "flash";

        public string Description => "Throws a flash at your camera";

        public float Exp => PluginExtensions.GetExp(Name, 5f);

        public float Cooldown => PluginExtensions.GetCooldown(Name, 2f);

        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.RoleID == 79)
                return new CommandResult
                {
                    Message = PluginClass.Translation.ActiveTranslation.NotAsRobot,
                    State = CommandResultState.Error
                };


            Synapse.Api.Map.Get.Explode(context.Player.Position, GrenadeType.Flashbang, context.Player);

            return new CommandResult
            {
                Message = "All players who looked at you should now be flashed",
                State = CommandResultState.Ok
            };
        }
    }
}
