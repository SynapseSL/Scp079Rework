using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Respawning;
using Respawning.NamingRules;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079CommandKeyBind(
    CommandName = "Mtf",
    Aliases = new [] { "MobileTaskForce", "ntf" },
    Description = "Starts an Mtf Announcement.",
    Cooldown = 90f,
    EnergyUsage = 30,
    ExperienceGain = 2,
    RequiredLevel = 3,
    DefaultKey = KeyCode.Keypad3
)]
public class MtfAnnounceCommand : Scp079CommandKeyBind
{
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        ExecuteBind(null);
        result.Response = "Mtf announcement was send";
        result.StatusCode = CommandStatusCode.Ok;
    }

    public override bool ExecuteBind(SynapsePlayer scp079)
    {
        var rule = UnitNamingRule.AllNamingRules[SpawnableTeamType.NineTailedFox] as NineTailedFoxNamingRule;
        rule?.PlayEntranceAnnouncement(
            NineTailedFoxNamingRule.PossibleCodes[Random.Range(0, NineTailedFoxNamingRule.PossibleCodes.Length)] +
            "-" +
            ((int)Random.Range(1f, 20f)).ToString("00"));
        return true;
    }
}