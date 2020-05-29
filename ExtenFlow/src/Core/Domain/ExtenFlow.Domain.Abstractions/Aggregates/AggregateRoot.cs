using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using ExtenFlow.Domain.Exceptions;
using ExtenFlow.Infrastructure.ValueObjects;
using ExtenFlow.Messages;

namespace ExtenFlow.Domain.Aggregates
{
    /// <summary>
    /// Class AggregateRoot. Implements the <see cref="ExtenFlow.Domain.Aggregates.Entity{T}"/>
    /// Implements the <see cref="ExtenFlow.Domain.Aggregates.IAggregateRoot"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ExtenFlow.Domain.Aggregates.Entity{T}"/>
    /// <seealso cref="ExtenFlow.Domain.Aggregates.IAggregateRoot"/>
    public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot
    {
        private ConcurrencyCheckStamp? _concurrencyCheckStamp;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot{T}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="repository"></param>
        protected AggregateRoot(string name, string id, IRepository repository)
            : base(name, id, repository)
        {
        }

        /// <summary>
        /// Gets the concurrency stamp. A random value that should change whenever a role is
        /// persisted to the store.
        /// </summary>
        /// <value>The concurrency stamp.</value>
        protected ConcurrencyCheckStamp ConcurrencyCheckStamp
            => _concurrencyCheckStamp ?? throw new EntityStateNotInitializedException(this, nameof(ConcurrencyCheckStamp));

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

        /// <summary>
        /// Checks the concurrency stamp.
        /// </summary>
        /// <exception cref="EntityConcurrencyCheckFailedException"></exception>
        protected void CheckCanCreate()
        {
            if (HasData)
            {
                throw new EntityDuplicateException(this);
            }
            if (ConcurrencyCheckStamp != null)
            {
                throw new EntityConcurrencyCheckFailedException(this);
            }
        }

        /// <summary>
        /// Checks the concurrency stamp.
        /// </summary>
        /// <param name="concurrencyStamp">The concurrency stamp.</param>
        /// <exception cref="EntityConcurrencyCheckFailedException"></exception>
        protected void CheckConcurrencyStamp(string? concurrencyStamp)
        {
            CheckExists();
            if (ConcurrencyCheckStamp.Value != concurrencyStamp)
            {
                throw new EntityConcurrencyCheckFailedException(this);
            }
        }

        /// <summary>
        /// Clears the concurrency check stamp.
        /// </summary>
        protected void ClearConcurrencyCheckStamp() => _concurrencyCheckStamp = null;

        /// <summary>
        /// Sets the concurrency check stamp.
        /// </summary>
        /// <param name="concurrencyStamp">The concurrency stamp.</param>
        protected void SetConcurrencyCheckStamp(string concurrencyStamp)
            => _concurrencyCheckStamp = new ConcurrencyCheckStamp(concurrencyStamp);
    }
}