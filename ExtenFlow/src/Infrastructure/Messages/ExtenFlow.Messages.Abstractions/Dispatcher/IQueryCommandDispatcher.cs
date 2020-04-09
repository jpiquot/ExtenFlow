namespace ExtenFlow.Messages
{
    /// <summary>
    /// The query and command dispatcher interface
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.IQueryDispatcher"/>
    /// <seealso cref="ExtenFlow.Messages.ICommandDispatcher"/>
    public interface IQueryCommandDispatcher : IQueryDispatcher, ICommandDispatcher
    {
    }
}