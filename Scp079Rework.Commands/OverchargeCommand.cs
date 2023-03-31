using System.Collections.Generic;
using System.Linq;
using MEC;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Ninject;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Map.Rooms;
using Synapse3.SynapseModule.Player;

namespace Scp079Rework.Commands;


[Automatic]
[Scp079Command(
    CommandName = "Overcharge",
    Aliases = new string[] {  },
    Description = "Overcharges the Generators and therefore deactivates them",
    Cooldown = 200f,
    EnergyUsage = 125,
    ExperienceGain = 30,
    RequiredLevel = 3
)]
public class OverchargeCommand : Scp079Command
{
    [Inject]
    public Scp079Commands Plugin { get; set; }
    
    [Inject]
    public MapService Map { get; set; }
    
    [Inject]
    public CassieService Cassie { get; set; }
    
    [Inject]
    public RoomService Room { get; set; }

    public bool OverchargeActive { get; private set; }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        if (OverchargeActive)
        {
            result.StatusCode = CommandStatusCode.Error;
            result.Response = Plugin.Translation.Get(context.Scp079).OverchargeStillActive;
            return;
        }

        if (!Map.SynapseGenerators.Any(x => x.Active && !x.Engaged))
        {
            result.StatusCode = CommandStatusCode.Error;
            result.Response = Plugin.Translation.Get(context.Scp079).OverchargeNoGenerator;
            return;
        }
        
        Timing.RunCoroutine(Overcharge(context.Scp079));

        result.Response = Plugin.Translation.Get(context.Scp079).Overcharge;
    }

    public IEnumerator<float> Overcharge(SynapsePlayer scp079)
    {
        OverchargeActive = true;
        
        Cassie.Announce(Plugin.Config.OverchargeCassie, CassieSettings.Noise,
            CassieSettings.DisplayText);


        yield return Timing.WaitForSeconds(Plugin.Config.OverchargeDelay);

        foreach (var room in Room.Rooms)
        {
            room.TurnOffLights(Plugin.Config.OverchargeTime);
        }
        
        foreach (var door in Map.SynapseDoors)
        {
            if(door.Locked) continue;
            if(door.DoorType == DoorType.Scp914Door) continue;
            door.Open = false;
        }
        
        foreach (var generator in Map.SynapseGenerators)
        {
            if (!generator.Active || generator.Engaged) continue;
                
            var pos = generator.Position + generator.GameObject.transform.forward * -0.3f;
            Map.Explode(pos, GrenadeType.Grenade, scp079);

            generator.Active = false;
        }

        yield return Timing.WaitForSeconds(Plugin.Config.OverchargeTime);
        OverchargeActive = false;
    }
}