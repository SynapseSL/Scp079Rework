using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Synapse3.SynapseModule.Item;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Flash",
    Aliases = new [] { "Flashing" },
    Description = "Flashes everyone around your Camera",
    Cooldown = 5f,
    EnergyUsage = 10,
    ExperienceGain = 1f,
    RequiredLevel = 1
)]
public class FlashCommand : Scp079Command
{
    private readonly Scp079Commands _plugin;

    public FlashCommand(Scp079Commands plugin)
    {
        _plugin = plugin;
    }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        if (context.Arguments.Length == 0) context.Arguments = new[] { "0" };
        if (!float.TryParse(context.Arguments[0], out var delay))
            delay = 0;
        
        var grenade = new SynapseItem(ItemType.GrenadeFlash, context.Scp079.Position);
        grenade.Throwable.Fuse(context.Scp079);
        grenade.Throwable.FuseTime = !_plugin.Config.AllowChangingFlashTime || delay <= 0.05 ? 0.05f : delay;

        result.Response = _plugin.Translation.Explode;
    }
}