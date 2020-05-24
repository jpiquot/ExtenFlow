using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace ExtenFlow.Domain.Aggregates
{
    /// <summary>
    /// Class AggregateRoot. Implements the <see cref="Entity"/> Implements the <see cref="IAggregateRoot"/>
    /// </summary>
    /// <seealso cref="Entity"/>
    /// <seealso cref="IAggregateRoot"/>
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="repository"></param>
        protected AggregateRoot(string name, string id, IRepository repository)
            : base(name, id, repository)
        {
        }

        /// <summary>
        /// Handles commands.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;IList&lt;IEvent&gt;&gt;.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public virtual Task<IList<IEvent>> HandleCommand(ICommand command)
            => throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.InvalidCommand, command?.GetType().Name, GetType().Name));

        /// <summary>
        /// Handles notifications.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        public abstract Task HandleNotification(IMessage message);

        /// <summary>
        /// Handles queries that need strong consistency.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public virtual Task<object> HandleQuery(IQuery query)
            => throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.InvalidQuery, query?.GetType().Name, GetType().Name));
    }
}