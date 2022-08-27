using System.Collections.Generic;
using Neuron.Core.Logging;
using Neuron.Core.Meta;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Role;

namespace Scp079Rework.Robot;

[Automatic]
[Role(
    Name = "SCP-079 Robot",
    Id = 79,
    TeamId = (uint)Team.SCP
)]
public class RobotRole : SynapseRole
{
    private readonly Scp079Robot _plugin;
    private readonly HeavyZoneService _heavyZone;
    private readonly CassieService _cassie;
    private readonly NukeService _nuke;

    public RobotRole(Scp079Robot plugin, HeavyZoneService heavyZone, CassieService cassie, NukeService nuke)
    {
        _plugin = plugin;
        _heavyZone = heavyZone;
        _cassie = cassie;
        _nuke = nuke;
    }

    public override void SpawnPlayer(bool spawnLite)
    {
        if(spawnLite) return;

        //So normally a SCP-079 Robot is always spawned lite but for the case someone is set manually is here a backup
        Player.RoleType = RoleType.FacilityGuard;
    }

    public override void DeSpawn(DespawnReason reason)
    {
        if (reason != DespawnReason.Death) return;

        if (_heavyZone.Is079Recontained || _nuke.State == NukeState.Detonated)
        {
            var script = Player.GetComponent<Scp079Script>();
            if (script.Robots.Count > 0)
            {
                script.TakeRobot(script.Robots[0]);
                return;
            }
            
            _cassie.AnnounceScpDeath("079-2");
            return;
        }
        
        NeuronLogger.For<Synapse>().Warn("SET TO 079");

        Player.RoleID = (int)RoleType.Scp079;
    }

    public override List<uint> GetFriendsID() => _plugin.Config.Ff ? new List<uint> { } : new List<uint> { (uint)Team.SCP };

    public override List<uint> GetEnemiesID() => new() { (uint)Team.RSC, (uint)Team.CDP, (uint)Team.MTF };
}