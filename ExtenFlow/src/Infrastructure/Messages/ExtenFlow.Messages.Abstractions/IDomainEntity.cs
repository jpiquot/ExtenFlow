using System.Collections.Generic;

namespace ExtenFlow.Messages
{
    /// <summary>
    /// Interface IDomainEntity
    /// </summary>
    public interface IDomainEntity
    {
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>The events.</value>
        ICollection<IEvent> Events { get; }
    }
}