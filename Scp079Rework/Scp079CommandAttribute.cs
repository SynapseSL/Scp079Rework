using Neuron.Modules.Commands.Command;

namespace Scp079Rework;

public class Scp079CommandAttribute : CommandAttribute
{
    public int RequiredLevel { get; set; } = 0;

    public float EnergyUsage { get; set; } = 1f;

    public float ExperienceGain { get; set; } = 0f;

    public float Cooldown { get; set; } = 0f;
}