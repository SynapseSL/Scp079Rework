using System;
using System.Collections.Generic;
using System.ComponentModel;
using Neuron.Core.Meta;
using Syml;

namespace Scp079Rework;

[Automatic]
[Serializable]
[DocumentSection("Scp079Rework")]
public class Scp079ReworkConfig : IDocumentSection
{
    [Description("If Enabled every Role can use .079/.scp079 to see a list of all Command that SCP-079 could use (they can't use any SCP-079 Command)")]
    public bool ShowCommandListToOthers { get; set; } = true;
    
    public List<CommandConfiguration> CommandConfigurations { get; set; } = new List<CommandConfiguration>()
    {
        new()
        {
            Name = "Example",
            Cooldown = 30,
            EnergyUsage = 50,
            ExperienceGain = 5,
            RequiredLevel = 2
        }
    };

    [Serializable]
    public class CommandConfiguration
    {
        public string Name { get; set; }
        public int RequiredLevel { get; set; } 
        public float EnergyUsage { get; set; }
        public int ExperienceGain { get; set; }
        public float Cooldown { get; set; }
    }
}