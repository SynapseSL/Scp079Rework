using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Map.Objects;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079Command(
    CommandName = "Lockdown",
    Aliases = new string[] {  },
    Description = "Locks the entire Facility",
    Cooldown = 120f,
    EnergyUsage = 110,
    ExperienceGain = 30f,
    RequiredLevel = 4
)]
public class LockdownCommand : Scp079Command
{
    private readonly Scp079Commands _plugin;
    private readonly MapService _map;
    private readonly CassieService _cassie;

    public LockdownCommand(Scp079Commands plugin, MapService map, CassieService cassie)
    {
        _plugin = plugin;
        _map = map;
        _cassie = cassie;
    }
    
    public bool OnLockDown { get; private set; }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        if (OnLockDown)
        {
            result.StatusCode = CommandStatusCode.Error;
            result.Response = _plugin.Translation.Get(context.Scp079).LockdownStillActive;
            return;
        }

        Timing.RunCoroutine(LockDown());

        result.Response = _plugin.Translation.Get(context.Scp079).Lockdown;
    }

    public IEnumerator<float> LockDown()
    {
        OnLockDown = true;
        var lockedDoors = new List<SynapseDoor>();

        _cassie.Announce(_plugin.Config.LockdownCassie, CassieSettings.Noise, CassieSettings.Glitched,
            CassieSettings.DisplayText);
        
        foreach (var door in _map.SynapseDoors)
        {
            if(door.Locked) continue;
            door.LockWithReason(DoorLockReason.Lockdown079);
            lockedDoors.Add(door);
        }
        
        yield return Timing.WaitForSeconds(_plugin.Config.LockdownDuration);

        foreach (var door in lockedDoors)
        {
            door.LockWithReason(DoorLockReason.Lockdown079, false);
        }

        OnLockDown = false;
    }
}