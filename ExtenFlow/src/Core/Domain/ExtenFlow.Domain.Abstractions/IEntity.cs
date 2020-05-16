using System.Collections.Generic;

namespace ExtenFlow.Domain
{
    /// <summary>
    /// Interface IDomainEntity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        ICollection<IEvent> Events { get; }
    }
}