using System;
using System.Collections.Generic;
using System.Linq;
using Neuron.Core.Meta;
using Neuron.Core.Modules;
using Neuron.Core.Plugins;
using Neuron.Modules.Commands;
using Neuron.Modules.Configs;
using Ninject;
using Synapse3.SynapseModule;

namespace Scp079Rework;

[Module(
    Name = "SCP-079 Rework",
    Author = "Dimenzio",
    Dependencies = new []
    {
        typeof(Synapse),
        typeof(CommandsModule),
        typeof(ConfigsModule)
    },
    Description = "A Module that allows Plugins to create new Commands for SCP-079",
    Version = "3.0.0",
    Repository = "https://github.com/SynapseSL/Scp079Rework"
    )]
public class Scp079Rework : ReloadableModule<Scp079ReworkConfig, Scp079ReworkTranslation>
{
    public Scp079CommandService Scp079CommandService { get; private set; }

    public override void Load(IKernel kernel)
    {
        //Kernel is a ShortCut Synapse.Get would be the same
        kernel.Get<MetaManager>().MetaGenerateBindings.Subscribe(GenerateCommandBinding);
        kernel.Get<PluginManager>().PluginLoadLate.Subscribe(OnPluginLoadLate);
        kernel.Get<ModuleManager>().ModuleLoadLate.Subscribe(OnModuleLoadLate);
    }

    public override void EnableModule()
    {
        Scp079CommandService = Synapse.Get<Scp079CommandService>();
        Logger.Info("SCP-079 Rework was Enabled");
    }

    private void GenerateCommandBinding(MetaGenerateBindingsEvent args)
    {
        if(!args.MetaType.TryGetAttribute<AutomaticAttribute>(out _)) return;
        if(!args.MetaType.TryGetAttribute<Scp079CommandAttribute>(out _)) return;
        if(!args.MetaType.Is<Scp079Command>()) return;
        
        args.Outputs.Add(new Scp079CommandBinding()
        {
            Type = args.MetaType.Type
        });
    }

    private void OnPluginLoadLate(PluginLoadEvent args)
    {
        args.Context.MetaBindings.OfType<Scp079CommandBinding>().ToList()
            .ForEach(x => Scp079CommandService.LoadBinding(x));
    }

    internal readonly Queue<Scp079CommandBinding> ModuleCommandBindingQueue = new();

    private void OnModuleLoadLate(ModuleLoadEvent args)
    {
        args.Context.MetaBindings.OfType<Scp079CommandBinding>().ToList()
            .ForEach(x => ModuleCommandBindingQueue.Enqueue(x));
    }
    
    public class Scp079CommandBinding : IMetaBinding
    {
        public Type Type { get; set; }

        public IEnumerable<Type> PromisedServices => new Type[] { };
    }
}
