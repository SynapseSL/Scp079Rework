using Synapse3.SynapseModule.Events;

namespace Scp079Rework.Robot;

public class EventHandler
{
    private readonly Scp079Robot _plugin;

    public EventHandler(PlayerEvents player, Scp079Robot plugin)
    {
        _plugin = plugin;

        player.Death.Subscribe(Death);
        player.FallingIntoAbyss.Subscribe(FallingIntoAbyss);
        player.DoorInteract.Subscribe(DoorInteract);
    }

    private void FallingIntoAbyss(FallingIntoAbyssEvent ev)
    {
        if (ev.Player.RoleType == RoleType.Scp079) ev.Allow = false;
    }

    private void DoorInteract(DoorInteractEvent ev)
    {
        if (ev.Player.RoleID == 79) ev.Allow = true;
    }

    private void Death(DeathEvent ev)
    {
        if(ev.Attacker == null) return;

        if (ev.Player.RoleID == 79)
            ev.Attacker.SendBroadcast(_plugin.Translation.Get(ev.Attacker).KilledRobot, 5);

        if (ev.Attacker.RoleID == 79)
            ev.Player.SendWindowMessage(_plugin.Translation.Get(ev.Player).KilledByRobot.Replace("\\n", "\n"));
    }
}