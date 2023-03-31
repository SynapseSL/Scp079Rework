using System;
using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Neuron.Modules.Commands.Event;
using PlayerRoles;

namespace Scp079Rework;

public class Scp079CommandService : Service
{
    private readonly CommandService _commandService;
    private readonly Scp079Rework _module;

    public Scp079CommandService(CommandService commandService, Scp079Rework module)
    {
        _commandService = commandService;
        _module = module;
    }
    
    public CommandReactor Scp079Commands { get; private set; }

    public override void Enable()
    {
        Scp079Commands = _commandService.CreateCommandReactor();
        Scp079Commands.NotFoundFallbackHandler = NotFound;

        while (_module.ModuleCommandBindingQueue.Count != 0)
        {
            var binding = _module.ModuleCommandBindingQueue.Dequeue();
            LoadBinding(binding);
        }
    }

    public void LoadBinding(Scp079Rework.Scp079CommandBinding binding) => RegisterCommand(binding.Type);

    public void RegisterCommand<TCommand>() where TCommand : Scp079Command
    {
        Scp079Commands.RegisterCommand<TCommand>();
    }

    public void RegisterCommand(Type type)
    {
        if(!typeof(Scp079Command).IsAssignableFrom(type)) return;
        Scp079Commands.RegisterCommand(type);
    }

    private CommandResult NotFound(CommandEvent args)
    {
        var context = args.Context as Scp079Context;
        var isContextProper = context != null;
        
        if (isContextProper && context.Scp079.RoleType != RoleTypeId.Scp079)
        {
            return new CommandResult()
            {
                Response = _module.Translation.Get(context.Scp079).Not079,
                StatusCode = CommandStatusCode.Forbidden
            };
        }
        
        return new CommandResult()
        {
            Response = isContextProper ? _module.Translation.Get(context.Scp079).NoSubCommand : _module.Translation.Get().NoSubCommand,
            StatusCode = CommandStatusCode.NotFound
        };
    }
}