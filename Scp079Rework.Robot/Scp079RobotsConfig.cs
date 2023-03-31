using System;
using System.Collections.Generic;
using Neuron.Core.Meta;
using PlayerRoles;
using Syml;
using Synapse3.SynapseModule.Config;
using Synapse3.SynapseModule.Map.Rooms;
using UnityEngine;

namespace Scp079Rework.Robot;

[Automatic]
[Serializable]
[DocumentSection("SCP 079 Robot")]
public class Scp079RobotsConfig : IDocumentSection
{
    public bool Ff { get; set; } = false;

    public List<RobotConfiguration> Robots { get; set; } = new()
    {
        new RobotConfiguration()
        {
            SpawnLocation = new RoomPoint()
            {
                roomName = "Scp079",
                position = new SerializedVector3(-1f, -5.87f, -5.95f),
                rotation = new SerializedVector3(0f, 180f, 0f)
            },
            Inventory = new SerializedPlayerInventory()
            {
                Items = new List<SerializedPlayerItem>()
                {
                    new SerializedPlayerItem((uint)ItemType.GunCOM15,15,0u,Vector3.one, 100,true)
                },
                Ammo = new SerializedAmmo()
                {
                    Ammo9 = 30
                }
            },
            RoleType = RoleTypeId.FacilityGuard,
            Name = "Facility Guard Unit"
        },
        new RobotConfiguration()
        {
            SpawnLocation = new RoomPoint()
            {
                roomName = "Scp079",
                position = new SerializedVector3(-2f, -5.87f, -5.95f),
                rotation = new SerializedVector3(0f, 180f, 0f)
            },
            Inventory = new SerializedPlayerInventory()
            {
                Items = new List<SerializedPlayerItem>()
                {
                    new SerializedPlayerItem((uint)ItemType.GunCOM15,15,0u,Vector3.one, 100,true)
                },
                Ammo = new SerializedAmmo()
                {
                    Ammo9 = 30
                }
            },
            RoleType = RoleTypeId.ClassD,
            Name = "Class D Unit"
        },
        new RobotConfiguration()
        {
            SpawnLocation = new RoomPoint()
            {
                roomName = "Scp079",
                position = new SerializedVector3(-3f, -5.87f, -5.95f),
                rotation = new SerializedVector3(0f, 180f, 0f)
            },
            Inventory = new SerializedPlayerInventory()
            {
                Items = new List<SerializedPlayerItem>()
                {
                    new SerializedPlayerItem((uint)ItemType.GunCOM15,15,0u,Vector3.one, 100,true)
                },
                Ammo = new SerializedAmmo()
                {
                    Ammo9 = 30
                }
            },
            RoleType = RoleTypeId.Scientist,
            Name = "Scientist Unit"
        }
    };

    [Serializable]
    public class RobotConfiguration
    {
        public RoomPoint SpawnLocation { get; set; } = new();

        public SerializedPlayerInventory Inventory { get; set; } = new();

        public RoleTypeId RoleType { get; set; } = RoleTypeId.ClassD;

        public int Health { get; set; } = 50;

        public SerializedVector3 Scale { get; set; } = Vector3.one;

        public string Name { get; set; } = "Facility Guard Unit";
    }
}