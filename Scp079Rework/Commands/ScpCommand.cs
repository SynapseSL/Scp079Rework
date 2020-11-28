using Synapse.Command;
using Synapse;
using System.Collections.Generic;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class ScpCommand : I079Command
    {
        public KeyCode Key => KeyCode.Alpha4;

        public int RequiredLevel => 2;

        public float Energy => 50f;

        public float Exp => 1f;

        public string Name => "scp";

        public string Description => "brings you near a other Scp";

        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.RoleID == 79)
                return new CommandResult
                {
                    Message = "As Scp079-robot you can't use this Command",
                    State = CommandResultState.Error
                };

            var cams = GetCameras();
            if(cams.Count > 0)
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

        private List<Camera079> GetCameras()
        {
            var list = Server.Get.GetObjectsOf<Camera079>();
            var cams = new List<Camera079>();

            foreach (var player in Server.Get.GetPlayers(x => x.Team == Team.SCP && x.RoleID != (int)RoleType.Scp079))
                foreach (var cam in list)
                    if (Vector3.Distance(cam.transform.position, player.Position) <= 15f)
                        cams.Add(cam);

            return cams;    
        }
    }
}
