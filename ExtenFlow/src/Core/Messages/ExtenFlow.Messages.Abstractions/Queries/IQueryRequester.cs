using System.Threading;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Queries
{
    /// <summary>
    /// Defines an query requester
    /// </summary>
    public interface IQueryRequester
    {
        /// <summary>
        /// Ask for a query result. The query execution is synchronous.
        /// </summary>
        /// <param name="query">Query to be executed by the application.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<T> Ask<T>(IQuery<T> query, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Ask for the result of an asynchronous query (submitted with the Send method).
        /// </summary>
        /// <param name="queryId">The query id of the sent query.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<T> Ask<T>(string queryId, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Send a query. The query execution is asynchronous.
        /// </summary>
        /// <param name="query">The query to be sent.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task Send(IQuery query, CancellationToken? cancellationToken = default);
    }
}