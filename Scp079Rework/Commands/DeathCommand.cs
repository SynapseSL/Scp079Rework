using Synapse.Command;
using System.Linq;
using Synapse.Api;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class DeathCommand : I079Command
    {
        public KeyCode Key => KeyCode.None;

        public int RequiredLevel => 3;

        public float Energy => 100f;

        public float Exp => 20;

        public string Name => "death";

        public string Description => "sends a fake scp death announcement";

        public float Cooldown => 20f;

        public CommandResult Execute(CommandContext context)
        {
            if (context.Arguments.Count == 0 || string.IsNullOrWhiteSpace(context.Arguments.First()))
                return new CommandResult
                {
                    Message = "You have to enter a Scp number with spaces between!\nExample: .079 death 1 7 3",
                    State = CommandResultState.NoPermission,
                };

            Map.Get.AnnounceScpDeath(string.Join(" ", context.Arguments));

            return new CommandResult
            {
                Message = "Fake Scp death announcement was send",
                State = CommandResultState.Ok,
            };
        }
    }
}
