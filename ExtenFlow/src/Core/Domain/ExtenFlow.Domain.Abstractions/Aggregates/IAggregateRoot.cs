using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExtenFlow.Domain
{
    /// <summary>
    /// Interface IDomainAggregateRoot
    /// </summary>
    public interface IAggregateRoot
    {
        /// <summary>
        /// Gets the aggregate identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; }

        /// <summary>
        /// Gets aggregate name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Handles commands.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;IList&lt;IEvent&gt;&gt;.</returns>
        Task<IList<IEvent>> HandleCommand(ICommand command);

        /// <summary>
        /// Handles events.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task HandleEvent(IEvent message);

        /// <summary>
        /// Handles notifications.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task HandleNotification(IMessage message);

        /// <summary>
        /// Handles queries that need strong consistency.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        Task<object> HandleQuery(IQuery query);
    }
}