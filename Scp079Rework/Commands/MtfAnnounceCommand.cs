using Synapse.Command;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class MtfAnnounceCommand : I079Command
    {
        public int RequiredLevel => 2;

        public float Energy => 50;

        public string Name => "mtf";

        public string Description => "Sends a fake Mtf Announcement";

        public KeyCode Key => KeyCode.Alpha2;

        public float Exp => 5f;

        public float Cooldown => 15f;

        public CommandResult Execute(CommandContext context)
        {
            string letter;
            switch (UnityEngine.Random.Range(0f, 6f))
            {
                case 1f: letter = "a"; break;
                case 2f: letter = "b"; break;
                case 3f: letter = "d"; break;
                case 4f: letter = "e"; break;
                case 5f: letter = "i"; break;
                case 6f: letter = "q"; break;
                default: letter = "z"; break;
            }

            var scps = SynapseController.Server.GetPlayers(x => x.RealTeam == Team.SCP).Count;
            Synapse.Api.Map.Get.Cassie($"MTFUnit Epsilon 11 designated Nato_{letter} 05 HasEntered AllRemaining AwaitingRecontainment {scps} ScpSubjects");

            return new CommandResult
            {
                Message = "Fake Announcement was send",
                State = CommandResultState.Ok,
            };
        }
    }
}
