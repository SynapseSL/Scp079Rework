using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Map;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Death",
    Aliases = new [] { "ScpDeath" },
    Description = "Announces the Death of one SCP",
    Cooldown = 120f,
    EnergyUsage = 40,
    ExperienceGain = 10f,
    RequiredLevel = 2
)]
public class DeathCommand : Scp079Command
{
    private readonly Scp079Commands _plugin;
    private readonly CassieService _cassie;

    public DeathCommand(Scp079Commands plugin, CassieService cassie)
    {
        _plugin = plugin;
        _cassie = cassie;
    }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        if (context.Arguments.Length == 0 || string.IsNullOrWhiteSpace(context.Arguments[0]))
        {
            result.StatusCode = CommandStatusCode.BadSyntax;
            result.Response = _plugin.Translation.Get(context.Scp079).DeathNoNumber;
            return;
        }

        if (context.Arguments[0].Length > 5)
        {
            result.StatusCode = CommandStatusCode.BadSyntax;
            result.Response = _plugin.Translation.Get(context.Scp079).DeathToLong;
            return;
        }
        
        _cassie.AnnounceScpDeath(context.Arguments[0], CassieSettings.Glitched, CassieSettings.Noise);
        result.Response = _plugin.Translation.Get(context.Scp079).Death;
    }
}