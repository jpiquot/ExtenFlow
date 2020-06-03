using System;
using System.Threading;
using System.Threading.Tasks;

using Paramore.Brighter;

namespace ExtenFlow.Messages.Brighter
{
    /// <summary>
    /// Class CommandStore. Implements the <see cref="Paramore.Brighter.IAmACommandStoreAsync"/>
    /// </summary>
    /// <seealso cref="Paramore.Brighter.IAmACommandStoreAsync"/>
    public class CommandStore : IAmACommandStoreAsync
    {
        /// <summary>
        /// Gets or sets a value indicating whether [continue on captured context].
        /// </summary>
        /// <value><c>true</c> if [continue on captured context]; otherwise, <c>false</c>.</value>
        public bool ContinueOnCapturedContext { get; set; }

        Task IAmACommandStoreAsync.AddAsync<T>(T command, string contextKey, int timeoutInMilliseconds, CancellationToken cancellationToken) => throw new NotImplementedException();

        Task<bool> IAmACommandStoreAsync.ExistsAsync<T>(Guid id, string contextKey, int timeoutInMilliseconds, CancellationToken cancellationToken) => throw new NotImplementedException();

        Task<T> IAmACommandStoreAsync.GetAsync<T>(Guid id, string contextKey, int timeoutInMilliseconds, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}