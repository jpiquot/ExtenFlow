using System.Threading.Tasks;
using ExtenFlow.Messages;
using Microsoft.AspNetCore.SignalR;

namespace ExtenFlow.Identity.Web
{
    /// <summary>
    /// Class QueryHub. Implements the <see cref="Microsoft.AspNetCore.SignalR.Hub"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.SignalR.Hub"/>
    public class QueryHub : Hub
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="queryId">The user.</param>
        /// <param name="result">The message.</param>
        public Task QueryResult(string queryId, object result)
            => Clients.All.SendAsync(nameof(QueryResult), queryId, result);
    }
}