using Synapse.Command;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class FlashCommand : I079Command
    {
        public KeyCode Key => KeyCode.Alpha3;

        public int RequiredLevel => 1;

        public float Energy => 50f;

        public string Name => "flash";

        public string Description => "Throws a flash at your camera";

        public float Exp => 5f;

        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.RoleID == 79)
                return new CommandResult
                {
                    Message = "As Scp079-robot you can't use this Command",
                    State = CommandResultState.Error
                };


            Synapse.Api.Map.Get.Explode(context.Player.Position,Synapse.Api.Enum.GrenadeType.Flashbang,context.Player);

            return new CommandResult
            {
                Message = "All players who looked at you should now be flashed",
                State = CommandResultState.Ok
            };
        }
    }
}
