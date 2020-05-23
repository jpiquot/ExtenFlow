using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace ExtenFlow.Domain.Aggregates
{
    /// <summary>
    /// Class AggregateRoot. Implements the <see cref="ExtenFlow.Domain.IAggregateRoot"/>
    /// </summary>
    /// <seealso cref="ExtenFlow.Domain.IAggregateRoot"/>
    public abstract class AggregateRoot : IAggregateRoot
    {
        private readonly IRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="repository"></param>
        public AggregateRoot(string name, string id, IRepository repository)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the aggregate identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; }

        /// <summary>
        /// Gets aggregate name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>The repository.</value>
        protected IRepository Repository => _repository;

        /// <summary>
        /// Handles commands.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;IList&lt;IEvent&gt;&gt;.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public virtual Task<IList<IEvent>> HandleCommand(ICommand command)
            => throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.InvalidCommand, command?.GetType().Name, GetType().Name));

        /// <summary>
        /// Handles events.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords

        public abstract Task HandleEvent(IEvent @event);

#pragma warning restore CA1716 // Identifiers should not match keywords

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