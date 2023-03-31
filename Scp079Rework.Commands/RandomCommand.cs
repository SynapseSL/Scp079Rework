using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Ninject;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079CommandKeyBind(
    CommandName = "Random",
    Aliases = new [] { "Luck" },
    Description = "Sets you into a random Camera somewhere in the Facility",
    Cooldown = 2f,
    EnergyUsage = 10,
    ExperienceGain = 1,
    RequiredLevel = 1,
    DefaultKey = KeyCode.Keypad4
)]
public class RandomCommand : Scp079CommandKeyBind
{
    [Inject]
    public Scp079Commands Plugin { get; set; }
    
    [Inject]
    public MapService Map { get; set; }

    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        ExecuteBind(context.Scp079);
        result.Response = Plugin.Translation.Get(context.Scp079).Random;
    }

    public override bool ExecuteBind(SynapsePlayer scp079)
    {
        var cams = Map.SynapseCameras;
        var cam = cams[Random.Range(0, cams.Count)];
        scp079.MainScpController.Scp079.Camera = cam;
        return true;
    }
}