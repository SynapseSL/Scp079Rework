using System;
using Neuron.Core.Meta;
using Syml;

namespace Scp079Rework.Commands;

[Automatic]
[Serializable]
[DocumentSection("SCP 079 Commands")]
public class Scp079CommandConfigs : IDocumentSection
{
    public float BlackoutDuration { get; set; } = 10f;

    public string BlackoutCassie { get; set; } = "Facility Black out";
    
    public float LockdownDuration { get; set; } = 10f;

    public string LockdownCassie { get; set; } = "Facility Lockdown";

    public bool AllowChangingExplodeTime { get; set; } = true;
    
    public bool AllowChangingFlashTime { get; set; } = true;
}