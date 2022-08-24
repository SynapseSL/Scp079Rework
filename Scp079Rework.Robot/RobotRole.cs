﻿using System.Collections.Generic;
using Neuron.Core.Meta;
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

    public RobotRole(Scp079Robot plugin, HeavyZoneService heavyZone, CassieService cassie)
    {
        _plugin = plugin;
        _heavyZone = heavyZone;
        _cassie = cassie;
    }

    public RoleType SpawnRole { get; set; } = RoleType.ClassD;
    
    public override void SpawnPlayer(bool spawnLite)
    {
        if(spawnLite) return;

        Player.ChangeRoleLite(SpawnRole);
    }

    public override void DeSpawn(DespawnReason reason)
    {
        if (reason != DespawnReason.Death) return;

        if (_heavyZone.Is079Recontained)
        {
            _cassie.AnnounceScpDeath("079-2");
            return;
        }

        Player.RoleID = (int)RoleType.Scp079;
    }

    public override List<uint> GetFriendsID() => _plugin.Config.Ff ? new List<uint> { } : new List<uint> { (uint)Team.SCP };

    public override List<uint> GetEnemiesID() => new() { (uint)Team.RSC, (uint)Team.CDP, (uint)Team.MTF };
}