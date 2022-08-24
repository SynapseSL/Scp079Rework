using System;
using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;

namespace Scp079Rework;

[Automatic]
[Serializable]
public class Scp079ReworkTranslation : Translations<Scp079ReworkTranslation>
{
    public string Not079 { get; set; } = "You are not SCP-079 therefore can you not use this Command";

    public string NoSubCommand { get; set; } = "This Sub Command wasn't found. Use .079 or .scp079 to see a list of all Commands";

    public string CooldownActive { get; set; } = "You still have to was %time% seconds before you can use this Command again";

    public string LevelToLow { get; set; } = "You can't use this Command currently. You need to be level %level% at least";

    public string EnergyToLow { get; set; } = "You haven't enough Energy. You need at lest %energy%";

    public string CommandList { get; set; } = "This are all Commands that SCP-079 can use:";
}