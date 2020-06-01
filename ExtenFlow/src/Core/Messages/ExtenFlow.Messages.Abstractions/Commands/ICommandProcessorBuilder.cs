namespace ExtenFlow.Messages.Commands
{
    /// <summary>
    /// Defines a command processor builder.
    /// </summary>
    public interface ICommandProcessorBuilder
    {
        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>ICommandProcessor.</returns>
        ICommandProcessor Build();
    }
}