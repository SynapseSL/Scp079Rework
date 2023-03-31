using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Ninject;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Map.Objects;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework.Commands;

[Automatic]
[Scp079CommandKeyBind(
    CommandName = "Lockdown",
    Aliases = new string[] {  },
    Description = "Locks the entire Facility",
    Cooldown = 120f,
    EnergyUsage = 110,
    ExperienceGain = 5,
    RequiredLevel = 4,
    DefaultKey = KeyCode.Keypad6
)]
public class LockdownCommand : Scp079CommandKeyBind
{
    [Inject]
    public Scp079Commands Plugin { get; set; }
    
    [Inject]
    public MapService Map { get; set; }
    
    [Inject]
    public CassieService Cassie { get; set; }

    public bool OnLockDown { get; private set; }
    
    public override void ExecuteCommand(Scp079Context context, ref CommandResult result)
    {
        if (OnLockDown)
        {
            result.StatusCode = CommandStatusCode.Error;
            result.Response = Plugin.Translation.Get(context.Scp079).LockdownStillActive;
            return;
        }

        Timing.RunCoroutine(LockDown());
        result.Response = Plugin.Translation.Get(context.Scp079).Lockdown;
    }

    public IEnumerator<float> LockDown()
    {
        OnLockDown = true;
        var lockedDoors = new List<SynapseDoor>();

        Cassie.Announce(Plugin.Config.LockdownCassie, CassieSettings.Noise, CassieSettings.Glitched,
            CassieSettings.DisplayText);
        
        foreach (var door in Map.SynapseDoors)
        {
            if(door.Locked) continue;
            door.LockWithReason(DoorLockReason.Lockdown079);
            lockedDoors.Add(door);
        }
        
        yield return Timing.WaitForSeconds(Plugin.Config.LockdownDuration);

        foreach (var door in lockedDoors)
        {
            door.LockWithReason(DoorLockReason.Lockdown079, false);
        }

        OnLockDown = false;
    }

    public override bool ExecuteBind(SynapsePlayer scp079)
    {
        if (OnLockDown) return false;
        Timing.RunCoroutine(LockDown());
        return true;
    }
}