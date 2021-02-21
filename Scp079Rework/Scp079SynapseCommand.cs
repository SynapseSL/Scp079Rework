using Synapse.Command;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CommandHandler = Scp079Rework.Handlers.CommandHandler;

namespace Scp079Rework
{
    [CommandInformation(
        Name = "079",
        Aliases = new[] { "scp079"},
        Description = "A Command for Scp079 to do much stuff",
        Permission = "",
        Platforms = new[] {Platform.ClientConsole},
        Usage = "Use .079 for an Help"
        )]
    public class Scp079SynapseCommand : ISynapseCommand
    {
        internal static readonly Dictionary<string, float> cooldown = new Dictionary<string, float>();

        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();

            if(context.Player.RoleID != 79 && context.Player.RoleID != (int)RoleType.Scp079)
            {
                result.Message = PluginClass.Translation.ActiveTranslation.Not079;
                result.State = CommandResultState.NoPermission;
                return result;
            }

            if(context.Arguments.Count < 1 || string.IsNullOrWhiteSpace(context.Arguments.At(0)))
            {
                result.Message = PluginClass.Translation.ActiveTranslation.Help;
                foreach (var command in CommandHandler.Handler.commands)
                    result.Message += $"\n.079 {command.Name}\n    Description: {command.Description}\n    KeyCode:{command.Key}";
                result.State = CommandResultState.Ok;
                return result;
            }

            if (CommandHandler.Handler.GetCommand(context.Arguments.At(0),out var cmd))
            {
                try
                {
                    if(cooldown.TryGetValue(cmd.Name,out var time) && Time.time < time)
                    {
                        result.Message = PluginClass.Translation.ActiveTranslation.Cooldown.Replace("%seconds%", Math.Round(time - Time.time).ToString());
                        result.State = CommandResultState.NoPermission;
                        return result;
                    }
                    if (cmd.RequiredLevel > context.Player.Scp079Controller.Level && !GetBypass(context.Player))
                    {
                        result.Message = PluginClass.Translation.ActiveTranslation.LowLevel.Replace("%level%", cmd.RequiredLevel.ToString());
                        result.State = CommandResultState.NoPermission;
                        return result;
                    }
                    if (cmd.Energy > context.Player.Scp079Controller.Energy && !GetBypass(context.Player))
                    {
                        result.Message = PluginClass.Translation.ActiveTranslation.LowEnergy.Replace("%energy%", cmd.Energy.ToString());
                        result.State = CommandResultState.NoPermission;
                        return result;
                    }
                    var newcontext = new CommandContext()
                    {
                        Arguments = context.Arguments.ToArray().Segment(1),
                        Platform = context.Platform,
                        Player = context.Player,
                    };

                    result = cmd.Execute(newcontext);

                    if (result.State == CommandResultState.Ok)
                    {
                        if (!context.Player.Bypass)
                            context.Player.Scp079Controller.Energy -= cmd.Energy;

                        context.Player.Scp079Controller.GiveExperience(cmd.Exp);
                        if (cooldown.Keys.Any(x => x == cmd.Name))
                            cooldown.Remove(cmd.Name);
                        cooldown.Add(cmd.Name, Time.time + cmd.Cooldown);
                    }

                    return result;
                }
                catch(Exception e)
                {
                    Synapse.Api.Logger.Get.Error("Command executing failed: " + e.ToString());
                    result.Message = PluginClass.Translation.ActiveTranslation.Error;
                    result.State = CommandResultState.Error;
                    return result;
                }
            }
            result.Message = PluginClass.Translation.ActiveTranslation.NoCommand.Replace("%command%", context.Arguments.At(0));
            result.State = CommandResultState.Error;
            return result;
        }

        private bool GetBypass(Synapse.Api.Player player)
        {
            if (player.RoleID != 79) return player.Bypass;
            return (player.CustomRole as Scp079Robot)._bypass;
        }
    }
}
