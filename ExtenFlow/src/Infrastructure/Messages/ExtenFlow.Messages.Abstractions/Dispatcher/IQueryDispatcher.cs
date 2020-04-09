using System.Threading.Tasks;

namespace ExtenFlow.Messages
{
    /// <summary>
    /// Query dispatcher interface
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Asks the specified query.
        /// </summary>
        /// <typeparam name="T">The response type</typeparam>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        Task<T> Ask<T>(IQuery<T> query);
    }
}