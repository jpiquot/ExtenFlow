namespace ExtenFlow.Messages
{
    /// <summary>
    /// The base interface for all queries
    /// </summary>
    /// <typeparam name="T">The type of the query result</typeparam>
    public interface IQuery<T> : IRequest
    {
    }
}