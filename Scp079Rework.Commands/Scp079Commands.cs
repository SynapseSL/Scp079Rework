using Neuron.Core.Plugins;
using Synapse3.SynapseModule;

namespace Scp079Rework.Commands;

[Plugin(
    Name = "SCP-079 Commands",
    Description = "Adds Command for SCP-079 with the SCP-079 Rework Module",
    Author = "Dimenzio",
    Version = "1.0.0"
)]
public class Scp079Commands : ReloadablePlugin
{
    public override void Load()
    {
        Logger.Info("Enabled SCP-079 Commands");
    }
}
