using System.Linq;

namespace Scp079Rework
{
    public static class PluginExtensions
    {
        public static int GetRequiredLevel(string commandName, int def)
        {
            var config = PluginClass.Config.CommandConfigurations.FirstOrDefault(x => x.Name.ToLower() == commandName.ToLower());
            if (config == null) return def;
            return config.RequiredLevel;
        }

        public static float GetEnergy(string commandName, float def)
        {
            var config = PluginClass.Config.CommandConfigurations.FirstOrDefault(x => x.Name.ToLower() == commandName.ToLower());
            if (config == null) return def;
            return config.Energy;
        }

        public static float GetExp(string commandName, float def)
        {
            var config = PluginClass.Config.CommandConfigurations.FirstOrDefault(x => x.Name.ToLower() == commandName.ToLower());
            if (config == null) return def;
            return config.Exp;
        }

        public static float GetCooldown(string commandName, float def)
        {
            var config = PluginClass.Config.CommandConfigurations.FirstOrDefault(x => x.Name.ToLower() == commandName.ToLower());
            if (config == null) return def;
            return config.Cooldown;
        }
    }
}
