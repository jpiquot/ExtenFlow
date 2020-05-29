using System.Threading.Tasks;

namespace ExtenFlow.Messages.Commands
{
    /// <summary>
    /// Interface IQueryHandler
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TResponse">The type of the query response.</typeparam>
    public interface IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        /// <summary>
        /// Asks the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;TResponse&gt;.</returns>
        Task<TResponse> Ask(TQuery query);
    }
}