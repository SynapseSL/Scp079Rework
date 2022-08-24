using System.Collections.Generic;
using System.Text;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Respawning;
using Respawning.NamingRules;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Player;
using UnityEngine;
using Utils.Networking;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Mtf",
    Aliases = new [] { "MobileTaskForce", "ntf" },
    Description = "Starts an Mtf Announcement.",
    Cooldown = 90f,
    EnergyUsage = 30,
    ExperienceGain = 20f,
    RequiredLevel = 3
)]
public class MtfAnnounceCommand : Scp079Command
{
    private readonly PlayerService _player;
    private readonly Scp079Commands _plugin;

    public MtfAnnounceCommand(PlayerService player, Scp079Commands plugin)
    {
        _player = player;
        _plugin = plugin;
    }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        if (context.Arguments.Length == 0)
            context.Arguments = new[]
            {
                _player.GetPlayers(x => x.TeamID == (uint)Team.SCP && x.RoleType != RoleType.Scp0492, PlayerType.Player)
                    .Count.ToString()
            };

        if (!int.TryParse(context.Arguments[0], out var number) || number < 0)
        {
            result.Response = _plugin.Translation.Get(context.Scp079).InvalidNumber;
            result.StatusCode = CommandStatusCode.Forbidden;
            return;
        }
        
        if (UnitNamingRules.TryGetNamingRule(SpawnableTeamType.NineTailedFox, out var namingRule))
        {
            var unit =
                NineTailedFoxNamingRule.PossibleCodes[Random.Range(0, NineTailedFoxNamingRule.PossibleCodes.Length)] +
                "-" + Random.Range(1, 20).ToString("00");

            PlayAnnouncement(number, unit, namingRule);
            result.Response = _plugin.Translation.Get(context.Scp079).MtfAnnouncement;
            return;
        }

        result.Response = "Couldn't find MTF Announcer";
        result.StatusCode = CommandStatusCode.Error;
    }

    private void PlayAnnouncement(int number, string unit, UnitNamingRule namingRule)
    {
        string cassieUnitName = namingRule.GetCassieUnitName(unit);

        var msg = "";
        if (ClutterSpawner.IsHolidayActive(Holidays.Christmas))
        {
            msg += "XMAS_EPSILON11 ";
            msg += cassieUnitName;
            msg += "XMAS_HASENTERED ";
            msg += number;
            msg += " XMAS_SCPSUBJECTS";
        }
        else
        {
            msg += "MTFUNIT EPSILON 11 DESIGNATED ";
            msg += cassieUnitName;
            msg += " HASENTERED ALLREMAINING ";
            if (number == 0)
            {
                msg += "NOSCPSLEFT";
            }
            else
            {
                msg += "AWAITINGRECONTAINMENT ";
                msg += number;
                if (number == 1)
                {
                    msg += " SCPSUBJECT";
                }
                else
                {
                    msg += " SCPSUBJECTS";
                }
            }
        }

        var list = new List<Subtitles.SubtitlePart>
        {
            new(Subtitles.SubtitleType.NTFEntrance, new[]
            {
                unit
            })
        };

        switch (number)
        {
            case 0:
                list.Add(new Subtitles.SubtitlePart(Subtitles.SubtitleType.ThreatRemains, null));
                break;

            case 1:
                list.Add(new Subtitles.SubtitlePart(Subtitles.SubtitleType.AwaitContainSingle, null));
                break;

            default:
                list.Add(new Subtitles.SubtitlePart(Subtitles.SubtitleType.AwaitContainPlural, new string[]
                {
                    number.ToString()
                }));
                break;
        }

        new Subtitles.SubtitleMessage(list.ToArray()).SendToAuthenticated();
        var builder = new StringBuilder(msg);
        namingRule.ConfirmAnnouncement(ref builder);
    }
}
