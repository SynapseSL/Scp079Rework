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
    ExperienceGain = 40f,
    RequiredLevel = 3
)]
public class BlackoutCommand : Scp079Command
{
    private readonly HeavyZoneService _heavy;
    private readonly CassieService _cassie;

    public BlackoutCommand(HeavyZoneService heavy, CassieService cassie)
    {
        _heavy = heavy;
        _cassie = cassie;
    }
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        _heavy.LightsOut(10f, false);
        _cassie.Announce("Facility Black out", CassieSettings.DisplayText, CassieSettings.Glitched);

        result.Response = "The Facility is now in Blackout for 10 seconds";
    }
}