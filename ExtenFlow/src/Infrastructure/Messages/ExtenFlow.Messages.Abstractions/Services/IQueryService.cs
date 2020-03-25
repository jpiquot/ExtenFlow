using System.Threading;
using System.Threading.Tasks;

namespace ExtenFlow.Messages.Services
{
    /// <summary>
    /// The query service interface
    /// </summary>
    public interface IQueryService
    {
        /// <summary>
        /// Submit the query
        /// </summary>
        /// <typeparam name="T">The query result type</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The query result</returns>
        Task<T> Invoke<T>(IQuery<T> query, CancellationToken cancellationToken = default);
    }
}