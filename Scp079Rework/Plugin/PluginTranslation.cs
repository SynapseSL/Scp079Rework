using Synapse.Translation;

namespace Scp079Rework
{
    public class PluginTranslation : IPluginTranslation
    {
        public string Not079 { get; set; } = "You are not SCP-079";

        public string Help { get; set; } = "All Scp-079 commands that you can execute:";

        public string Cooldown { get; set; } = "You have to wait %seconds% more seoncds until you can execute this command again";

        public string LowLevel { get; set; } = "You`re level are to low! You need at least level %level%";

        public string LowEnergy { get; set; } = "You need at least %energy% Energy for executing this Command";

        public string Error { get; set; } = "Error while Executing this Command";

        public string NoCommand { get; set; } = "No Scp-079 Command was found for %command%";

        public string NotAsRobot { get; set; } = "As SCP-079-robot you can't use this Command";
    }
}
