using Synapse.Api.Plugin;
using System.Reflection;
using System.IO;
using System;

namespace Scp079Rework
{
    [PluginInformation(
        Name = "Scp079Rework",
        Author = "Dimenzio",
        Description = "A Plugin which gives Scp-079 a lot of new Features",
        LoadPriority = 10,
        SynapseMajor = 2,
        SynapseMinor = 2,
        SynapsePatch = 0,
        Version = "v.2.0.0"
        )]
    public class PluginClass : AbstractPlugin
    {
        [Config(section = "Scp079Rework")]
        public static PluginConfig Config { get; set; }

        public override void Load()
        {
            SynapseController.Server.RoleManager.RegisterCustomRole<Scp079Robot>();
            Commands.CommandHandler.Handler.RegisterCommands(Assembly.GetExecutingAssembly());

            foreach(var path in Directory.GetFiles(this.PluginDirectory,"*.dll"))
            {
                try
                {
                    var assembly = Assembly.Load(File.ReadAllBytes(path));
                    Commands.CommandHandler.Handler.RegisterCommands(assembly);
                }
                catch(Exception e)
                {
                    Synapse.Api.Logger.Get.Info($"Error while loading Commands of Assembly\n{path}\n{e}");
                }
            }

            new EventHandlers();
        }
    }
}
