﻿using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;

namespace Scp079Rework.Commands;

[Automatic]
public class Scp079CommandTranslation : Translations<Scp079CommandTranslation>
{
    public string LockdownStillActive { get; set; } = "The previous Lockdown is still active.";

    public string Lockdown { get; set; } = "Lockdown started";

    public string DeathNoNumber { get; set; } = "You have to specify a SCP Number that should be announced dead";

    public string DeathToLong { get; set; } = "Your Scp is to long. A simple .079 death 173 is enough";

    public string Death { get; set; } = "Death announcement was send";

    public string Explode { get; set; } = "Exploded successfully";

    public string Random { get; set; } = "You are now inside a random Camera";

    public string Scan { get; set; } = "All living beings your cameras could find:";

    public string NoScp { get; set; } = "No Scp was found near a Camera";

    public string Scp { get; set; } = "Your now near another SCP";

    public string OverchargeStillActive { get; set; } = "The Overcharge is still in Progress";

    public string OverchargeNoGenerator { get; set; } = "You can't use overcharge sine no Generator is active";

    public string Overcharge { get; set; } = "Overcharge started";
}