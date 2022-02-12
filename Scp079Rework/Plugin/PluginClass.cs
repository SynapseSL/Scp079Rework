using Scp079Rework.Handlers;
using Synapse.Api.Plugin;
using Synapse.Translation;
using System;
using System.IO;
using System.Reflection;

namespace Scp079Rework
{
    [PluginInformation(
        Name = "Scp079Rework",
        Author = "Dimenzio",
        Description = "A Plugin which gives Scp-079 a lot of new Features",
        LoadPriority = 10,
        SynapseMajor = 2,
        SynapseMinor = 8,
        SynapsePatch = 3,
        Version = "v.2.2.1"
        )]
    public class PluginClass : AbstractPlugin
    {
        [Config(section = "Scp079Rework")]
        public static PluginConfig Config { get; set; }

        [SynapseTranslation]
        public static new SynapseTranslation<PluginTranslation> Translation { get; set; }

        public override void Load()
        {
            SynapseController.Server.RoleManager.RegisterCustomRole<Scp079Robot>();

            Translation.AddTranslation(new PluginTranslation());
            Translation.AddTranslation(new PluginTranslation
            {
                Not079 = "Du bist nicht SCP-079.",
                Help = "Alle SCP-079 Befehle:",
                Cooldown = "Du musst noch %seconds% Sekunden abwarten bevor du den Befehl wieder ausführen kannst.",
                LowLevel = "Dein Level ist nich hoch genug!Du brauchst mindestens Level %level%.",
                LowEnergy = "Du braucht mindestens %energy% Energie um diesen Befehl auszuführen.",
                Error = "Ein Fehler ist aufgetreten während dem Ausführen des Befehls.",
                NoCommand = "Kein solcher Unter befehl mit dem namen %command% wurde gefunden.",
                NotAsRobot = "Als SCP-079-Roboter kannst du diesen Befehl nicht ausführen"
            }, "GERMAN");

            CommandHandler.Handler.RegisterCommands(Assembly.GetExecutingAssembly());

            foreach(var path in Directory.GetFiles(this.PluginDirectory,"*.dll"))
            {
                try
                {
                    var assembly = Assembly.Load(File.ReadAllBytes(path));
                    CommandHandler.Handler.RegisterCommands(assembly);
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
