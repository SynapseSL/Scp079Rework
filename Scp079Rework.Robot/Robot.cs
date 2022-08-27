using Neuron.Core.Logging;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Config;
using Synapse3.SynapseModule.Dummy;
using Synapse3.SynapseModule.Item;
using Synapse3.SynapseModule.Map.Rooms;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework.Robot;

public class Robot : SynapseDummy
{
    public SerializedPlayerState PlayerState { get; }
    
    public string RobotName { get; }
    
    public SynapsePlayer Owner { get; }

    public void ReplaceWithPlayer(SynapsePlayer player)
    {
        PlayerState.Health = Player.Health;
        PlayerState.ArtificialHealth = Player.ArtificialHealth;
        PlayerState.Apply(player);
        Destroy();
    }

    public Robot(SynapsePlayer player, Scp079RobotsConfig.RobotConfiguration configuration) : base(
        configuration.SpawnLocation.GetMapPosition(),
        new Vector2(configuration.SpawnLocation.GetMapRotation().eulerAngles.x,
            configuration.SpawnLocation.GetMapRotation().eulerAngles.y), configuration.RoleType,
         string.IsNullOrWhiteSpace(player.DisplayName) ? player.NickName : player.DisplayName,
        player.HideRank ? "" : player.RankName, player.HideRank ? "" : player.RankColor)
    {
        Owner = player;
        Player.Scale = configuration.Scale;
        RobotName = configuration.Name;
        Player.Health = configuration.Health;

        var rot = configuration.SpawnLocation.GetMapRotation();
        PlayerState = new SerializedPlayerState()
        {
            Health = configuration.Health,
            Inventory = configuration.Inventory,
            Position = configuration.SpawnLocation.GetMapPosition(),
            Rotation = new SerializedVector2(rot.x, rot.y),
            Scale = configuration.Scale,
            MaxHealth = configuration.Health,
            RoleType = Player.RoleType,
            RoleID = 79
        };
    }

    public Robot(SynapsePlayer player, string robotName) : base(player.Position, player.RotationVector2, player.RoleType,
        string.IsNullOrWhiteSpace(player.DisplayName) ? player.NickName : player.DisplayName, player.RankName, player.RankColor)
    {
        Owner = player;
        RobotName = robotName;
        Player.GodMode = false;
        Player.Health = player.Health;
        Player.ArtificialHealth = player.ArtificialHealth;
        Player.Scale = player.Scale;
        HeldItem = player.Inventory.ItemInHand == SynapseItem.None
            ? ItemType.None
            : player.Inventory.ItemInHand.ItemType;
        PlayerState = player;
    }

    public override void OnDestroy()
    {
        if (Owner != null)
        {
            Owner.GetComponent<Scp079Script>().Robots?.Remove(this);
        }
        base.OnDestroy();
    }
}