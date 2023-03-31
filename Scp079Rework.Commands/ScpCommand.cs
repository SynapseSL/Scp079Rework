using System.Linq;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Ninject;
using PlayerRoles;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079CommandKeyBind(
    CommandName = "Scp",
    Aliases = new [] { "Teammate" },
    Description = "Brings you near another SCP",
    Cooldown = 10f,
    EnergyUsage = 25,
    ExperienceGain = 1,
    RequiredLevel = 2,
    DefaultKey = KeyCode.Keypad5
)]
public class ScpCommand : Scp079CommandKeyBind
{
    [Inject]
    public Scp079Commands Plugin { get; set; }
    
    [Inject]
    public MapService Map { get; set; }
    
    [Inject]
    public PlayerService Player { get; set; }

    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        var scps = Player.GetPlayers(x => x.TeamID == (uint)Team.SCPs && x.RoleType != RoleTypeId.Scp079);

        if (scps.Count == 0)
        {
            result.StatusCode = CommandStatusCode.Error;
            result.Response = Plugin.Translation.Get(context.Scp079).NoScp;
            return;
        }

        var scp = scps[Random.Range(0, scps.Count)];
        var cams = Map.SynapseCameras.OrderBy(x =>
            Vector3.Distance(x.Camera.transform.position, scp.Position)).ToList();
        cams = cams.Take(5).ToList();
        
        var cam = cams[Random.Range(0, cams.Count)];
        context.Scp079.MainScpController.Scp079.Camera = cam;

        result.Response = Plugin.Translation.Get(context.Scp079).Scp;
    }

    public override bool ExecuteBind(SynapsePlayer scp079)
    {
        var scps = Player.GetPlayers(x => x.TeamID == (uint)Team.SCPs && x.RoleType != RoleTypeId.Scp079);
        if (scps.Count == 0) return false;

        var scp = scps[Random.Range(0, scps.Count)];
        var cams = Map.SynapseCameras.OrderBy(x =>
            Vector3.Distance(x.Camera.transform.position, scp.Position)).ToList();
        cams = cams.Take(5).ToList();
        
        var cam = cams[Random.Range(0, cams.Count)];
        scp079.MainScpController.Scp079.Camera = cam;
        return true;
    }
}