namespace ExtenFlow.Domain
{
    /// <summary>
    /// The query and command dispatcher interface
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.IQueryDispatcher"/>
    /// <seealso cref="ExtenFlow.Domain.ICommandDispatcher"/>
    public interface IQueryCommandDispatcher : IQueryDispatcher, ICommandDispatcher
    {
    }
}