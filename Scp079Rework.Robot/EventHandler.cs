using MEC;
using Neuron.Core.Events;
using Neuron.Core.Meta;
using Ninject;
using PlayerRoles;
using Synapse3.SynapseModule.Dummy;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Player;

namespace Scp079Rework.Robot;

[Automatic]
public class EventHandler : Listener
{
    [Inject]
    public Scp079Robot Plugin { get; set; }
    
    [Inject]
    public Scp079Rework Module { get; set; }
    
    [Inject]
    public PlayerService Player { get; set; }

    [EventHandler]
    public void Contain(Scp079ContainEvent ev)
    {
        if(ev.Status != Scp079ContainmentStatus.Overcharge) return;
        var level = RobotCommand.Command.GetRequiredLevel(Module);
        foreach (var player in Player.Players)
        {
            if (player.RoleID != (uint)RoleTypeId.Scp079) continue;
            if (level > player.MainScpController.Scp079.Level && !player.Bypass) continue;
            var script = player.GetComponent<Scp079Script>();
            if (script.Robots.Count < 1) continue;
            script.TakeRobot(script.Robots[0]);
            player.SendHint(Plugin.Translation.Get(player).ForcedIntoRobot,15);
            player.GodMode = true;
            Timing.CallDelayed(0.2f, () => player.GodMode = false);
        }
    }
    
    [EventHandler]
    public void Detonate(DetonateWarheadEvent _)
    {
        foreach (var player in Player.Players)
        {
            foreach (var robot in player.GetComponent<Scp079Script>().Robots)
            {
                if(robot.Position.y < 900f)
                    robot.Destroy();
            }
        }
    }

    [EventHandler]
    public void SetClass(SetClassEvent ev)
    {
        if (ev.Role == RoleTypeId.Scp079)
        {
            ev.Player.GetComponent<Scp079Script>().SpawnRobots();
        }
    }

    [EventHandler]
    public void LoadComponent(LoadComponentEvent ev) => ev.AddComponent<Scp079Script>();

    [EventHandler]
    public void HarmPermission(HarmPermissionEvent ev)
    {
        if (ev.Victim.PlayerType != PlayerType.Dummy) return;
        if (ev.Victim is not DummyPlayer { SynapseDummy: Robot robot }) return;

        ev.Allow = Synapse3Extensions.GetHarmPermission(ev.Attacker, robot.Owner);
    }

    [EventHandler]
    public void FallingIntoAbyss(FallingIntoAbyssEvent ev)
    {
        if (ev.Player.RoleType == RoleTypeId.Scp079) ev.Allow = false;
    }

    [EventHandler]
    public void DoorInteract(DoorInteractEvent ev)
    {
        if (ev.Player.RoleID == 79) ev.Allow = true;
    }

    [EventHandler]
    public void Death(DeathEvent ev)
    {
        if(ev.Attacker == null || ev.Player.PlayerType != PlayerType.Player) return;

        if (ev.Player.RoleID == 79)
            ev.Attacker.SendBroadcast(Plugin.Translation.Get(ev.Attacker).KilledRobot, 5);

        if (ev.Attacker.RoleID == 79)
            ev.Player.SendWindowMessage(Plugin.Translation.Get(ev.Player).KilledByRobot.Replace("\\n", "\n"));
    }
}