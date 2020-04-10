namespace ExtenFlow.Messages
{
    /// <summary>
    /// The base interface for all queries
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.IRequest"/>
    public interface IQuery : IRequest
    {
    }

    /// <summary>
    /// The base interface for generic queries
    /// </summary>
    /// <typeparam name="T">The type of the query result</typeparam>
    public interface IQuery<T> : IQuery
    {
    }
}