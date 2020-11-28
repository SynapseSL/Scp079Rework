namespace Scp079Rework
{
    public interface I079Command
    {
        UnityEngine.KeyCode Key { get; }

        int RequiredLevel { get; }

        float Energy { get; }

        float Exp { get; }

        string Name { get; }

        string Description { get; }

        Synapse.Command.CommandResult Execute(Synapse.Command.CommandContext context);
    }
}
