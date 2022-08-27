using System.Collections.Generic;
using System.Linq;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Player;
using UnityEngine;

namespace Scp079Rework.Robot;

public class Scp079Script : MonoBehaviour
{
    public Scp079Robot Plugin { get; private set; }
    
    public SynapsePlayer Player { get; private set; }

    public List<Robot> Robots { get; } = new ();

    public bool BotsSpawned { get; set; }

    private string _currentTakenRobot = "Default";

    public void SpawnRobots()
    {
        if (BotsSpawned) return;
        BotsSpawned = true;

        foreach (var configuration in Plugin.Config.Robots)
        {
            Robots.Add(new Robot(Player, configuration));
        }
    }

    public void TakeRobot(Robot robot)
    {
        if(Player.RoleID == 79) LeaveRobot();
        
        Robots.Remove(robot);
        _currentTakenRobot = robot.RobotName;
        robot.ReplaceWithPlayer(Player);
    }
    
    public void LeaveRobot()
    {
        if (Player.RoleID != 79) return;

        Robots.Add(new Robot(Player, _currentTakenRobot));
        Player.RoleID = (uint)RoleType.Scp079;
    }

    private void Awake()
    {
        Player = GetComponent<SynapsePlayer>();
        Plugin = Synapse.Get<Scp079Robot>();
    }

    private void OnDestroy()
    {
        foreach (var robot in Robots.ToList())
        {
            robot.Destroy();
        }
        
        Robots.Clear();
    }
}