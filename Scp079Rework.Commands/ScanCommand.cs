using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Ninject;
using Synapse3.SynapseModule.Player;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Scan",
    Aliases = new string[] {  },
    Description = "Scans the entire Facility for Targets",
    Cooldown = 60f,
    EnergyUsage = 50,
    ExperienceGain = 0,
    RequiredLevel = 4
)]
public class ScanCommand : Scp079Command
{
    [Inject]
    public Scp079Commands Plugin { get; set; }
    
    [Inject]
    public PlayerService Player { get; set; }

    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        var msg = "\n" + Plugin.Translation.Get(context.Scp079).Scan;
        foreach (var player in Player.Players)
        {
            msg += $"\n{player.RoleType} - {player.Room.Name}";
        }

        result.Response = msg;
    }
}