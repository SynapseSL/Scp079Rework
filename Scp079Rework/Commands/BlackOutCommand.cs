using Synapse.Command;
using Synapse.Api;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class BlackOutCommand : I079Command
    {
        public int RequiredLevel => 3;

        public float Energy => 125f;

        public string Name => "blackout";

        public string Description => "Deactivates all Lights in Heavy/Light";

        public KeyCode Key => KeyCode.Alpha1;

        public float Exp => 15f;

        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.RoleID == 79)
                return new CommandResult
                {
                    Message = "As Scp079-robot you can't use this Command",
                    State = CommandResultState.Error
                };

            Generator079.mainGenerator.ServerOvercharge(10f, false);
            Map.Get.GlitchedCassie("Black out through Scp 0 7 9 Now");

            return new CommandResult
            {
                Message = "Blackout are now active for 10 seconds",
                State = CommandResultState.Ok
            };
        }
    }
}
