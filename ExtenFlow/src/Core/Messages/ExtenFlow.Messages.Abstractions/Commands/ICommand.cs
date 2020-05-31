namespace ExtenFlow.Messages
{
    /// <summary>
    /// The base interface for all commands
    /// </summary>
    public interface ICommand : ExtenFlow.Messages.IRequest
    {
        /// <summary>
        /// Gets the concurrency check stamp.
        /// </summary>
        /// <value>The concurrency check stamp.</value>
        string ConcurrencyCheckStamp { get; }
    }
}