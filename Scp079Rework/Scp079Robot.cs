using System.Collections.Generic;
using Synapse.Api;

namespace Scp079Rework
{
    public class Scp079Robot : Synapse.Api.Roles.Role
    {
        public Scp079Robot() => _role = RoleType.ClassD;

        public Scp079Robot(RoleType role) => _role = role;

        private RoleType _role;

        public override int GetRoleID() => 79;

        public override string GetRoleName() => "Scp079Robot";

        public override int GetTeamID() => (int)Team.SCP;

        public override List<int> GetFriendsID() => PluginClass.Config.RobotFF ? new List<int> { } : new List<int> { (int)Team.SCP };

        public override List<int> GetEnemiesID() => new List<int> { (int)Team.RSC, (int)Team.CDP, (int)Team.MTF };

        internal bool _bypass = false;

        public override void Spawn()
        {
            Player.Position = PluginClass.Config.RobotSpawn.Parse().Position;
            Player.RoleType = _role;
            Player.MaxHealth = PluginClass.Config.Health;
            Player.Health = PluginClass.Config.Health;
            PluginClass.Config.Inventory.Apply(Player);

            _bypass = Player.Bypass;
            Player.Bypass = true;
        }

        public override void DeSpawn()
        {
            Player.Bypass = _bypass;
            if (Map.Get.HeavyController.Is079Recontained) return;

            Player.RoleID = (int)RoleType.Scp079;
        }

        public static int Scp079DRobot = 1;

        public static int Scp079SRobot = 1;

        public static int Scp079GRobot = 1;

        public static int Scp079CRobot = 1;

        public static int Scp079MRobot = 1;
    }
}
