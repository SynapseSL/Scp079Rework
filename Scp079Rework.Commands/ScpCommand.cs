using System.Linq;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Scp",
    Aliases = new [] { "Teammate" },
    Description = "Brings you near another SCP",
    Cooldown = 10f,
    EnergyUsage = 25,
    ExperienceGain = 5f,
    RequiredLevel = 2
)]
public class ScpCommand : Scp079Command
{
    private readonly Scp079Commands _plugin;
    private readonly MapService _map;
    private readonly PlayerService _player;

    public ScpCommand(Scp079Commands plugin, MapService map, PlayerService player)
    {
        _plugin = plugin;
        _map = map;
        _player = player;
    }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        var scps = _player.GetPlayers(x => x.TeamID == (uint)Team.SCP && x.RoleType != RoleType.Scp079);

        if (scps.Count == 0)
        {
            result.StatusCode = CommandStatusCode.Error;
            result.Response = _plugin.Translation.Get(context.Scp079).NoScp;
            return;
        }

        var scp = scps[Random.Range(0, scps.Count)];
        var cams = _map.SynapseCameras.OrderBy(x =>
            Vector3.Distance(x.Camera.transform.position, scp.Position)).ToList();
        cams = cams.Take(5).ToList();
        
        var cam = cams[Random.Range(0, cams.Count)];
        context.Scp079.ScpController.Scp079.Camera = cam;

        result.Response = _plugin.Translation.Get(context.Scp079).Scp;
    }
}