using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Neuron.Modules.Commands.Event;
using Synapse3.SynapseModule.Command;

namespace Scp079Rework;

[Automatic]
[SynapseCommand(
    CommandName = "079",
    Aliases = new []{ "Scp079" },
    Description = "A Command for SCP-079 to activate various different Abilities",
    Platforms = new [] { CommandPlatform.PlayerConsole }
    )]
public class Scp079MainCommand : SynapseCommand
{
    public CommandReactor.FallbackHandler NoCommandResponse;
    
    private readonly Scp079CommandService _commandService;
    private readonly Scp079Rework _module;

    public Scp079MainCommand(Scp079CommandService commandService, Scp079Rework module)
    {
        _commandService = commandService;
        _module = module;
        
        NoCommandResponse = DefaultHelp;
    }
    
    public override void Execute(SynapseContext context, ref CommandResult result)
    {
        var newCommand = string.Join(" ", context.Arguments);

        if (string.IsNullOrWhiteSpace(newCommand))
        {
            var noCommandResult = NoCommandResponse.Invoke(new CommandEvent()
            {
                Context = context,
            });

            result.Response = noCommandResult.Response;
            result.StatusCodeInt = noCommandResult.StatusCodeInt;
            
            return;
        }
        
        var commandResult = _commandService.Scp079Commands.Invoke(Scp079Context.Of(newCommand, context.Player), newCommand);
        result.Response = commandResult.Response;
        result.StatusCodeInt = commandResult.StatusCodeInt;
    }

    public CommandResult DefaultHelp(CommandEvent args)
    {
        if (!_module.Config.ShowCommandListToOthers && args.Context is SynapseContext context && context.Player.RoleType != RoleType.Scp079)
        {
            return new CommandResult()
            {
                Response = _module.Translation.Get(context.Player).Not079,
                StatusCode = CommandStatusCode.Forbidden
            };
        }
        
        return new CommandResult()
        {
            Response = GenerateHelpList(),
            StatusCode = CommandStatusCode.Ok
        };
    }

    public string GenerateHelpList()
    {
        var msg = "\n" + _module.Translation.CommandList;

        foreach (var command in _commandService.Scp079Commands.Handler.Commands)
        {
            msg += $"\n{command.Meta.CommandName} - {command.Meta.Description}";
        }

        return msg;
    }
}