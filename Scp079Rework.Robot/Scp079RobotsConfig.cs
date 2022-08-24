using System;
using System.Collections.Generic;
using Neuron.Core.Meta;
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
    
    public Dictionary<int, RobotConfiguration> Robots { get; set; } = new Dictionary<int, RobotConfiguration>()
    {

    };

    public class RobotConfiguration
    {
        public int Amount { get; set; } = 1;
        
        public List<RoomPoint> RobotSpawns { get; set; } = new List<RoomPoint>()
        {
            new("", new Vector3(9.3f, -2.4f, 0f), Vector3.zero)
        };

        public SerializedPlayerInventory Inventory { get; set; } = new SerializedPlayerInventory()
        {

        };

        public int Health { get; set; } = 50;
    }
}