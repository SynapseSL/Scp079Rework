using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Synapse3.SynapseModule.Map;
using UnityEngine;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Random",
    Aliases = new [] { "Luck" },
    Description = "Sets you into a random Camera somewhere in the Facility",
    Cooldown = 2f,
    EnergyUsage = 10,
    ExperienceGain = 5f,
    RequiredLevel = 1
)]
public class RandomCommand : Scp079Command
{
    private readonly Scp079Commands _plugin;
    private readonly MapService _map;

    public RandomCommand(Scp079Commands plugin, MapService map)
    {
        _plugin = plugin;
        _map = map;
    }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        var cams = _map.SynapseCameras;
        var cam = cams[Random.Range(0, cams.Count)];
        context.Scp079.ScpController.Scp079.Camera = cam;

        result.Response = _plugin.Translation.Get(context.Scp079).Random;
    }
}