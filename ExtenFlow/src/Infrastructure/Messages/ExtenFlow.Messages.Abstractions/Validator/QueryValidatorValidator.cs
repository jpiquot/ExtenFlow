#pragma warning disable CA1710 // Identifiers should have correct suffix

namespace ExtenFlow.Messages.Validators
{
    /// <summary>
    /// Query validation
    /// </summary>
    public abstract class QueryValidator<T> : RequestValidator<T> where T : IQuery
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected QueryValidator(bool aggregateIdRequired = true) : base(aggregateIdRequired)
        {
        }
    }
}