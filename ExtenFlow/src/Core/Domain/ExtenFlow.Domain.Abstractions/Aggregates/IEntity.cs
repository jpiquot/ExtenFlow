using System.Threading.Tasks;

namespace ExtenFlow.Domain.Aggregates
{
    /// <summary>
    /// Interface IDomainEntity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        string EntityName { get; }

        /// <summary>
        /// Gets the aggregate identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; }

        /// <summary>
        /// Handles events.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task.</returns>
        Task HandleEvent(IEvent message);
    }
}