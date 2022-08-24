using System.Collections.Generic;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Synapse3.SynapseModule.Player;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Scan",
    Aliases = new string[] {  },
    Description = "Scans the entire Facility for Targets",
    Cooldown = 60f,
    EnergyUsage = 50,
    ExperienceGain = 10f,
    RequiredLevel = 4
)]
public class ScanCommand : Scp079Command
{
    private readonly Scp079Commands _plugin;
    private readonly PlayerService _player;

    public ScanCommand(Scp079Commands plugin, PlayerService player)
    {
        _plugin = plugin;
        _player = player;
    }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        var msg = "\n" + _plugin.Translation.Get(context.Scp079).Scan;
        foreach (var player in _player.Players)
        {
            msg += $"\n{player.RoleType} - {player.Room.Name}";
        }

        result.Response = msg;
    }
}