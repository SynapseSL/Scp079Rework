using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Ninject;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Item;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079CommandKeyBind(
    CommandName = "Flash",
    Aliases = new [] { "Flashing" },
    Description = "Flashes everyone around your Camera",
    Cooldown = 5f,
    EnergyUsage = 10,
    ExperienceGain = 1,
    RequiredLevel = 1,
    DefaultKey = KeyCode.Keypad1
)]
public class Flash : Scp079CommandKeyBind
{
    [Inject]
    public Scp079Commands Plugin { get; set; }
    
    [Inject]
    public MapService Map { get; set; }
    
    public EventHandler EventHandler { get; private set; }

    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        EventHandler ??= Synapse.Get<EventHandler>();
        
        if (context.Arguments.Length == 0) context.Arguments = new[] { "0" };
        if (!float.TryParse(context.Arguments[0], out var delay))
            delay = 0;
        
        var grenade = new SynapseItem(ItemType.GrenadeFlash, context.Scp079.Position);
        grenade.Throwable.Fuse(context.Scp079);
        grenade.Throwable.FuseTime = !Plugin.Config.AllowChangingFlashTime || delay <= 0.05 ? 0.05f : delay;
        
        if (Plugin.Config.AllowInstantExplode)
            EventHandler.GrenadesToExplode.Add(grenade);

        result.Response = Plugin.Translation.Explode;
    }

    public override bool ExecuteBind(SynapsePlayer scp079)
    {
        Map.Explode(scp079.MainScpController.Scp079.Camera.Position, GrenadeType.FlashBang, scp079);
        return true;
    }
}