using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Ninject;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Map;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Death",
    Aliases = new [] { "ScpDeath" },
    Description = "Announces the Death of one SCP",
    Cooldown = 120f,
    EnergyUsage = 50,
    ExperienceGain = 2,
    RequiredLevel = 2
)]
public class DeathCommand : Scp079Command
{
    [Inject]
    public Scp079Commands Plugin { get; set; }
    
    [Inject]
    public CassieService Cassie { get; set; }

    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        if (context.Arguments.Length == 0 || string.IsNullOrWhiteSpace(context.Arguments[0]))
        {
            result.StatusCode = CommandStatusCode.BadSyntax;
            result.Response = Plugin.Translation.Get(context.Scp079).DeathNoNumber;
            return;
        }

        if (context.Arguments[0].Length > 5)
        {
            result.StatusCode = CommandStatusCode.BadSyntax;
            result.Response = Plugin.Translation.Get(context.Scp079).DeathToLong;
            return;
        }
        
        Cassie.AnnounceScpDeath(context.Arguments[0], CassieSettings.Glitched, CassieSettings.Noise);
        result.Response = Plugin.Translation.Get(context.Scp079).Death;
    }
}