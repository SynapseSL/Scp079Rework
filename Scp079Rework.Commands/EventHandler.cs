using System.Collections.Generic;
using System.Linq;
using Neuron.Core.Events;
using Neuron.Core.Meta;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Item;
using UnityEngine;

namespace Scp079Rework.Commands;

[Automatic]
public class EventHandler : Listener
{
    public List<SynapseItem> GrenadesToExplode = new();

    [EventHandler]
    public void OnWaiting(RoundWaitingEvent _) => GrenadesToExplode.Clear();
    
    [EventHandler]
    public void OnPing(Scp079PingEvent ev)
    {
        if (ev.PingType != Scp079PingType.Projectile) return;
        foreach (var grenade in GrenadesToExplode.ToList())
        {
            if (grenade.State != ItemState.Thrown) GrenadesToExplode.Remove(grenade);
            if(Vector3.Distance(grenade.Position,ev.Position) > 3f) return;
            grenade.Throwable.FuseTime = 0.05f;
            GrenadesToExplode.Remove(grenade);
        }
    }
}