using System;
using System.Collections.Generic;
using System.Globalization;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Synapse3.SynapseModule;
using UnityEngine;

namespace Scp079Rework;

public abstract class Scp079Command : Command<Scp079Context>
{
    public float CurrentCooldown { get; set; } = 0f;
    
    public override CommandResult PreExecute(Scp079Context context)
    {
        if (context.Scp079.RoleType != RoleType.Scp079)
        {
            return new CommandResult()
            {
                StatusCode = CommandStatusCode.Forbidden,
                Response = Synapse.Get<Scp079ReworkTranslation>().Get(context.Scp079).Not079,
            };
        }
        
        var module = Synapse.Get<Scp079Rework>();
        if (Time.time < CurrentCooldown && !context.Scp079.Bypass)
        {
            return new CommandResult()
            {
                StatusCode = CommandStatusCode.Forbidden,
                Response = module.Translation.Get(context.Scp079).CooldownActive
                    .Replace("%time%", Math.Round(CurrentCooldown - Time.time).ToString(CultureInfo.InvariantCulture))
            };
        }

        var level = GetRequiredLevel(module);
        if (level > context.Scp079.ScpController.Scp079.Level && !context.Scp079.Bypass)
        {
            return new CommandResult()
            {
                StatusCode = CommandStatusCode.Forbidden,
                Response = module.Translation.Get(context.Scp079).LevelToLow.Replace("%level%", level.ToString(CultureInfo.InvariantCulture))
            };
        }

        var neededEnergy = GetRequiredEnergy(module);
        if (neededEnergy > context.Scp079.ScpController.Scp079.Energy && !context.Scp079.Bypass)
        {
            return new CommandResult()
            {
                StatusCode = CommandStatusCode.Forbidden,
                Response = module.Translation.Get(context.Scp079).EnergyToLow
                    .Replace("%energy%", neededEnergy.ToString(CultureInfo.InvariantCulture))
            };
        }

        return null;
    }

    public sealed override void Execute(Scp079Context context, ref CommandResult result)
    {
        ExecuteCommand(context, ref result);
        AfterExecution(context, ref result);
    }

    public abstract void ExecuteCommand(Scp079Context context, ref CommandResult result);

    public virtual void AfterExecution(Scp079Context context, ref CommandResult result)
    {
        if (result.StatusCode != CommandStatusCode.Ok) return;
        var module = Synapse.Get<Scp079Rework>();

        if (!context.Scp079.Bypass)
        {
            context.Scp079.ScpController.Scp079.Energy -= GetRequiredEnergy(module);
            CurrentCooldown = Time.time + GetCooldown(module);
        }
        context.Scp079.ScpController.Scp079.GiveExperience(GetExperienceGain(module));
    }

    public int GetRequiredLevel(Scp079Rework module)
    {
        foreach (var configuration in module.Config.CommandConfigurations)
        {
            var wordsToCheck = new List<string>() { Meta.CommandName };
            wordsToCheck.AddRange(Meta.Aliases);

            foreach (var word in wordsToCheck)
            {
                if (string.Equals(word, configuration.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return configuration.RequiredLevel;
                }
            }
        }

        return (Meta as Scp079CommandAttribute)?.RequiredLevel ?? 0;
    }
    
    public float GetRequiredEnergy(Scp079Rework module)
    {
        foreach (var configuration in module.Config.CommandConfigurations)
        {
            var wordsToCheck = new List<string>() { Meta.CommandName };
            wordsToCheck.AddRange(Meta.Aliases);

            foreach (var word in wordsToCheck)
            {
                if (string.Equals(word, configuration.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return configuration.EnergyUsage;
                }
            }
        }

        return (Meta as Scp079CommandAttribute)?.EnergyUsage ?? 0f;
    }
    
    public float GetExperienceGain(Scp079Rework module)
    {
        foreach (var configuration in module.Config.CommandConfigurations)
        {
            var wordsToCheck = new List<string>() { Meta.CommandName };
            wordsToCheck.AddRange(Meta.Aliases);

            foreach (var word in wordsToCheck)
            {
                if (string.Equals(word, configuration.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return configuration.ExperienceGain;
                }
            }
        }

        return (Meta as Scp079CommandAttribute)?.ExperienceGain ?? 0f;
    }
    
    public float GetCooldown(Scp079Rework module)
    {
        foreach (var configuration in module.Config.CommandConfigurations)
        {
            var wordsToCheck = new List<string>() { Meta.CommandName };
            wordsToCheck.AddRange(Meta.Aliases);

            foreach (var word in wordsToCheck)
            {
                if (string.Equals(word, configuration.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return configuration.Cooldown;
                }
            }
        }

        return (Meta as Scp079CommandAttribute)?.Cooldown ?? 0f;
    }
}