using System;
using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Neuron.Core.Logging;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Item;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Map.Objects;
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
    ExperienceGain = 30f,
    RequiredLevel = 3
)]
public class OverchargeCommand : Scp079Command
{
    private readonly Scp079Commands _plugin;
    private readonly MapService _map;
    private readonly CassieService _cassie;
    private readonly RoomService _room;

    public OverchargeCommand(Scp079Commands plugin, MapService map, CassieService cassie, RoomService room)
    {
        _plugin = plugin;
        _map = map;
        _cassie = cassie;
        _room = room;
    }
    
    public bool OverchargeActive { get; private set; }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        if (OverchargeActive)
        {
            result.StatusCode = CommandStatusCode.Error;
            result.Response = _plugin.Translation.Get(context.Scp079).OverchargeStillActive;
            return;
        }

        if (!_map.SynapseGenerators.Any(x => x.Active && !x.Engaged))
        {
            result.StatusCode = CommandStatusCode.Error;
            result.Response = _plugin.Translation.Get(context.Scp079).OverchargeNoGenerator;
            return;
        }
        
        Timing.RunCoroutine(Overcharge(context.Scp079));

        result.Response = _plugin.Translation.Get(context.Scp079).Overcharge;
    }

    public IEnumerator<float> Overcharge(SynapsePlayer scp079)
    {
        OverchargeActive = true;
        
        _cassie.Announce(_plugin.Config.OverchargeCassie, CassieSettings.Noise,
            CassieSettings.DisplayText);


        yield return Timing.WaitForSeconds(_plugin.Config.OverchargeDelay);

        foreach (var room in _room.Rooms)
        {
            room.TurnOffLights(_plugin.Config.OverchargeTime);
        }
        
        foreach (var door in _map.SynapseDoors)
        {
            if(door.Locked) continue;
            if(door.DoorType == DoorType.Scp914Door) continue;
            door.Open = false;
        }
        
        foreach (var generator in _map.SynapseGenerators)
        {
            if (!generator.Active || generator.Engaged) continue;
                
            var pos = generator.Position + generator.GameObject.transform.forward * -0.3f;
            _map.Explode(pos, GrenadeType.Grenade, scp079);

            generator.Active = false;
        }

        yield return Timing.WaitForSeconds(_plugin.Config.OverchargeTime);
        OverchargeActive = false;
    }
}