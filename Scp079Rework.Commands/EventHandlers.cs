using Synapse3.SynapseModule.Events;
using UnityEngine;

namespace Scp079Rework.Commands;

public class EventHandlers
{
    public EventHandlers(PlayerEvents playerEvents)
    {
        playerEvents.KeyPress.Subscribe(KeyPress);
    }

    private void KeyPress(KeyPressEvent ev)
    {
        if(ev.Player.RoleType != RoleType.Scp079) return;
        
        switch (ev.KeyCode)
        {
            case KeyCode.Alpha1:
                ev.Player.ExecuteCommand("079 flash",false);
                break;
            
            case KeyCode.Alpha2:
                ev.Player.ExecuteCommand("079 explode",false);
                break;
        }
    }
}