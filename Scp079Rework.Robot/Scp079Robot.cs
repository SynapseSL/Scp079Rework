using Neuron.Core.Plugins;
using Synapse3.SynapseModule;

namespace Scp079Rework.Robot;

[Plugin(
    Name = "SCP-079 Robot",
    Description = "Adds the Robot Command to SCP-079",
    Author = "Dimenzio",
    Version = "1.0.0",
    Repository = "https://github.com/SynapseSL/Scp079Rework"
)]
public class Scp079Robot : ReloadablePlugin<Scp079RobotsConfig, Scp079RobotTranslation>
{
    public override void EnablePlugin()
    {
        Logger.Info("Enabled SCP-079 Robot");
    }
}
