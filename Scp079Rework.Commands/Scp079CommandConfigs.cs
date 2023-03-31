using System;
using Neuron.Core.Meta;
using Syml;

namespace Scp079Rework.Commands;

[Automatic]
[Serializable]
[DocumentSection("SCP 079 Commands")]
public class Scp079CommandConfigs : IDocumentSection
{
    public float LockdownDuration { get; set; } = 10f;

    public string LockdownCassie { get; set; } = "Facility Lockdown";

    public bool AllowChangingExplodeTime { get; set; } = true;
    
    public bool AllowChangingFlashTime { get; set; } = true;
    
    public bool AllowInstantExplode { get; set; } = true;

    public string OverchargeCassie { get; set; } = "Warning , all Facility power will return to Heavy Zone in 3 . 2 . 1";

    public float OverchargeDelay { get; set; } = 10.5f;

    public float OverchargeTime { get; set; } = 20f;
}