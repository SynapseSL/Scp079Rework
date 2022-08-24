using System;
using System.Linq;
using Neuron.Modules.Commands;
using Synapse3.SynapseModule.Player;

namespace Scp079Rework;

public class Scp079Context : ICommandContext
{
    public string Command { get; set; }
    public string[] Arguments { get; set; }
    public string FullCommand { get; set; }
    public bool IsAdmin => false;
    public Type ContextType => typeof(Scp079Context);
    
    public SynapsePlayer Scp079 { get; private set; }

    public static Scp079Context Of(string message, SynapsePlayer scp079)
    {
        var context = new Scp079Context()
        {
            FullCommand = message
        };
        var args = message.Split(' ').ToList();
        context.Command = args[0];
        args.RemoveAt(0);
        context.Arguments = args.ToArray();
        
        context.Scp079 = scp079;
        return context;
    }
}