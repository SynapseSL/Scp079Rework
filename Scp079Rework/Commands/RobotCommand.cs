using Synapse.Command;
using MEC;
using UnityEngine;

namespace Scp079Rework.Commands
{
    public class RobotCommand : I079Command
    {
        public int RequiredLevel => PluginExtensions.GetRequiredLevel(Name, 3);

        public float Energy => PluginExtensions.GetEnergy(Name, 100f);

        public string Name => "robot";

        public string Description => "A Command which spawns you as robot into the Map";

        public KeyCode Key => KeyCode.None;

        public float Exp => PluginExtensions.GetExp(Name, 1f);

        public float Cooldown => PluginExtensions.GetCooldown(Name, 0f);

        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();
            
            if(context.Arguments.Count < 1 || string.IsNullOrWhiteSpace(context.Arguments.At(0)))
            {
                result.Message = "All Robots:";
                if (context.Player.RoleID == 79)
                    result.Message += $"\n.079 robot 079 => Destroys your current robot";
                result.Message += $"\n.079 robot C => Chaos robots: {Scp079Robot.Scp079CRobot}";
                result.Message += $"\n.079 robot D => Dboy robots: {Scp079Robot.Scp079DRobot}";
                result.Message += $"\n.079 robot S => Scientist robots: {Scp079Robot.Scp079SRobot}";
                result.Message += $"\n.079 robot G => Guard robots: {Scp079Robot.Scp079GRobot}";
                result.Message += $"\n.079 robot M => MTF robots: {Scp079Robot.Scp079MRobot}";
                //I use No Permission so that no Energy get removed
                result.State = CommandResultState.NoPermission;
                return result;
            }

            RoleType role;
            switch (context.Arguments.At(0).ToUpper())
            {
                case "079" when context.Player.RoleID == 79:
                    context.Player.Kill();
                    result.State = CommandResultState.NoPermission;
                    result.Message = "You have destroyed your current robot";
                    return result;
                case "C": role = RoleType.ChaosRifleman; break;
                case "D": role = RoleType.ClassD; break;
                case "S": role = RoleType.Scientist; break;
                case "G": role = RoleType.FacilityGuard; break;
                case "M": role = RoleType.NtfSergeant; break;
                default:
                    result.Message = $"No Robot was found for {context.Arguments.At(0)}";
                    result.State = CommandResultState.Error;
                    return result;
            }

            if (CanSpawnRobot(role))
            {
                if (context.Player.RoleID == 79)
                {
                    context.Player.Kill();
                    Timing.CallDelayed(0.1f, () => context.Player.CustomRole = new Scp079Robot(role));
                }
                else
                    context.Player.CustomRole = new Scp079Robot(role);
                result.Message = $"You are now a {role}-robot";
                return result;
            }

            result.Message = "You don't have acces to a robot of that type!";
            result.State = CommandResultState.Error;
            return result;
        }

        private bool CanSpawnRobot(RoleType role)
        {
            switch (role)
            {
                case RoleType.ClassD:
                    if (Scp079Robot.Scp079DRobot >= 1)
                    {
                        Scp079Robot.Scp079DRobot--;
                        return true;
                    }
                    return false;

                case RoleType.Scientist:
                    if (Scp079Robot.Scp079SRobot >= 1)
                    {
                        Scp079Robot.Scp079SRobot--;
                        return true;
                    }
                    return false;

                case RoleType.FacilityGuard:
                    if (Scp079Robot.Scp079GRobot >= 1)
                    {
                        Scp079Robot.Scp079GRobot--;
                        return true;
                    }
                    return false;

                case RoleType.ChaosRifleman:
                    if (Scp079Robot.Scp079CRobot >= 1)
                    {
                        Scp079Robot.Scp079CRobot--;
                        return true;
                    }
                    return false;

                case RoleType.NtfSergeant:
                    if (Scp079Robot.Scp079MRobot >= 1)
                    {
                        Scp079Robot.Scp079MRobot--;
                        return true;
                    }
                    return false;
                default: return false;
            }
        }
    }
}
