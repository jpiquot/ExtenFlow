namespace ExtenFlow.Domain
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
        string EntityTypeName { get; }
    }
}