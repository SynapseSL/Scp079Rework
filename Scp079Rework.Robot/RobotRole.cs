using System.Collections.Generic;
using Neuron.Core.Logging;
using Neuron.Core.Meta;
using PlayerRoles;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Enums;
using Synapse3.SynapseModule.Map;
using Synapse3.SynapseModule.Role;

namespace Scp079Rework.Robot;

[Automatic]
[Role(
    Name = "SCP-079 Robot",
    Id = 79,
    TeamId = (uint)Team.SCPs
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

    public override void SpawnPlayer(ISynapseRole previousRole = null, bool spawnLite = false)
    {
        if(spawnLite) return;

        //So normally a SCP-079 Robot is always spawned lite but for the case someone is set manually is here a backup
        Player.RoleType = RoleTypeId.FacilityGuard;
    }

    public override void DeSpawn(DeSpawnReason reason)
    {
        if (reason != DeSpawnReason.Death) return;

        if (_heavyZone.Is079Contained || _nuke.State == NukeState.Detonated)
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

        Player.RoleID = (uint)RoleTypeId.Scp079;
    }

    public override List<uint> GetFriendsID() => _plugin.Config.Ff ? new List<uint> { } : new List<uint> { (uint)Team.SCPs };

    public override List<uint> GetEnemiesID() => new() { (uint)Team.Scientists, (uint)Team.ClassD, (uint)Team.FoundationForces };
}