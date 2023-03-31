using System.Globalization;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using PlayerRoles;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Map;

namespace Scp079Rework.Robot;

[Automatic]
[Scp079Command(
    CommandName = "Robot",
    Aliases = new string[] {  },
    Description = "Allows you to transfer your mind into an Robot",
    Cooldown = 0f,
    EnergyUsage = 0,
    ExperienceGain = 0,
    RequiredLevel = 3
)]
public class RobotCommand : Scp079Command
{
    private readonly Scp079Robot _plugin;
    private readonly HeavyZoneService _heavyZone;
    private readonly NukeService _nuke;
    public static RobotCommand Command { get; private set; }

    public RobotCommand(Scp079Robot plugin, HeavyZoneService heavyZone, NukeService nuke)
    {
        _plugin = plugin;
        _heavyZone = heavyZone;
        _nuke = nuke;
        Command = this;
    }

    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        var script = context.Scp079.GetComponent<Scp079Script>();

        if (context.Arguments.Length != 0 && uint.TryParse(context.Arguments[0], out var index))
        {
            if (index == script.Robots.Count && !_heavyZone.Is079Contained && _nuke.State != NukeState.Detonated)
            {
                script.LeaveRobot();
                result.Response = _plugin.Translation.Get(context.Scp079).BackInto079;
                return;
            }

            if (index < script.Robots.Count)
            {
                script.TakeRobot(script.Robots[(int)index]);
                result.Response = _plugin.Translation.Get(context.Scp079).IntoRobot;
                return;
            }
        }

        var msg = "\n" + _plugin.Translation.Get(context.Scp079).RobotList;
        for (int i = 0; i < script.Robots.Count; i++)
        {
            msg += $"\n{i} - {script.Robots[i].RobotName}";
        }

        if (context.Scp079.RoleID == 79 && !_heavyZone.Is079Contained && _nuke.State != NukeState.Detonated)
            msg += $"\n{script.Robots.Count} - Scp079";

        result.Response = msg;
    }

    public override CommandResult PreExecute(Scp079Context context)
    {
        if (context.Scp079.RoleType != RoleTypeId.Scp079 && context.Scp079.RoleID != 79)
        {
            return new CommandResult()
            {
                StatusCode = CommandStatusCode.Forbidden,
                Response = Synapse.Get<Scp079ReworkTranslation>().Get(context.Scp079).Not079,
            };
        }

        var module = Synapse.Get<Scp079Rework>();
        var level = GetRequiredLevel(module);
        if (level > context.Scp079.MainScpController.Scp079.Level && !context.Scp079.Bypass)
        {
            return new CommandResult()
            {
                StatusCode = CommandStatusCode.Forbidden,
                Response = module.Translation.Get(context.Scp079).LevelToLow.Replace("%level%", level.ToString(CultureInfo.InvariantCulture))
            };
        }

        return null;
    }

    public override void AfterExecution(Scp079Context context, ref CommandResult result) { }
}