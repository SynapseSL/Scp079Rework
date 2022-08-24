using System.Globalization;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Synapse3.SynapseModule;

namespace Scp079Rework.Robot;

[Automatic]
[Scp079Command(
    CommandName = "Robot",
    Aliases = new string[] {  },
    Description = "Allows you to transfer your mind into an Robot",
    Cooldown = 0f,
    EnergyUsage = 0,
    ExperienceGain = 0f,
    RequiredLevel = 3
)]
public class RobotCommand : Scp079Command
{
    private readonly Scp079Robot _plugin;

    public RobotCommand(Scp079Robot plugin)
    {
        _plugin = plugin;
    }

    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        result.Response = "Robot Command!";
    }

    public override CommandResult PreExecute(Scp079Context context)
    {
        if (context.Scp079.RoleType != RoleType.Scp079 && context.Scp079.RoleID != 79)
        {
            return new CommandResult()
            {
                StatusCode = CommandStatusCode.Forbidden,
                Response = Synapse.Get<Scp079ReworkTranslation>().Get(context.Scp079).Not079,
            };
        }
        
        var module = Synapse.Get<Scp079Rework>();
        var level = GetRequiredLevel(module);
        if (context.Scp079.RoleID != 79 && level > context.Scp079.ScpController.Scp079.Level &&
            !context.Scp079.Bypass)
        {
            return new CommandResult()
            {
                StatusCode = CommandStatusCode.Forbidden,
                Response = module.Translation.Get(context.Scp079).LevelToLow
                    .Replace("%level%", level.ToString(CultureInfo.InvariantCulture))
            };
        }

        return null;
    }
    public override void AfterExecution(Scp079Context context, ref CommandResult result) { }
}