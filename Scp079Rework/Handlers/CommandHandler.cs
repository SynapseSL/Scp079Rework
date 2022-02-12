using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scp079Rework.Handlers
{
    public class CommandHandler
    {
        public static CommandHandler Handler { get; } = new CommandHandler();

        public readonly List<I079Command> commands = new List<I079Command>();

        public void RegisterCommand(I079Command cmd)
        {
            if (commands.Any(x => x.Name.ToLower() == cmd.Name.ToLower())) throw new System.Exception();
            commands.Add(cmd);
        }

        public bool GetCommand(string name,out I079Command command)
        {
            command = commands.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            return command != null;
        }

        public bool GetCommand(UnityEngine.KeyCode keyCode,out I079Command command)
        {
            command = commands.FirstOrDefault(x => x.Key == keyCode);
            return command != null;
        }

        internal void RegisterCommands(Assembly assembly)
        {
            foreach(var commandclass in assembly.DefinedTypes.Where(x => x != typeof(I079Command) && typeof(I079Command).IsAssignableFrom(x)))
                try
                {
                    RegisterCommand(Activator.CreateInstance(commandclass) as I079Command);
                }
                catch(Exception e)
                {
                    Synapse.Api.Logger.Get.Error($"Registering Scp-079 Command for type {commandclass.Name} failed:\n\n {e}");
                }
        }
    }
}
