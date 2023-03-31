using Neuron.Core.Plugins;
using Synapse3.SynapseModule;

namespace Scp079Rework.Commands;

[Plugin(
    Name = "SCP-079 Commands",
    Description = "Adds Command for SCP-079 with the SCP-079 Rework Module",
    Author = "Dimenzio",
    Version = "1.0.0",
    Repository = "https://github.com/SynapseSL/Scp079Rework"
)]
public class Scp079Commands : ReloadablePlugin<Scp079CommandConfigs, Scp079CommandTranslation>
{
    public override void EnablePlugin()
    {
        Logger.Info("Enabled SCP-079 Commands");
    }
}
