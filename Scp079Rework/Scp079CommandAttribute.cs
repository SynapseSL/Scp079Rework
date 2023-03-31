using Neuron.Modules.Commands.Command;
using UnityEngine;

namespace Scp079Rework;

public class Scp079CommandAttribute : CommandAttribute
{
    public int RequiredLevel { get; set; } = 0;

    public float EnergyUsage { get; set; } = 1f;

    public int ExperienceGain { get; set; } = 0;

    public float Cooldown { get; set; } = 0f;
}

public class Scp079CommandKeyBindAttribute : Scp079CommandAttribute
{
    public KeyCode DefaultKey { get; set; } = KeyCode.Alpha1;
}