using Neuron.Core.Logging;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Dummy;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework.Robot;

public class EventHandler
{
    private readonly Scp079Robot _plugin;
    private readonly PlayerService _player;
    private readonly NukeService _nuke;

    public EventHandler(PlayerEvents player, MapEvents map, Scp079Robot plugin, PlayerService playerService,
        NukeService nuke)
    {
        _plugin = plugin;
        _player = playerService;
        _nuke = nuke;

        player.Death.Subscribe(Death);
        player.FallingIntoAbyss.Subscribe(FallingIntoAbyss);
        player.DoorInteract.Subscribe(DoorInteract);
        player.HarmPermission.Subscribe(HarmPermission);
        player.LoadComponent.Subscribe(LoadComponent);
        player.SetClass.Subscribe(SetClass);
        map.DetonateWarhead.Subscribe(Detonate);
    }

    private void Detonate(DetonateWarheadEvent _)
    {
        foreach (var player in _player.Players)
        {
            foreach (var robot in player.GetComponent<Scp079Script>().Robots)
            {
                if(robot.Position.y < 900f)
                    robot.Destroy();
            }
        }
    }

    private void SetClass(SetClassEvent ev)
    {
        if (ev.Role == RoleType.Scp079)
        {
            ev.Player.GetComponent<Scp079Script>().SpawnRobots();
        }
    }

    private void LoadComponent(LoadComponentEvent ev) => ev.AddComponent<Scp079Script>();

    private void HarmPermission(HarmPermissionEvent ev)
    {
        if (ev.Victim.PlayerType != PlayerType.Dummy) return;
        if (ev.Victim is not DummyPlayer { SynapseDummy: Robot robot }) return;

        ev.Allow = Synapse3Extensions.GetHarmPermission(ev.Attacker, robot.Owner);
        NeuronLogger.For<Synapse>().Warn("Harm Permission on Robot: " + ev.Allow);
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
        if(ev.Attacker == null || ev.Player.PlayerType != PlayerType.Player) return;

        if (ev.Player.RoleID == 79)
            ev.Attacker.SendBroadcast(_plugin.Translation.Get(ev.Attacker).KilledRobot, 5);

        if (ev.Attacker.RoleID == 79)
            ev.Player.SendWindowMessage(_plugin.Translation.Get(ev.Player).KilledByRobot.Replace("\\n", "\n"));
    }
}