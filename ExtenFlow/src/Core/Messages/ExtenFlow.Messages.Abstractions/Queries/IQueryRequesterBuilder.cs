namespace ExtenFlow.Messages.Queries
{
    /// <summary>
    /// Interface IQueryRequesterBuilder
    /// </summary>
    public interface IQueryRequesterBuilder
    {
        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>IQueryRequester.</returns>
        IQueryRequester Build();
    }
}