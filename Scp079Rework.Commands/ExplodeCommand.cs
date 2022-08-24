using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Synapse3.SynapseModule.Item;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Explode",
    Aliases = new [] { "Grenade" },
    Description = "Explodes your Camera",
    Cooldown = 30f,
    EnergyUsage = 80,
    ExperienceGain = 20f,
    RequiredLevel = 4
)]
public class ExplodeCommand : Scp079Command
{
    private readonly Scp079Commands _plugin;

    public ExplodeCommand(Scp079Commands plugin)
    {
        _plugin = plugin;
    }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        if (context.Arguments.Length == 0) context.Arguments = new[] { "0" };
        if (!float.TryParse(context.Arguments[0], out var delay))
            delay = 0;
        
        var grenade = new SynapseItem(ItemType.GrenadeHE, context.Scp079.Position);
        grenade.Throwable.Fuse(context.Scp079);
        grenade.Throwable.FuseTime = !_plugin.Config.AllowChangingExplodeTime || delay <= 0.05 ? 0.05f : delay;

        result.Response = _plugin.Translation.Explode;
    }
}