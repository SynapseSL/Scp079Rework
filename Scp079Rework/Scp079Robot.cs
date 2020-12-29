using System.Collections.Generic;
using MEC;
using Synapse.Api;
using System.Linq;

namespace Scp079Rework
{
    public class Scp079Robot : Synapse.Api.Roles.Role
    {
        public Scp079Robot() => _role = RoleType.ClassD;

        public Scp079Robot(RoleType role) => _role = role;

        private RoleType _role;

        public override int GetRoleID() => 79;

        public override string GetRoleName() => "Scp079Robot";

        public override Team GetTeam() => Team.SCP;

        public override List<Team> GetFriends() => PluginClass.Config.RobotFF ? new List<Team> { } : new List<Team> { Team.SCP };

        public override List<Team> GetEnemys() => new List<Team> { Team.RSC, Team.CDP, Team.MTF };

        internal bool _bypass = false;

        public override void Spawn()
        {
            Player.Position = PluginClass.Config.RobotSpawn.Parse().Position;
            Player.ChangeRoleAtPosition(_role);
            Player.MaxHealth = PluginClass.Config.Health;
            Player.Health = PluginClass.Config.Health;
            Player.Inventory.Clear();
            foreach (var item in PluginClass.Config.Inventory)
                Player.Inventory.AddItem(item.Parse());

            _bypass = Player.Bypass;
            Player.Bypass = true;
        }

        public override void DeSpawn()
        {
            Player.Bypass = _bypass;
            if (Map.Get.HeavyController.Is079Recontained) return;

            var locked = SynapseController.Server.Map.Round.RoundLock;
            SynapseController.Server.Map.Round.RoundLock = true;
            Timing.CallDelayed(0.1f, () =>
             {
                 if (Player.RoleID == (int)RoleType.Spectator)
                 {
                     Player.RoleID = (int)RoleType.Scp079;
                     NineTailedFoxAnnouncer.CheckForZombies(SynapseController.Server.Host.gameObject);
                 }
                 SynapseController.Server.Map.Round.RoundLock = locked;
             });
        }

        public static int Scp079DRobot = 1;

        public static int Scp079SRobot = 1;

        public static int Scp079GRobot = 1;

        public static int Scp079CRobot = 1;

        public static int Scp079MRobot = 1;
    }
}
