namespace ExtenFlow.Messages
{
    /// <summary>
    /// Interface IDomainAggregateRoot
    /// </summary>
    public interface IDomainAggregateRoot
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
    }
}