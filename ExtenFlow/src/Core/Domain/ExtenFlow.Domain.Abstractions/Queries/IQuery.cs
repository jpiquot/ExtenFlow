namespace ExtenFlow.Domain
{
    /// <summary>
    /// The base interface for all queries
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.IRequest"/>
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