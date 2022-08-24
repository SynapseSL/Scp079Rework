using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Map;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Blackout",
    Aliases = new [] { "LightsOut" },
    Description = "Turns of all Lights",
    Cooldown = 120f,
    EnergyUsage = 100,
    ExperienceGain = 20f,
    RequiredLevel = 3
)]
public class BlackoutCommand : Scp079Command
{
    private readonly HeavyZoneService _heavy;
    private readonly CassieService _cassie;
    private readonly Scp079Commands _plugin;

    public BlackoutCommand(HeavyZoneService heavy, CassieService cassie, Scp079Commands plugin)
    {
        _heavy = heavy;
        _cassie = cassie;
        _plugin = plugin;
    }
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        _heavy.LightsOut(_plugin.Config.BlackoutDuration, false);

        if (!string.IsNullOrWhiteSpace(_plugin.Config.BlackoutCassie))
            _cassie.Announce(_plugin.Config.BlackoutCassie, CassieSettings.DisplayText, CassieSettings.Glitched);

        result.Response = _plugin.Translation.Get(context.Scp079).Blackout;
    }
}