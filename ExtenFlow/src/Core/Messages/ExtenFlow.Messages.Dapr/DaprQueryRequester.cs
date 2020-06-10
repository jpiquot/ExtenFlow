using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Client;

using ExtenFlow.Messages.Queries;

namespace ExtenFlow.Messages.Dapr
{
    /// <summary>
    /// Class DaprQueryRequester. Implements the <see cref="ExtenFlow.Messages.Queries.IQueryRequester"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Messages.Queries.IQueryRequester"/>
    public class DaprQueryRequester : IQueryRequester
    {
        private readonly JsonSerializerOptions _options;
        private DaprClient? _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaprQueryRequester"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public DaprQueryRequester(JsonSerializerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private DaprClient Client => _client ??
                    (_client = new DaprClientBuilder()
                .UseJsonSerializationOptions(_options)
                .Build()
            );

        /// <summary>
        /// Ask for a query result. The query execution is synchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">Query to be executed by the application.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task&lt;T&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<T> Ask<T>(IQuery<T> query, CancellationToken? cancellationToken = default)
        {
            T value = (cancellationToken == null) ?
                await Client.InvokeMethodAsync<IQuery<T>, T>(nameof(IQueryRequester), nameof(Ask), query) :
                await Client.InvokeMethodAsync<IQuery<T>, T>(nameof(IQueryRequester), nameof(Ask), query, null, cancellationToken.Value);
            return value;
        }

        /// <summary>
        /// Ask for a query result. The query execution is synchronous.
        /// </summary>
        /// <param name="query">Query to be executed by the application.</param>
        /// <param name="cancellationToken">
        /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<object> Ask(IQuery query, CancellationToken? cancellationToken = null) => throw new NotImplementedException();

        /// <summary>
        /// Ask for the result of an asynchronous query (submitted with the Send method).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryId">The query id of the sent query.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task&lt;T&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<T> AskForResult<T>(string queryId, CancellationToken? cancellationToken = default)
        {
            T value = (cancellationToken == null) ?
                await Client.InvokeMethodAsync<string, T>(nameof(IQueryRequester), nameof(AskForResult), queryId) :
                await Client.InvokeMethodAsync<string, T>(nameof(IQueryRequester), nameof(AskForResult), queryId, null, cancellationToken.Value);
            return value;
        }

        /// <summary>
        /// Send a query. The query execution is asynchronous.
        /// </summary>
        /// <param name="query">The query to be sent.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task Send(IQuery query, CancellationToken? cancellationToken = default)
            => (cancellationToken == null) ?
                Client.PublishEventAsync(nameof(IQueryRequester) + "." + nameof(Send), query) :
                Client.PublishEventAsync(nameof(IQueryRequester) + "." + nameof(Send), query, cancellationToken.Value);
    }
}