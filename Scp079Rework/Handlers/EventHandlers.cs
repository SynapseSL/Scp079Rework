using Event = Synapse.Api.Events.EventHandler;
using UnityEngine;
using System.Linq;
using System;

namespace Scp079Rework.Handlers
{
    public class EventHandlers
    {
        public EventHandlers()
        {
            Event.Get.Player.PlayerKeyPressEvent += OnKeyPress;
            Event.Get.Player.PlayerDeathEvent += OnDeath;
            Event.Get.Round.WaitingForPlayersEvent += OnWaitng;
            Event.Get.Player.PlayerSetClassEvent += OnSetClass;
        }

        private void OnSetClass(Synapse.Api.Events.SynapseEventArguments.PlayerSetClassEventArgs ev)
        {
            if(ev.Player.RoleID == 79)
            {
                ev.Position = PluginClass.Config.RobotSpawn.Parse().Position;

            }
        }

        private void OnWaitng()
        {
            Scp079Robot.Scp079DRobot = PluginClass.Config.Scp079DRobot;
            Scp079Robot.Scp079SRobot = PluginClass.Config.Scp079SRobot;
            Scp079Robot.Scp079GRobot = PluginClass.Config.Scp079GRobot;
            Scp079Robot.Scp079CRobot = PluginClass.Config.Scp079CRobot;
            Scp079Robot.Scp079MRobot = PluginClass.Config.Scp079MRobot;
        }

        private void OnDeath(Synapse.Api.Events.SynapseEventArguments.PlayerDeathEventArgs ev)
        {
            if (ev.Killer == null || ev.Killer == ev.Victim) return;

            if(ev.Victim.RoleID == 79)
                ev.Killer.SendBroadcast(3,PluginClass.Translation.ActiveTranslation.Killed079);
            if (ev.Killer.RoleID == 79)
                ev.Victim.OpenReportWindow(PluginClass.Translation.ActiveTranslation.KilledBy079.Replace("\\n", "\n"));
        }

        private void OnKeyPress(Synapse.Api.Events.SynapseEventArguments.PlayerKeyPressEventArgs ev)
        {
            if ((ev.Player.RoleID == (int)RoleType.Scp079 || ev.Player.RoleID == 79) && CommandHandler.Handler.GetCommand(ev.KeyCode, out var cmd)) 
            {
                if (Scp079SynapseCommand.cooldown.TryGetValue(cmd.Name, out var time) && Time.time < time)
                {
                    ev.Player.SendConsoleMessage(PluginClass.Translation.ActiveTranslation.Cooldown.Replace("%seconds%", Math.Round(time - Time.time).ToString()));
                    return;
                }
                if (cmd.RequiredLevel > ev.Player.Scp079Controller.Level && !ev.Player.Bypass)
                {
                    ev.Player.SendConsoleMessage(PluginClass.Translation.ActiveTranslation.LowLevel.Replace("%level%", cmd.RequiredLevel.ToString()));
                    return;
                }
                if (cmd.Energy > ev.Player.Scp079Controller.Energy && !ev.Player.Bypass)
                {
                    ev.Player.SendConsoleMessage(PluginClass.Translation.ActiveTranslation.LowEnergy.Replace("%energy%", cmd.Energy.ToString()));
                    return;
                }

                var result = cmd.Execute(new Synapse.Command.CommandContext()
                {
                    Arguments = new System.ArraySegment<string>(),
                    Platform = Synapse.Command.Platform.ClientConsole,
                    Player = ev.Player
                });

                if (result.State == Synapse.Command.CommandResultState.Ok)
                {
                    if (!ev.Player.Bypass)
                        ev.Player.Scp079Controller.Energy -= cmd.Energy;

                    ev.Player.Scp079Controller.GiveExperience(cmd.Exp);
                    if (Scp079SynapseCommand.cooldown.Keys.Any(x => x == cmd.Name))
                        Scp079SynapseCommand.cooldown.Remove(cmd.Name);
                    Scp079SynapseCommand.cooldown.Add(cmd.Name, Time.time + cmd.Cooldown);
                }

                ev.Player.SendConsoleMessage(result.Message, "gray");
            }
        }
    }
}
