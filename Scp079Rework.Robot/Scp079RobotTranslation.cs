using System;
using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;

namespace Scp079Rework.Robot;

[Automatic]
[Serializable]
public class Scp079RobotTranslation : Translations<Scp079RobotTranslation>
{
    public string KilledRobot { get; set; } = "You have killed one of SCP-079's Robots";

    public string KilledByRobot { get; set; } = "You were killed by an SCP-079 Robot\nPress Esc to Close";

    public string RobotList { get; set; } = "All of your currently existing vessels";

    public string BackInto079 { get; set; } = "You are now SCP-079 again";

    public string IntoRobot { get; set; } = "You are now inside a SCP-079-2";
}